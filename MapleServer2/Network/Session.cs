using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Security.Cryptography;
using Maple2Storage.Extensions;
using MaplePacketLib2.Crypto;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Packets;
using NLog;

namespace MapleServer2.Network
{
    public abstract class Session : IDisposable
    {
        public const uint VERSION = 12;
        private const uint BLOCK_IV = 12; // TODO: should this be variable

        private const int HANDSHAKE_SIZE = 19;
        private const int STOP_TIMEOUT = 2000;

        public EventHandler<string> OnError;
        public EventHandler<PoolPacketReader> OnPacket;

        private bool Disposed;
        private uint Siv;
        private uint Riv;

        private string Name;
        private TcpClient Client;
        private NetworkStream NetworkStream;
        private MapleCipher.Encryptor SendCipher;
        private MapleCipher.Decryptor RecvCipher;

        private readonly Thread Thread;
        private readonly QueuedPipeScheduler PipeScheduler;
        private readonly Pipe RecvPipe;

        protected abstract PatchType Type { get; }
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

        protected Session()
        {
            Thread = new Thread(StartInternal);
            PipeScheduler = new QueuedPipeScheduler();
            PipeOptions options = new PipeOptions(
                readerScheduler: PipeScheduler,
                writerScheduler: PipeScheduler,
                useSynchronizationContext: false
            );
            RecvPipe = new Pipe(options);
        }

        public void Init([NotNull] TcpClient client)
        {
            if (Disposed)
            {
                throw new ObjectDisposedException("Session has been disposed.");
            }

            // Allow client to close immediately
            client.LingerState = new LingerOption(true, 0);
            Name = client.Client.RemoteEndPoint?.ToString();

            byte[] sivBytes = new byte[4];
            byte[] rivBytes = new byte[4];
            rng.GetBytes(sivBytes);
            rng.GetBytes(rivBytes);
            Siv = BitConverter.ToUInt32(sivBytes);
            Riv = BitConverter.ToUInt32(rivBytes);

            Client = client;
            NetworkStream = client.GetStream();
            SendCipher = new MapleCipher.Encryptor(VERSION, Siv, BLOCK_IV);
            RecvCipher = new MapleCipher.Decryptor(VERSION, Riv, BLOCK_IV);
        }

        public void Dispose()
        {
            if (Disposed)
            {
                return;
            }

            Disposed = true;
            Complete();
            Thread.Join(STOP_TIMEOUT);

            CloseClient();

#if DEBUG
            GC.SuppressFinalize(this);
#endif
        }

        protected void Complete()
        {
            RecvPipe.Writer.Complete();
            RecvPipe.Reader.Complete();
            PipeScheduler.Complete();
        }

        public void Disconnect()
        {
            if (Disposed)
            {
                return;
            }

            Logger.Info($"Disconnected {this}");
            EndSession();
            ((IDisposable) this).Dispose();
        }

        public bool Connected()
        {
            if (Disposed || Client?.Client == null)
            {
                return false;
            }

            Socket socket = Client.Client;
            return !((socket.Poll(1000, SelectMode.SelectRead) && (socket.Available == 0)) || !socket.Connected);
        }

        public void Start()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException("Session has been disposed.");
            }
            if (Client == null)
            {
                throw new InvalidOperationException("Cannot start a session without a client.");
            }

