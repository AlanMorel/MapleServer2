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
            using BinaryReader headerReader = new BinaryReader(headerFile);
            IPackStream stream = IPackStream.CreateStream(headerReader);

            string fileString =
                Encoding.UTF8.GetString(CryptoManager.DecryptFileString(stream, headerReader.BaseStream));
            stream.FileList.AddRange(PackFileEntry.CreateFileList(fileString));
            stream.FileList.Sort();

            // Load the file allocation table and assign each file header to the entry within the list
            byte[] fileTable = CryptoManager.DecryptFileTable(stream, headerReader.BaseStream);

            using MemoryStream tableStream = new MemoryStream(fileTable);
            using BinaryReader reader = new BinaryReader(tableStream);
            stream.InitFileList(reader);

            return stream.FileList;
        }
    }
}
