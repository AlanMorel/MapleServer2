using System.Collections.Generic;
using System.IO;
using GameDataParser.Crypto.Common;

namespace GameDataParser.Crypto.Stream
{
    public class PackStreamVer2 : IPackStream
    {
        public PackVersion Version => PackVersion.NS2F;

        public ulong CompressedHeaderSize { get; set; }
        public ulong EncodedHeaderSize { get; set; }
        public ulong HeaderSize { get; set; }

        public ulong CompressedDataSize { get; set; }
        public ulong EncodedDataSize { get; set; }
        public ulong DataSize { get; set; }

        public ulong FileListCount { get; set; }
        public List<PackFileEntry> FileList { get; }

        private PackStreamVer2()
        {
            FileList = new List<PackFileEntry>();
        }

        public static PackStreamVer2 ParseHeader(BinaryReader reader)
        {
            return new PackStreamVer2
            {
                FileListCount = reader.ReadUInt32(),
                CompressedDataSize = reader.ReadUInt64(),
                EncodedDataSize = reader.ReadUInt64(),
                HeaderSize = reader.ReadUInt64(),
                CompressedHeaderSize = reader.ReadUInt64(),
                EncodedHeaderSize = reader.ReadUInt64(),
                DataSize = reader.ReadUInt64()
            };
        }

        public void Encode(BinaryWriter pWriter)
        {
            pWriter.Write(FileListCount);
            pWriter.Write(CompressedDataSize);
            pWriter.Write(EncodedDataSize);
            pWriter.Write(HeaderSize);
            pWriter.Write(CompressedHeaderSize);
            pWriter.Write(EncodedHeaderSize);
            pWriter.Write(DataSize);
        }

        public void InitFileList(BinaryReader reader)
        {
            for (ulong i = 0; i < FileListCount; i++)
            {
                IPackFileHeader fileHeader = new PackFileHeaderVer2(reader);
                FileList[fileHeader.FileIndex - 1].FileHeader = fileHeader;
            }
        }
    }
}
