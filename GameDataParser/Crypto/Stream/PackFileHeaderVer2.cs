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
            this.BufferFlag = (Encryption) reader.ReadUInt32(); //[ecx+8]
            this.FileIndex = reader.ReadInt32(); //[ecx+12]
            this.EncodedFileSize = reader.ReadUInt32(); //[ecx+16]
            this.CompressedFileSize = reader.ReadUInt64(); //[ecx+20] | [ecx+24]
            this.FileSize = reader.ReadUInt64(); //[ecx+28] | [ecx+32]
            this.Offset = reader.ReadUInt64(); //[ecx+36] | [ecx+40]
        }

        public static PackFileHeaderVer2 CreateHeader(int index, Encryption dwFlag, ulong offset,
                byte[] data)
        {
            CryptoManager.Encrypt(PackVersion.NS2F, data, dwFlag, out uint size, out uint compressedSize,
                out uint encodedSize);

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
            pWriter.Write((uint) this.BufferFlag);
            pWriter.Write(this.FileIndex);
            pWriter.Write(this.EncodedFileSize);
            pWriter.Write(this.CompressedFileSize);
            pWriter.Write(this.FileSize);
            pWriter.Write(this.Offset);
        }
    }
}
