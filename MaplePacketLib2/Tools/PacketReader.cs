using System.Runtime.CompilerServices;
using System.Text;

namespace MaplePacketLib2.Tools
{
    public unsafe class PacketReader : IPacketReader
    {
        public byte[] Buffer { get; }
        public int Length { get; protected set; }
        public int Position { get; protected set; }

        public int Available => Length - Position;

        public PacketReader(byte[] packet, int offset = 0)
        {
            Buffer = packet;
            Length = packet.Length;
            Position = offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void CheckLength(int length)
        {
            int index = Position + length;
            if (index > Length || index < Position)
            {
                throw new IndexOutOfRangeException($"Not enough space in packet: {this}\n");
            }
        }

        public T Read<T>() where T : struct
        {
            int size = Unsafe.SizeOf<T>();
            CheckLength(size);
            fixed (byte* ptr = &Buffer[Position])
            {
                T value = Unsafe.Read<T>(ptr);
                Position += size;
                return value;
            }
        }

        public T Peek<T>() where T : struct
        {
            int size = Unsafe.SizeOf<T>();
            CheckLength(size);
            fixed (byte* ptr = &Buffer[Position])
            {
                return Unsafe.Read<T>(ptr);
            }
        }

        public byte[] ReadBytes(int count)
        {
            if (count == 0)
            {
                return Array.Empty<byte>();
            }

            CheckLength(count);
            byte[] bytes = new byte[count];
            fixed (byte* ptr = &Buffer[Position])
            fixed (byte* bytesPtr = bytes)
            {
                Unsafe.CopyBlock(bytesPtr, ptr, (uint) count);
            }

            Position += count;
            return bytes;
        }

        public bool ReadBool()
        {
            return ReadByte() != 0;
        }

        public byte ReadByte()
        {
            CheckLength(1);
            return Buffer[Position++];
        }

        public short ReadShort()
        {
            CheckLength(2);
            fixed (byte* ptr = &Buffer[Position])
            {
                short value = *(short*) ptr;
                Position += 2;
                return value;
            }
        }

        public int ReadInt()
        {
            CheckLength(4);
            fixed (byte* ptr = &Buffer[Position])
            {
                int value = *(int*) ptr;
                Position += 4;
                return value;
            }
        }

        public float ReadFloat()
        {
            CheckLength(4);
            fixed (byte* ptr = &Buffer[Position])
            {
                float value = *(float*) ptr;
                Position += 4;
                return value;
            }
        }

        public long ReadLong()
        {
            CheckLength(8);
            fixed (byte* ptr = &Buffer[Position])
            {
                long value = *(long*) ptr;
                Position += 8;
                return value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ReadString()
        {
            ushort length = Read<ushort>();
            return ReadRawString(length);
        }

        public string ReadRawString(int length)
        {
            if (length == 0)
            {
                return string.Empty;
            }

            CheckLength(length);
            fixed (byte* ptr = &Buffer[Position])
            {
                string value = new string((sbyte*) ptr, 0, length, Encoding.UTF8);
                Position += length;
                return value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ReadUnicodeString()
        {
            ushort length = Read<ushort>();
            return ReadRawUnicodeString(length);
        }

        public string ReadRawUnicodeString(int length)
        {
            if (length == 0)
            {
                return string.Empty;
            }

            CheckLength(length * 2);
            fixed (byte* ptr = &Buffer[Position])
            {
                string value = new string((sbyte*) ptr, 0, length * 2, Encoding.Unicode);
                Position += length * 2;
                return value;
            }
        }

        public void Skip(int count)
        {
            int index = Position + count;
            if (index > Length || index < 0)
            { // Allow backwards seeking
                throw new IndexOutOfRangeException($"Not enough space in packet: {this}\n");
            }
            Position += count;
        }

        public override string ToString()
        {
            return Buffer.ToHexString(Length, ' ');
        }

        void IDisposable.Dispose() => GC.SuppressFinalize(this);
    }
}
