using System;
using System.Collections.Generic;
using System.IO;
using GameDataParser.Crypto.Common;

namespace GameDataParser.Crypto.Stream
{
    public interface IPackStream
    {
        // Represents the format of the packed stream (MS2F/NS2F/etc)
        public PackVersion Version { get; }

        // The total compressed size of the (raw) file list
        public ulong CompressedHeaderSize { get; set; }
        // The total (base64) encoded size of the file list
        public ulong EncodedHeaderSize { get; set; }
        // The total size of the raw (decoded, decompressed) file list
        public ulong HeaderSize { get; set; }

        // The total compressed size of the (raw) file table
        public ulong CompressedDataSize { get; set; }
        // The total (base64) encoded size of the file table
        public ulong EncodedDataSize { get; set; }
        // The total size of the raw (decoded, decompressed) file table
        public ulong DataSize { get; set; }

        // The total count of files within the data file
        public ulong FileListCount { get; set; }
        // Represents a list of file info containers (<Index>,<Hash>,<Name>)
        public List<PackFileEntry> FileList { get; }

        public void Encode(BinaryWriter pWriter); // Encodes the header/data pack sizes to stream

        public void InitFileList(BinaryReader reader);

        /*
         * Creates a new packed stream based on the type of version.
         *
         * @param pHeader The stream to read the pack version from
         *
         * @return A packed stream with header sizes decoded
         *
        */
        public static IPackStream CreateStream(BinaryReader pHeader)
        {
            var version = (PackVersion) pHeader.ReadUInt32();
            return version switch
            {
                PackVersion.MS2F => PackStreamVer1.ParseHeader(pHeader),
                PackVersion.NS2F => PackStreamVer2.ParseHeader(pHeader),
                PackVersion.OS2F => PackStreamVer3.ParseHeader(pHeader, version),
                PackVersion.PS2F => PackStreamVer3.ParseHeader(pHeader, version),
                _ => throw new ArgumentException($"Invalid PackVersion:{version}")
            };
        }
    }
}
