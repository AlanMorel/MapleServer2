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
            BinaryReader headerReader = new BinaryReader(headerFile);
            IPackStream stream = IPackStream.CreateStream(headerReader);

            byte[] fileStringBytes = CryptoManager.DecryptFileString(stream, headerReader.BaseStream);
            string fileString = Encoding.UTF8.GetString(fileStringBytes);

            stream.FileList.AddRange(PackFileEntry.CreateFileList(fileString));
            stream.FileList.Sort();

            // Load the file allocation table and assign each file header to the entry within the list
            byte[] fileTable = CryptoManager.DecryptFileTable(stream, headerReader.BaseStream);

            MemoryStream tableStream = new MemoryStream(fileTable);
            BinaryReader reader = new BinaryReader(tableStream);
            stream.InitFileList(reader);

            return stream.FileList;
        }
    }
}
