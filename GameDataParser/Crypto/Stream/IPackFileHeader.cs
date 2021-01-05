using System.IO;
using GameDataParser.Crypto.Common;

namespace GameDataParser.Crypto.Stream
{
    public interface IPackFileHeader
    {
        // Represents the format of the packed stream (MS2F/NS2F/etc)
        public PackVersion Version { get; }

        // The flag that determines buffer manipulation
        public Encryption BufferFlag { get; set; }

        // The index of this file located within the lookup table
        public int FileIndex { get; set; }

        // The start offset of this file's data within the m2d file
        public ulong Offset { get; set; }

        // The total (base64) encoded size of the file
        public uint EncodedFileSize { get; set; }
        // The total compressed size of the (raw) file
        public ulong CompressedFileSize { get; set; }
        // The total size of the raw (decoded, decompressed) file
        public ulong FileSize { get; set; }


        public void Encode(BinaryWriter pWriter); // Encodes the contents of this file to stream
    }
}
