using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MaplePacketLib2.Tools;

public unsafe class PoolPacketWriter : PacketWriter, IDisposable
{
    private readonly ArrayPool<byte> Pool;
    private bool Disposed;

    public PoolPacketWriter(int size = DEFAULT_SIZE, ArrayPool<byte> pool = null) : base((pool ?? ArrayPool<byte>.Shared).Rent(size))
    {
        Pool = pool ?? ArrayPool<byte>.Shared;
        Length = 0;
    }

    public override void ResizeBuffer(int newSize)
    {
        if (newSize < Buffer.Length)
        {
            throw new ArgumentException("Cannot decrease buffer size.");
        }

        byte[] copy = Pool.Rent(newSize);
        fixed (byte* ptr = Buffer)
        fixed (byte* copyPtr = copy)
        {
            Unsafe.CopyBlock(copyPtr, ptr, (uint) Length);
        }
        Pool.Return(Buffer);

        Buffer = copy;
    }

    // Returns a managed array ByteWriter and disposes this instance.
    public PacketWriter Managed()
    {
        PacketWriter writer = new(ToArray(), Length);
        Dispose();
        return writer;
    }

    public void Dispose()
    {
        if (!Disposed)
        {
            Disposed = true;
            Pool.Return(Buffer);
        }
#if DEBUG
        // In DEBUG, SuppressFinalize to mark object as disposed.
        GC.SuppressFinalize(this);
#endif
    }

#if DEBUG
    // Provides warning if Disposed in not called.
    ~PoolPacketWriter()
    {
        Debug.Fail($"PacketWriter not disposed: {this}");
    }
#endif
}
