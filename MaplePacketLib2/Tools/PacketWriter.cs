using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MaplePacketLib2.Tools
{
    public class PacketWriter : Packet
    {
        private const int DEFAULT_SIZE = 64;

        public int Remaining => Buffer.Length - Length;

        public PacketWriter(int size = DEFAULT_SIZE) : base(new byte[size])
        {
            this.Length = 0;
        }

        public static PacketWriter Of(Object opcode, int size = DEFAULT_SIZE)
        {
            PacketWriter packet = new PacketWriter(size);
            packet.WriteUShort(Convert.ToUInt16(opcode));
            return packet;
        }

        private void EnsureCapacity(int length)
        {
            if (length <= Remaining)
            {
                return;
            }
            int newSize = Buffer.Length * 2;
            while (newSize < Length + length)
            {
                newSize *= 2;
            }
            ResizeBuffer(newSize);
        }

        public void ResizeBuffer(int newSize)
        {
            byte[] newBuffer = new byte[newSize];
            System.Buffer.BlockCopy(Buffer, 0, newBuffer, 0, Length);
            Buffer = newBuffer;
        }

        public void Seek(int position)
        {
            if (position < 0 || position > Buffer.Length)
            {
                return;
            }

            Length = position;
        }

        public unsafe PacketWriter Write<T>(T value) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            EnsureCapacity(size);
            fixed (byte* ptr = &Buffer[Length])
            {
                Marshal.StructureToPtr(value, (IntPtr) ptr, false);
                Length += size;
            }

            return this;
        }

        public PacketWriter Write(params byte[] value)
        {
            EnsureCapacity(value.Length);
            System.Buffer.BlockCopy(value, 0, Buffer, Length, value.Length);
            Length += value.Length;

            return this;
        }

        public PacketWriter WriteEnum(Enum value)
        {
            Type type = Enum.GetUnderlyingType(value.GetType());
            dynamic converted = Convert.ChangeType(value, type);

            return Write(converted);
        }

        public PacketWriter WriteBool(bool value)
        {
            return WriteByte(value ? (byte) 1 : (byte) 0);
        }

        public unsafe PacketWriter WriteByte(byte value = 0)
        {
            EnsureCapacity(1);
            fixed (byte* ptr = Buffer)
            {
                *(ptr + Length) = value;
                ++Length;
            }

            return this;
        }

        public unsafe PacketWriter WriteShort(short value = 0)
        {
            EnsureCapacity(2);
            fixed (byte* ptr = Buffer)
            {
                *(short*) (ptr + Length) = value;
                Length += 2;
            }

            return this;
        }

        public unsafe PacketWriter WriteUShort(ushort value = 0)
        {
            EnsureCapacity(2);
            fixed (byte* ptr = Buffer)
            {
                *(ushort*) (ptr + Length) = value;
                Length += 2;
            }

            return this;
        }

        public unsafe PacketWriter WriteInt(int value = 0)
        {
            EnsureCapacity(4);
            fixed (byte* ptr = Buffer)
            {
                *(int*) (ptr + Length) = value;
                Length += 4;
            }

            return this;
        }

        public unsafe PacketWriter WriteUInt(uint value = 0)
        {
            EnsureCapacity(4);
            fixed (byte* ptr = Buffer)
            {
                *(uint*) (ptr + Length) = value;
                Length += 4;
            }

            return this;
        }

        public unsafe PacketWriter WriteFloat(float value = 0.0f)
        {
            EnsureCapacity(4);
            fixed (byte* ptr = Buffer)
            {
                *(float*) (ptr + Length) = value;
                Length += 4;
            }

            return this;
        }


        public unsafe PacketWriter WriteLong(long value = 0)
        {
            EnsureCapacity(8);
            fixed (byte* ptr = Buffer)
            {
                *(long*) (ptr + Length) = value;
                Length += 8;
            }

            return this;
        }

        public unsafe PacketWriter WriteULong(ulong value = 0)
        {
            EnsureCapacity(8);
            fixed (byte* ptr = Buffer)
            {
                *(ulong*) (ptr + Length) = value;
                Length += 8;
            }

            return this;
        }

        public PacketWriter WriteString(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            return Write(bytes);
        }

        public PacketWriter WritePaddedString(string value, int length, char pad = '\0')
        {
            WriteString(value);
            for (int i = value.Length; i < length; i++)
            {
                WriteByte((byte) pad);
            }

            return this;
        }

        public PacketWriter WriteUnicodeString(string value)
        {
            Write((ushort) value.Length);
            byte[] bytes = Encoding.Unicode.GetBytes(value);
            return Write(bytes);
        }

        public PacketWriter WriteMapleString(string value)
        {
            Write((ushort) value.Length);
            return WriteString(value);
        }

        public PacketWriter WriteHexString(string value)
        {
            byte[] bytes = value.ToByteArray();
            return Write(bytes);
        }

        public PacketWriter WriteZero(int count)
        {
            return Write(new byte[count]);
        }
    }
}
