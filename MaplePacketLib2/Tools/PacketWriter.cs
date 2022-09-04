using System.Runtime.CompilerServices;

namespace MaplePacketLib2.Tools;

public unsafe class PacketWriter : IPacketWriter
{
    protected const int DEFAULT_SIZE = 512;

    public byte[] Buffer { get; protected set; }
    public int Length { get; protected set; }

    public int Remaining => Buffer.Length - Length;

    public PacketWriter(int size = DEFAULT_SIZE) : this(new byte[size]) { }

    public PacketWriter(byte[] buffer, int offset = 0)
    {
        Buffer = buffer;
        Length = offset;
    }

    public static PacketWriter Of(object opcode, int size = DEFAULT_SIZE)
    {
        PacketWriter packet = new(size);
        packet.Write(Convert.ToUInt16(opcode));
        return packet;
    }

    private void EnsureCapacity(int length)
    {
        int required = Length + length;
        if (Buffer.Length >= required)
        {
            return;
        }

        int newSize = Buffer.Length * 2;
        while (newSize < required)
        {
            newSize *= 2;
        }

        ResizeBuffer(newSize);
    }

    public virtual void ResizeBuffer(int newSize)
    {
        byte[] copy = new byte[newSize];
        fixed (byte* ptr = Buffer)
        fixed (byte* copyPtr = copy)
        {
            Unsafe.CopyBlock(copyPtr, ptr, (uint) Length);
        }

        Buffer = copy;
    }

    public void Seek(int position)
    {
        if (position < 0 || position > Buffer.Length)
        {
            return;
        }

        Length = position;
    }

    public void Write<T>(in T value) where T : struct
    {
        int size = Unsafe.SizeOf<T>();
        EnsureCapacity(size);
        fixed (byte* ptr = &Buffer[Length])
        {
            Unsafe.Write(ptr, value);
            Length += size;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteBytes(byte[] value)
    {
        WriteBytes(value, 0, value.Length);
    }

    public void WriteBytes(byte[] value, int offset, int length)
    {
        EnsureCapacity(length);
        fixed (byte* ptr = &Buffer[Length])
        fixed (byte* valuePtr = value)
        {
            Unsafe.CopyBlock(ptr, valuePtr + offset, (uint) length);
            Length += length;
        }
    }

    public void WriteZero(int count)
    {
        WriteBytes(new byte[count]);
    }

    public void WriteBool(bool value)
    {
        EnsureCapacity(1);
        Buffer[Length++] = value ? (byte) 1 : (byte) 0;
    }

    public void WriteByte(byte value = 0)
    {
        EnsureCapacity(1);
        Buffer[Length++] = value;
    }

    public void WriteShort(short value = 0)
    {
        EnsureCapacity(2);
        fixed (byte* ptr = &Buffer[Length])
        {
            *(short*) ptr = value;
            Length += 2;
        }
    }

    public void WriteInt(int value = 0)
    {
        EnsureCapacity(4);
        fixed (byte* ptr = &Buffer[Length])
        {
            *(int*) ptr = value;
            Length += 4;
        }
    }

    public void WriteFloat(float value = 0f)
    {
        EnsureCapacity(4);
        fixed (byte* ptr = &Buffer[Length])
        {
            *(float*) ptr = value;
            Length += 4;
        }
    }

    public void WriteLong(long value = 0)
    {
        EnsureCapacity(8);
        fixed (byte* ptr = &Buffer[Length])
        {
            *(long*) ptr = value;
            Length += 8;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteString(string value = "")
    {
        Write((ushort) value.Length);
        WriteRawString(value);
    }

    // Note: char and string are UTF-16 in C#
    public void WriteRawString(string value)
    {
        int length = value.Length;
        EnsureCapacity(length);
        fixed (char* valuePtr = value)
        {
            for (int i = 0; i < length; i++)
            {
                Buffer[Length++] = (byte) *(valuePtr + i);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUnicodeString(string value = "")
    {
        Write((ushort) value.Length);
        WriteRawUnicodeString(value);
    }

    public void WriteRawUnicodeString(string value)
    {
        if (value.Length == 0)
        {
            return;
        }

        int length = value.Length * 2;

        EnsureCapacity(length);
        fixed (byte* ptr = &Buffer[Length])
        fixed (char* valuePtr = value)
        {
            Unsafe.CopyBlock(ptr, valuePtr, (uint) length);
            Length += length;
        }
    }

    public byte[] ToArray()
    {
        byte[] copy = new byte[Length];
        fixed (byte* ptr = Buffer)
        fixed (byte* copyPtr = copy)
        {
            Unsafe.CopyBlock(copyPtr, ptr, (uint) Length);
        }

        return copy;
    }

    public override string ToString()
    {
        return Buffer.ToHexString(Length, ' ');
    }

    void IDisposable.Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
