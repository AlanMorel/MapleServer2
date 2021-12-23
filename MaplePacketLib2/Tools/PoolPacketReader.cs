using System.Buffers;
using System.Diagnostics;

namespace MaplePacketLib2.Tools;

public class PoolPacketReader : PacketReader, IDisposable
{
    private readonly ArrayPool<byte> Pool;

    public PoolPacketReader(ArrayPool<byte> pool, byte[] packet, int packetSize, int offset = 0) : base(packet, offset)
    {
        Length = packetSize;
        Pool = pool;
    }

    public void Dispose()
    {
        Pool.Return(Buffer);
#if DEBUG
        // In DEBUG, SuppressFinalize to mark object as disposed.
        GC.SuppressFinalize(this);
#endif
    }

#if DEBUG
    // Provides warning if Disposed in not called.
    ~PoolPacketReader()
    {
        Debug.Fail($"PacketReader not disposed: {this}");
    }
#endif
}
