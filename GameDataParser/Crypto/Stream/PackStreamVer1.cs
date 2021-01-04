using System.Collections.Generic;
using System.IO;
using GameDataParser.Crypto.Common;

namespace GameDataParser.Crypto.Stream
{
    public class PackStreamVer1 : IPackStream
    {
        public PackVersion Version => PackVersion.MS2F;

        public ulong CompressedHeaderSize { get; set; }
        public ulong EncodedHeaderSize { get; set; }
        public ulong HeaderSize { get; set; }

        public ulong CompressedDataSize { get; set; }
        public ulong EncodedDataSize { get; set; }
        public ulong DataSize { get; set; }

        public ulong FileListCount { get; set; }
        public List<PackFileEntry> FileList { get; }

        private uint uReserved;

        private PackStreamVer1()
        {
            this.FileList = new List<PackFileEntry>();
        }

        public static IPackStream ParseHeader(BinaryReader reader)
        {
            return new PackStreamVer1
            {
                uReserved = reader.ReadUInt32(),
                CompressedDataSize = reader.ReadUInt64(),
                EncodedDataSize = reader.ReadUInt64(),
                HeaderSize = reader.ReadUInt64(),
                CompressedHeaderSize = reader.ReadUInt64(),
                EncodedHeaderSize = reader.ReadUInt64(),
                FileListCount = reader.ReadUInt64(),
                DataSize = reader.ReadUInt64()
            };
        }

        public void Encode(BinaryWriter pWriter)
        {
            pWriter.Write(this.uReserved);
            pWriter.Write(this.CompressedDataSize);
            pWriter.Write(this.EncodedDataSize);
            pWriter.Write(this.HeaderSize);
            pWriter.Write(this.CompressedHeaderSize);
            pWriter.Write(this.EncodedHeaderSize);
            pWriter.Write(this.FileListCount);
            pWriter.Write(this.DataSize);
        }

        public void InitFileList(BinaryReader reader)
        {
            for (ulong i = 0; i < FileListCount; i++)
            {
                IPackFileHeader fileHeader = new PackFileHeaderVer1(reader);
                FileList[fileHeader.FileIndex - 1].FileHeader = fileHeader;
            }
        }
    }
}
