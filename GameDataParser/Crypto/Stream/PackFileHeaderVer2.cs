using System.IO;
using GameDataParser.Crypto.Common;

namespace GameDataParser.Crypto.Stream
{
    public class PackFileHeaderVer2 : IPackFileHeader
    {
        public PackVersion Version => PackVersion.NS2F;

        public Encryption BufferFlag { get; set; }
        public int FileIndex { get; set; }
        public ulong Offset { get; set; }
        public uint EncodedFileSize { get; set; }
        public ulong CompressedFileSize { get; set; }
        public ulong FileSize { get; set; }

        private PackFileHeaderVer2()
        {
            // Interesting.. no reserved bytes stored in Ver2.
        }

        public PackFileHeaderVer2(BinaryReader reader) : this()
        {
            BufferFlag = (Encryption) reader.ReadUInt32(); //[ecx+8]
            FileIndex = reader.ReadInt32(); //[ecx+12]
            EncodedFileSize = reader.ReadUInt32(); //[ecx+16]
            CompressedFileSize = reader.ReadUInt64(); //[ecx+20] | [ecx+24]
            FileSize = reader.ReadUInt64(); //[ecx+28] | [ecx+32]
            Offset = reader.ReadUInt64(); //[ecx+36] | [ecx+40]
        }

        public static PackFileHeaderVer2 CreateHeader(int index, Encryption dwFlag, ulong offset, byte[] data)
        {
            CryptoManager.Encrypt(PackVersion.NS2F, data, dwFlag, out uint size, out uint compressedSize, out uint encodedSize);

            return new PackFileHeaderVer2
            {
                BufferFlag = dwFlag,
                FileIndex = index,
                EncodedFileSize = encodedSize,
                CompressedFileSize = compressedSize,
                FileSize = size,
                Offset = offset
            };
        }

        public void Encode(BinaryWriter pWriter)
        {
            pWriter.Write((uint) BufferFlag);
            pWriter.Write(FileIndex);
            pWriter.Write(EncodedFileSize);
            pWriter.Write(CompressedFileSize);
            pWriter.Write(FileSize);
            pWriter.Write(Offset);
        }
    }
}
