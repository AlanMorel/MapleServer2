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
using MapleServer2.Servers.Game;
using MapleServer2.Servers.Login;
using Serilog;

namespace MapleServer2.Network;

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
    private static readonly ILogger Logger = Log.Logger.ForContext<Session>();

    private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

    protected Session()
    {
        Thread = new(StartInternal);
        PipeScheduler = new();
        PipeOptions options = new(
            readerScheduler: PipeScheduler,
            writerScheduler: PipeScheduler,
            useSynchronizationContext: false
        );
        RecvPipe = new(options);
    }

    public void Init([NotNull] TcpClient client)
    {
        if (Disposed)
        {
            throw new ObjectDisposedException("Session has been disposed.");
        }

        // Allow client to close immediately
        client.LingerState = new(true, 0);
        Name = client.Client.RemoteEndPoint?.ToString();

        byte[] sivBytes = new byte[4];
        byte[] rivBytes = new byte[4];
        Rng.GetBytes(sivBytes);
        Rng.GetBytes(rivBytes);
        Siv = BitConverter.ToUInt32(sivBytes);
        Riv = BitConverter.ToUInt32(rivBytes);

        Client = client;
        NetworkStream = client.GetStream();
        SendCipher = new(VERSION, Siv, BLOCK_IV);
        RecvCipher = new(VERSION, Riv, BLOCK_IV);
    }

    public void Dispose()
    {
        if (Disposed)
        {
            return;
        }

        if (this is LoginSession loginSession)
        {
            MapleServer.GetLoginServer().RemoveSession(loginSession);
        }

        if (this is GameSession gameSession)
        {
            MapleServer.GetGameServer().RemoveSession(gameSession);
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

    public void Disconnect(bool logoutNotice)
    {
        if (Disposed)
        {
            return;
        }

        EndSession(logoutNotice);
        ((IDisposable) this).Dispose();
    }

    public bool Connected()
    {
        if (Disposed || Client?.Client == null)
        {
            return false;
        }

        Socket socket = Client.Client;
        return !(socket.Poll(1000, SelectMode.SelectRead) && socket.Available == 0 || !socket.Connected);
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

    public void Send(params byte[] packet)
    {
        SendInternal(new(packet), packet.Length);
    }

    public void Send(PacketWriter packet)
    {
        SendInternal(packet, packet.Length);
    }

    public void SendFinal(PacketWriter packet, bool logoutNotice)
    {
        SendInternal(packet, packet.Length);
        Disconnect(logoutNotice);
    }

    public override string ToString()
    {
        return $"{GetType().Name} from {Name}";
    }

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
                Logger.Fatal("Exception on session thread: {ex}", ex);
            }
        }
        finally
        {
            Disconnect(logoutNotice: true);
        }
    }

    private void PerformHandshake()
    {
        PacketWriter handshake = HandshakePacket.Handshake(VERSION, Riv, Siv, BLOCK_IV, Type, HANDSHAKE_SIZE);

        // No encryption for handshake
        using PoolPacketWriter packet = SendCipher.WriteHeader(handshake.Buffer, 0, handshake.Length);
        Logger.Debug("Handshake: {packet}", packet);
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
            Disconnect(logoutNotice: true);
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
                Logger.Fatal("Exception in recv PipeScheduler: {ex}", ex);
            }
        }
        finally
        {
            Disconnect(logoutNotice: true);
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
            Disconnect(logoutNotice: true);
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
            case SendOp.UserSync:
            case SendOp.KeyTable:
            case SendOp.Stat:
            case SendOp.Emotion:
            case SendOp.CharList:
            case SendOp.ItemInventory:
            case SendOp.FieldAddNPC:
            case SendOp.FieldPortal:
            case SendOp.NpcControl:
            case SendOp.RideSync:
            case SendOp.FieldObject:
            case SendOp.FieldAddPlayer:
            case SendOp.DungeonList:
            case SendOp.ServerEnter:
            case SendOp.Quest:
            case SendOp.StorageInventory:
            case SendOp.Trophy:
            case SendOp.ResponseTimeSync:
            case SendOp.Vibrate:
                break;
            default:
                string packetString = packet.ToString();
                Logger.Debug("{mode} ({sendOp} - {hexa}): {packetString}",
                    "SEND".ColorRed(), sendOp, $"0x{sendOp:X}", packetString[Math.Min(packetString.Length, 6)..]);
                break;
        }
    }

    private static void LogRecv(PacketReader packet)
    {
        RecvOp recvOp = (RecvOp) (packet.Buffer[1] << 8 | packet.Buffer[0]);
        switch (recvOp)
        {
            case RecvOp.UserSync:
            case RecvOp.KeyTable:
            case RecvOp.RideSync:
            case RecvOp.GuideObjectSync:
            case RecvOp.RequestTimeSync:
            case RecvOp.State:
            case RecvOp.StateFallDamage:
            case RecvOp.Vibrate:
                break;
            default:
                string packetString = packet.ToString();
                Logger.Debug("{mode} ({recvOp} - {hexa}): {packetString}",
                    "RECV".ColorGreen(), recvOp, $"0x{recvOp:X}", packetString[Math.Min(packetString.Length, 6)..]);
                break;
        }
    }

    protected abstract void EndSession(bool logoutNotice);
}
