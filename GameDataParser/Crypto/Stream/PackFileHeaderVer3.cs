using System.IO;
using GameDataParser.Crypto.Common;

namespace GameDataParser.Crypto.Stream
{
    public class PackFileHeaderVer3 : IPackFileHeader
    {
        // OS2F/PS2F
        public PackVersion Version { get; }

        public Encryption BufferFlag { get; set; }
        public int FileIndex { get; set; }
        public ulong Offset { get; set; }
        public uint EncodedFileSize { get; set; }
        public ulong CompressedFileSize { get; set; }
        public ulong FileSize { get; set; }

        private readonly int[] reserved;

        private PackFileHeaderVer3(PackVersion version)
        {
            this.Version = version;
            this.reserved = new int[1];
        }

        public PackFileHeaderVer3(PackVersion version, BinaryReader reader) : this(version)
        {
            this.BufferFlag = (Encryption) reader.ReadUInt32(); //[ecx+8]
            this.FileIndex = reader.ReadInt32(); //[ecx+12]
            this.EncodedFileSize = reader.ReadUInt32(); //[ecx+16]
            this.reserved[0] = reader.ReadInt32(); //[ecx+20]
            this.CompressedFileSize = reader.ReadUInt64(); //[ecx+24] | [ecx+28]
            this.FileSize = reader.ReadUInt64(); //[ecx+32] | [ecx+36]
            this.Offset = reader.ReadUInt64(); //[ecx+40] | [ecx+44]
        }

        public static PackFileHeaderVer3 CreateHeader(PackVersion version, int index, Encryption flag, ulong offset,
                byte[] data)
        {
            CryptoManager.Encrypt(version, data, flag, out uint size, out uint compressedSize, out uint encodedSize);

            return new PackFileHeaderVer3(version)
            {
                BufferFlag = flag,
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
            pWriter.Write(this.reserved[0]);
            pWriter.Write(this.CompressedFileSize);
            pWriter.Write(this.FileSize);
            pWriter.Write(this.Offset);
        }
    }
}
