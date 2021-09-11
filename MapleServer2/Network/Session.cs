using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using System.Security.Cryptography;
using MaplePacketLib2.Crypto;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Extensions;
using NLog;

namespace MapleServer2.Network
{
    public abstract class Session : IDisposable
    {
        public const uint VERSION = 12;

        private const int BUFFER_SIZE = 1024;
        private const uint BLOCK_IV = 12; // TODO: should this be variable

        private const int STOP_TIMEOUT = 2000;

        protected abstract SessionType Type { get; }

        public EventHandler<string> OnError;
        public EventHandler<Packet> OnPacket;

        private uint Siv;
        private uint Riv;

        private Task SendThread;
        private Task RecvThread;
        private TcpClient Client;
        private NetworkStream NetworkStream;
        private MapleStream MapleStream;
        private MapleCipher SendCipher;
        private MapleCipher RecvCipher;

        // These are unencrypted packets, scheduled to be sent on a single thread.
        private readonly Queue<byte[]> SendQueue;
        private readonly byte[] RecvBuffer;
        private readonly CancellationTokenSource Source;

        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

        protected Session()
        {
            SendQueue = new Queue<byte[]>();
            RecvBuffer = new byte[BUFFER_SIZE];
            Source = new CancellationTokenSource();
        }

        public void Init([NotNull] TcpClient client)
        {
            // Allow Client to close immediately
            client.LingerState = new LingerOption(true, 0);

            byte[] sivBytes = new byte[4];
            byte[] rivBytes = new byte[4];
            Rng.GetBytes(sivBytes);
            Rng.GetBytes(rivBytes);
            Siv = BitConverter.ToUInt32(sivBytes);
            Riv = BitConverter.ToUInt32(rivBytes);

            Client = client;
            NetworkStream = client.GetStream();
            MapleStream = new MapleStream();
            SendCipher = MapleCipher.Encryptor(VERSION, Siv, BLOCK_IV);
            RecvCipher = MapleCipher.Decryptor(VERSION, Riv, BLOCK_IV);
        }

        public void Dispose()
        {
            Disconnect();
            Client?.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Disconnect()
        {
            StopThreads();
            if (Connected())
            {
                // Must close socket before network stream to prevent lingering
                Client.Client.Close();
                Client.Close();
                Logger.Debug("Disconnected Client.");
            }
        }

        private void StopThreads()
        {
            Source.Cancel();
            SendThread.Wait(STOP_TIMEOUT);
            RecvThread.Wait(STOP_TIMEOUT);
            EndSession();
        }

        public bool Connected()
        {
            if (Client?.Client == null)
            {
                return false;
            }

            Socket socket = Client.Client;
            return !((socket.Poll(1000, SelectMode.SelectRead) && (socket.Available == 0)) || !socket.Connected);
        }

        public void Start()
        {
            if (SendThread != null || RecvThread != null)
            {
                throw new ArgumentException("Session has already been started.");
            }

            RecvThread = new Task(() =>
            {
                try
                {
                    PerformHandshake();
                    StartRead();
                }
                catch (SystemException ex)
                {
                    Logger.Trace("Fatal error for session:{ex}", ex);
                    Disconnect();
                }
            });
            SendThread = new Task(() =>
            {
                try
                {
                    StartWrite();
                }
                catch (SystemException ex)
                {
                    Logger.Trace("Fatal error for session:{ex}", ex);
                    Disconnect();
                }
            });
            SendThread.Start();
            RecvThread.Start();
        }

        private void PerformHandshake()
        {
            if (Client == null)
            {
                throw new InvalidOperationException("Cannot start a session without a Client.");
            }

            PacketWriter pWriter = PacketWriter.Of(SendOp.REQUEST_VERSION);
            pWriter.WriteUInt(VERSION);
            pWriter.WriteUInt(Riv);
            pWriter.WriteUInt(Siv);
            pWriter.WriteUInt(BLOCK_IV);
            pWriter.WriteByte((byte) Type);

            // No encryption for handshake
            Packet packet = SendCipher.WriteHeader(pWriter.ToArray());
            Logger.Debug("Handshake: {packet}", packet);
            SendRaw(packet);
        }

        public void Send(params byte[] packet)
        {
            lock (SendQueue)
            {
                SendQueue.Enqueue(packet);
            }
        }

        public void Send(Packet packet)
        {
            lock (SendQueue)
            {
                SendQueue.Enqueue(packet.ToArray());
            }
        }

        // Ensures no more communication before sending a final packet.
        public void SendFinal(Packet packet)
        {
            SendInternal(packet.ToArray());
            StopThreads();
        }

        private async void StartRead()
        {
            CancellationToken readToken = Source.Token;
            while (!readToken.IsCancellationRequested)
            {
                try
                {
                    int length = await NetworkStream.ReadAsync(RecvBuffer.AsMemory(0, RecvBuffer.Length), readToken);
                    if (length <= 0)
                    {
                        if (!Connected())
                        {
                            return;
                        }

                        continue;
                    }

                    MapleStream.Write(RecvBuffer, 0, length);
                }
                catch (IOException)
                {
                    Disconnect();
                    return;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Exception reading from socket:");
                    return;
                }

                while (MapleStream.TryRead(out byte[] packetBuffer))
                {
                    Packet packet = RecvCipher.Transform(packetBuffer);
                    short opcode = packet.Reader().ReadShort();

                    RecvOp recvOp = (RecvOp) opcode;

                    switch (recvOp)
                    {
                        case RecvOp.USER_SYNC:
                        case RecvOp.KEY_TABLE:
                        case RecvOp.RIDE_SYNC:
                        case RecvOp.GUIDE_OBJECT_SYNC:
                        case RecvOp.REQUEST_TIME_SYNC:
                        case RecvOp.STATE:
                        case RecvOp.STATE_FALL_DAMAGE:
                            break;
                        default:
                            string packetString = packet.ToString();
                            Logger.Debug($"RECV ({recvOp}): {packetString[Math.Min(packetString.Length, 6)..]}".ColorGreen());
                            break;
                    }
                    OnPacket?.Invoke(this, packet); // handle packet
                }
            }
        }

        private void StartWrite()
        {
            CancellationToken writeToken = Source.Token;
            while (!writeToken.IsCancellationRequested)
            {
                lock (SendQueue)
                {
                    while (SendQueue.TryDequeue(out byte[] packet))
                    {
                        SendInternal(packet);
                    }
                }
                Thread.Sleep(1);
            }
        }

        private void SendInternal(byte[] packet)
        {
            short opcode = BitConverter.ToInt16(packet, 0);
            SendOp sendOp = (SendOp) opcode;

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
                case SendOp.INTERACT_OBJECT:
                case SendOp.RESPONSE_TIME_SYNC:
                    break;
                default:
                    string packetString = packet.ToHexString(' ');
                    Logger.Debug($"SEND ({sendOp}): {packetString[Math.Min(packetString.Length, 6)..]}".ColorRed());
                    break;
            }
            Packet encryptedPacket = SendCipher.Transform(packet);
            SendRaw(encryptedPacket);
        }

        private void SendRaw(Packet packet)
        {
            try
            {
                NetworkStream.Write(packet.Buffer, 0, packet.Length);
            }
            catch (IOException ex)
            {
                Logger.Error("Exception writing to socket: {ex}", ex);
                Disconnect();
            }
        }

        public override string ToString()
        {
            return $"{GetType().Name} from {Client?.Client.RemoteEndPoint}";
        }

        public abstract void EndSession();
    }
}
