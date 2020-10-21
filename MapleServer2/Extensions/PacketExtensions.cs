using ICSharpCode.SharpZipLib.Zip.Compression;
using MaplePacketLib2.Tools;

namespace MapleServer2.Extensions {
    public static class PacketExtensions {
        private const int INT_SIZE = 4;

        // Write data deflated using Zlib
        public static PacketWriter WriteDeflated(this PacketWriter pWriter, byte[] data, int offset, int length) {
            if (length <= 4) {
                return pWriter.WriteInt(length).Write(data);
            }

            // We will write the deflated buffer size here later.
            int startIndex = pWriter.Length;
            pWriter.WriteInt(); // Reserve 4 bytes for later

            // Length of inflated buffer for client to use.
            pWriter.WriteIntBigEndian(length);

            var deflater = new Deflater(Deflater.BEST_SPEED);
            deflater.SetInput(data, offset, length);
            deflater.Finish();

            while (true) {
                int count = deflater.Deflate(pWriter.Buffer, pWriter.Length, pWriter.Remaining);
                if (count <= 0) {
                    break;
                }

                pWriter.Seek(pWriter.Length + count);
                pWriter.ResizeBuffer(pWriter.Length * 2);
            }

            // We need to seek backwards to write the deflated size since we can't know them beforehand.
            int endIndex = pWriter.Length;
            pWriter.Seek(startIndex);
            pWriter.WriteInt(endIndex - startIndex - INT_SIZE);
            pWriter.Seek(endIndex);

            return pWriter;
        }

        private static PacketWriter WriteIntBigEndian(this PacketWriter pWriter, int value) {
            return pWriter.WriteByte((byte) (value >> 24))
                .WriteByte((byte) (value >> 16))
                .WriteByte((byte) (value >> 8))
                .WriteByte((byte) (value));
        }
    }
}