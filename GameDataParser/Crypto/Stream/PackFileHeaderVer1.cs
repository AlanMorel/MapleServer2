using System.IO;
using GameDataParser.Crypto.Common;

namespace GameDataParser.Crypto.Stream
{
    public class PackFileHeaderVer1 : IPackFileHeader
    {
        public PackVersion Version => PackVersion.MS2F;

        public Encryption BufferFlag { get; set; }
        public int FileIndex { get; set; }
        public ulong Offset { get; set; }
        public uint EncodedFileSize { get; set; }
        public ulong CompressedFileSize { get; set; }
        public ulong FileSize { get; set; }

        private readonly byte[] PackingDef; //A "Packing Definition", unused.
        private readonly int[] Reserved;

        private PackFileHeaderVer1()
        {
            PackingDef = new byte[4];
            Reserved = new int[2];
        }

        public PackFileHeaderVer1(BinaryReader reader) : this()
        {
            PackingDef = reader.ReadBytes(4); //[ecx+16]
            FileIndex = reader.ReadInt32(); //[ecx+20]
            BufferFlag = (Encryption) reader.ReadUInt32(); //[ecx+24]
            Reserved[0] = reader.ReadInt32(); //[ecx+28]
            Offset = reader.ReadUInt64(); //[ecx+32] | [ecx+36]
            EncodedFileSize = reader.ReadUInt32(); //[ecx+40]
            Reserved[1] = reader.ReadInt32(); //[ecx+44]
            CompressedFileSize = reader.ReadUInt64(); //[ecx+48] | [ecx+52]
            FileSize = reader.ReadUInt64(); //[ecx+56] | [ecx+60]
        }

        public static PackFileHeaderVer1 CreateHeader(int index, Encryption dwFlag, ulong offset,
                byte[] data)
        {
            CryptoManager.Encrypt(PackVersion.MS2F, data, dwFlag, out uint size, out uint compressedSize,
                out uint encodedSize);

            return new PackFileHeaderVer1
            {
                FileIndex = index,
                BufferFlag = dwFlag,
                Offset = offset,
                EncodedFileSize = encodedSize,
                CompressedFileSize = compressedSize,
                FileSize = size
            };
        }

        public void Encode(BinaryWriter pWriter)
        {
            pWriter.Write(PackingDef);
            pWriter.Write(FileIndex);
            pWriter.Write((uint) BufferFlag);
            pWriter.Write(Reserved[0]);
            pWriter.Write(Offset);
            pWriter.Write(EncodedFileSize);
            pWriter.Write(Reserved[1]);
            pWriter.Write(CompressedFileSize);
            pWriter.Write(FileSize);
        }
    }
}
