using System;

namespace MaplePacketLib2.Tools {
    // Converts a stream of bytes into individual packets
    public class MapleStream {
        private const int DEFAULT_SIZE = 4096;
        private const int HEADER_SIZE = 6;

        private byte[] buffer = new byte[DEFAULT_SIZE];
        private int cursor;

        public void Write(byte[] packet) {
            Write(packet, 0, packet.Length);
        }

        public void Write(byte[] packet, int offset, int length) {
            if (buffer.Length - cursor < length) {
                int newSize = buffer.Length * 2;
                while (newSize < cursor + length) {
                    newSize *= 2;
                }
                byte[] newBuffer = new byte[newSize];
                Buffer.BlockCopy(buffer, 0, newBuffer, 0, cursor);
                buffer = newBuffer;
            }
            Buffer.BlockCopy(packet, offset, buffer, cursor, length);
            cursor += length;
        }

        public bool TryRead(out byte[] packet) {
            if (cursor < HEADER_SIZE) {
                packet = null;
                return false;
            }

            int packetSize = BitConverter.ToInt32(buffer, 2);
            int bufferSize = HEADER_SIZE + packetSize;
            if (cursor < bufferSize) {
                packet = null;
                return false;
            }

            packet = new byte[bufferSize];
            Buffer.BlockCopy(buffer, 0, packet, 0, bufferSize);

            cursor -= bufferSize;
            Buffer.BlockCopy(buffer, bufferSize, buffer, 0, cursor);

            return true;
        }
    }
}
