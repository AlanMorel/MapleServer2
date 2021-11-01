using NLog;

namespace MaplePacketLib2.Tools
{
    // Converts a stream of bytes into individual packets
    public class MapleStream
    {
        private const int DEFAULT_SIZE = 4096;
        private const int HEADER_SIZE = 6;
        private const int PACKET_MAX_SIZE = 10000;

        private byte[] Buffer = new byte[DEFAULT_SIZE];
        private int Cursor;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
            packet = null;

            if (Cursor < HEADER_SIZE)
            {
                return false;
            }

            int packetSize = BitConverter.ToInt32(Buffer, 2);
            int bufferSize = HEADER_SIZE + packetSize;

            if (bufferSize < 0 || bufferSize > PACKET_MAX_SIZE)
            {
                Logger.Debug($"Buffer: {string.Join(" ", Buffer)}");
                Logger.Fatal($"Packet size was too big or negative: {packetSize}");
                return false;
            }

            if (Cursor < bufferSize)
            {
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
