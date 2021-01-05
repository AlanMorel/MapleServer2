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

        private uint reserved;

        private PackStreamVer3(PackVersion version)
        {
            this.Version = version;
            this.FileList = new List<PackFileEntry>();
        }

        public static PackStreamVer3 ParseHeader(BinaryReader reader, PackVersion version)
        {
            return new PackStreamVer3(version)
            {
                FileListCount = reader.ReadUInt32(),
                reserved = reader.ReadUInt32(),
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
            pWriter.Write(this.FileListCount);
            pWriter.Write(this.reserved);
            pWriter.Write(this.CompressedDataSize);
            pWriter.Write(this.EncodedDataSize);
            pWriter.Write(this.CompressedHeaderSize);
            pWriter.Write(this.EncodedHeaderSize);
            pWriter.Write(this.DataSize);
            pWriter.Write(this.HeaderSize);
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
