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

        private readonly int[] Reserved;

        private PackFileHeaderVer3(PackVersion version)
        {
            Version = version;
            Reserved = new int[1];
        }

        public PackFileHeaderVer3(PackVersion version, BinaryReader reader) : this(version)
        {
            BufferFlag = (Encryption) reader.ReadUInt32(); //[ecx+8]
            FileIndex = reader.ReadInt32(); //[ecx+12]
            EncodedFileSize = reader.ReadUInt32(); //[ecx+16]
            Reserved[0] = reader.ReadInt32(); //[ecx+20]
            CompressedFileSize = reader.ReadUInt64(); //[ecx+24] | [ecx+28]
            FileSize = reader.ReadUInt64(); //[ecx+32] | [ecx+36]
            Offset = reader.ReadUInt64(); //[ecx+40] | [ecx+44]
        }

        public static PackFileHeaderVer3 CreateHeader(PackVersion version, int index, Encryption flag, ulong offset, byte[] data)
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
            pWriter.Write((uint) BufferFlag);
            pWriter.Write(FileIndex);
            pWriter.Write(EncodedFileSize);
            pWriter.Write(Reserved[0]);
            pWriter.Write(CompressedFileSize);
            pWriter.Write(FileSize);
            pWriter.Write(Offset);
        }
    }
}