            Thread.Start();
        }

        public void Send(params byte[] packet) => SendInternal(new PacketWriter(packet), packet.Length);

        public void Send(PacketWriter packet) => SendInternal(packet, packet.Length);

        // Ensures no more communication before sending a final packet.
        public void SendFinal(PacketWriter packet)
        {
            SendInternal(packet, packet.Length);
            EndSession();
        }

        public override string ToString() => $"{GetType().Name} from {Name}";

        private void StartInternal()
        {
            try
            {
                PerformHandshake(); // Perform handshake to initialize connection

                // Pipeline tasks can be run asynchronously
                Task writeTask = WriteRecvPipe(Client.Client, RecvPipe.Writer);
                Task readTask = ReadRecvPipe(RecvPipe.Reader);
                Task.WhenAll(writeTask, readTask).ContinueWith(_ => CloseClient());

                while (!Disposed && PipeScheduler.OutputAvailableAsync().Result)
                {
                    PipeScheduler.ProcessQueue();
                }
            }
            catch (Exception ex)
            {
                if (!Disposed)
                {
                    Logger.Error($"Exception on session thread: {ex}");
                }
            }
            finally
            {
                Disconnect();
            }
        }

        private void PerformHandshake()
        {
            PacketWriter handshake = HandshakePacket.Handshake(VERSION, Riv, Siv, BLOCK_IV, Type, HANDSHAKE_SIZE);

            // No encryption for handshake
            using PoolPacketWriter packet = SendCipher.WriteHeader(handshake.Buffer, 0, handshake.Length);
            Logger.Debug($"Handshake: {packet}");
            SendRaw(packet);
        }

        private async Task WriteRecvPipe(Socket socket, PipeWriter writer)
        {
            try
            {
                FlushResult result;
                do
                {
                    Memory<byte> memory = writer.GetMemory();
                    int bytesRead = await socket.ReceiveAsync(memory, SocketFlags.None);
                    if (bytesRead <= 0)
                    {
                        break;
                    }

                    writer.Advance(bytesRead);

                    result = await writer.FlushAsync();
                } while (!Disposed && !result.IsCompleted);
            }
            catch (Exception)
            {
                Disconnect();
            }
        }

        private async Task ReadRecvPipe(PipeReader reader)
        {
            try
            {
                ReadResult result;
                do
                {
                    result = await reader.ReadAsync();

                    int bytesRead;
                    ReadOnlySequence<byte> buffer = result.Buffer;
                    while ((bytesRead = RecvCipher.TryDecrypt(buffer, out PoolPacketReader packet)) > 0)
                    {
                        try
                        {
                            LogRecv(packet);
                            OnPacket?.Invoke(this, packet); // handle packet
                        }
                        finally
                        {
                            packet.Dispose();
                        }
                        buffer = buffer.Slice(bytesRead);
                    }

                    reader.AdvanceTo(buffer.Start, buffer.End);
                } while (!Disposed && !result.IsCompleted);
            }
            catch (Exception ex)
            {
                if (!Disposed)
                {
                    Logger.Error($"Exception reading recv packet: {ex}");
                }
            }
            finally
            {
                Disconnect();
            }
        }

        private void SendInternal(PacketWriter packet, int length)
        {
            if (Disposed)
            {
                return;
            }

            LogSend(packet);
            lock (SendCipher)
            {
                using PoolPacketWriter encryptedPacket = SendCipher.Encrypt(packet.Buffer, 0, length);
                SendRaw(encryptedPacket);
            }
        }

        private void SendRaw(PacketWriter packet)
        {
            if (Disposed)
            {
                return;
            }

            try
            {
                NetworkStream.Write(packet.Buffer, 0, packet.Length);
            }
            catch (Exception)
            {
                Disconnect();
            }
        }

        private void CloseClient()
        {
            // Must close socket before network stream to prevent lingering
            Client?.Client?.Close();
            Client?.Close();
        }

        private static void LogSend(PacketWriter packet)
        {
            SendOp sendOp = (SendOp) (packet.Buffer[1] << 8 | packet.Buffer[0]);
            switch (sendOp)
            {
                case SendOp.USER_SYNC:
                case SendOp.KEY_TABLE:
                case SendOp.STAT:
                case SendOp.EMOTION:
                case SendOp.CHARACTER_LIST:
                case SendOp.ITEM_INVENTORY:
                case SendOp.FIELD_ADD_NPC:
                case SendOp.FIELD_PORTAL:
                case SendOp.NPC_CONTROL:
                case SendOp.RIDE_SYNC:
                case SendOp.PROXY_GAME_OBJ:
                case SendOp.FIELD_ADD_USER:
                case SendOp.FIELD_ENTRANCE:
                case SendOp.SERVER_ENTER:
                case SendOp.QUEST:
                case SendOp.STORAGE_INVENTORY:
                case SendOp.TROPHY:
                case SendOp.RESPONSE_TIME_SYNC:
                case SendOp.VIBRATE:
                    break;
                default:
                    string packetString = packet.ToString();
                    Logger.Debug($"SEND ({sendOp}): {packetString[Math.Min(packetString.Length, 6)..]}".ColorRed());
                    break;
            }
        }

        private static void LogRecv(PacketReader packet)
        {
            RecvOp recvOp = (RecvOp) (packet.Buffer[1] << 8 | packet.Buffer[0]);
            switch (recvOp)
            {
                case RecvOp.USER_SYNC:
                case RecvOp.KEY_TABLE:
                case RecvOp.RIDE_SYNC:
                case RecvOp.GUIDE_OBJECT_SYNC:
                case RecvOp.REQUEST_TIME_SYNC:
                case RecvOp.STATE:
                case RecvOp.STATE_FALL_DAMAGE:
                case RecvOp.VIBRATE:
                    break;
                default:
                    string packetString = packet.ToString();
                    Logger.Debug($"RECV ({recvOp}): {packetString[Math.Min(packetString.Length, 6)..]}".ColorGreen());
                    break;
            }
        }

        public abstract void EndSession();
    }
}
