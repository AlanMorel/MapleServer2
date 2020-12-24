using System.Collections.Generic;
using System.IO;
using System.Text;
using GameDataParser.Crypto;
using GameDataParser.Crypto.Common;
using GameDataParser.Crypto.Stream;

namespace GameDataParser.Files
{
    public class FileList
    {
        public static List<PackFileEntry> ReadFile(Stream headerFile)
        {
            using var headerReader = new BinaryReader(headerFile);
            var stream = IPackStream.CreateStream(headerReader);

            string fileString =
                Encoding.UTF8.GetString(CryptoManager.DecryptFileString(stream, headerReader.BaseStream));
            stream.FileList.AddRange(PackFileEntry.CreateFileList(fileString));
            stream.FileList.Sort();

            // Load the file allocation table and assign each file header to the entry within the list
            byte[] fileTable = CryptoManager.DecryptFileTable(stream, headerReader.BaseStream);

            using var tableStream = new MemoryStream(fileTable);
            using var reader = new BinaryReader(tableStream);
            stream.InitFileList(reader);

            return stream.FileList;
        }
    }
}
