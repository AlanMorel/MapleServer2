using System.Collections.Generic;
using System.IO;
using GameDataParser.Crypto.Common;

namespace GameDataParser.Crypto.Stream
{
    public class PackStreamVer3 : IPackStream
    {
        // OS2F/PS2F
        public PackVersion Version { get; }

        public ulong CompressedHeaderSize { get; set; }
        public ulong EncodedHeaderSize { get; set; }
        public ulong HeaderSize { get; set; }

        public ulong CompressedDataSize { get; set; }
        public ulong EncodedDataSize { get; set; }
        public ulong DataSize { get; set; }

        public ulong FileListCount { get; set; }
        public List<PackFileEntry> FileList { get; }

        private uint Reserved;

        private PackStreamVer3(PackVersion version)
        {
            Version = version;
            FileList = new List<PackFileEntry>();
        }

        public static PackStreamVer3 ParseHeader(BinaryReader reader, PackVersion version)
        {
            return new PackStreamVer3(version)
            {
                FileListCount = reader.ReadUInt32(),
                Reserved = reader.ReadUInt32(),
                CompressedDataSize = reader.ReadUInt64(),
                EncodedDataSize = reader.ReadUInt64(),
                CompressedHeaderSize = reader.ReadUInt64(),
                EncodedHeaderSize = reader.ReadUInt64(),
                DataSize = reader.ReadUInt64(),
                HeaderSize = reader.ReadUInt64()
            };
        }

        public void Encode(BinaryWriter pWriter)
        {
            pWriter.Write(FileListCount);
            pWriter.Write(Reserved);
            pWriter.Write(CompressedDataSize);
            pWriter.Write(EncodedDataSize);
            pWriter.Write(CompressedHeaderSize);
            pWriter.Write(EncodedHeaderSize);
            pWriter.Write(DataSize);
            pWriter.Write(HeaderSize);
        }

        public void InitFileList(BinaryReader reader)
        {
            for (ulong i = 0; i < FileListCount; i++)
            {
                IPackFileHeader fileHeader = new PackFileHeaderVer3(Version, reader);
                FileList[fileHeader.FileIndex - 1].FileHeader = fileHeader;
            }
        }
    }
}
