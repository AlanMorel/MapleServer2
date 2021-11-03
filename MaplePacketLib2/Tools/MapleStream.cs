namespace MaplePacketLib2.Tools
{
    public class MapleStream
    {
        private const int DEFAULT_SIZE = 4096;
        private const int HEADER_SIZE = 6;

        private byte[] Buffer = new byte[DEFAULT_SIZE];
        private int Cursor;

        public void Write(byte[] packet)
        {
            Write(packet, 0, packet.Length);
        }

        public void Write(byte[] packet, int offset, int length)
        {
            if (Buffer.Length - Cursor < length)
            {
                int newSize = Buffer.Length * 2;
                while (newSize < Cursor + length)
                {
                    newSize *= 2;
                }
                byte[] newBuffer = new byte[newSize];
                System.Buffer.BlockCopy(Buffer, 0, newBuffer, 0, Cursor);
                Buffer = newBuffer;
            }
            System.Buffer.BlockCopy(packet, offset, Buffer, Cursor, length);
            Cursor += length;
        }

        public bool TryRead(out byte[] packet)
        {
            if (Cursor < HEADER_SIZE)
            {
                packet = null;
                return false;
            }

            int packetSize = BitConverter.ToInt32(Buffer, 2);
            int bufferSize = HEADER_SIZE + packetSize;
            if (Cursor < bufferSize)
            {
                packet = null;
                return false;
            }

            packet = new byte[bufferSize];
            System.Buffer.BlockCopy(Buffer, 0, packet, 0, bufferSize);

            Cursor -= bufferSize;
            System.Buffer.BlockCopy(Buffer, bufferSize, Buffer, 0, Cursor);

            return true;
        }
    }
}
