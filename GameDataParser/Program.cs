using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Xml;
using GameDataParser.Crypto;
using GameDataParser.Crypto.Common;
using GameDataParser.Crypto.Stream;
using GameDataParser.Parsers;
using Maple2Storage.Types;

namespace GameDataParser {
    internal static class Program {
        private const string XML_PATH = @"C:\Nexon\Library\maplestory2ps\appdata\Data\Xml.m2d";
        private const string EXPORTED_PATH = @"C:\Nexon\Library\maplestory2ps\appdata\Data\Resource\Exported.m2d";


        private static void Main() {
            /*string headerFile = XML_PATH.Replace(".m2d", ".m2h");
            List<PackFileEntry> files = ReadFile(File.OpenRead(headerFile));
            var memFile = MemoryMappedFile.CreateFromFile(XML_PATH);

            // Parse and save some item data from xml file
            List<ItemMetadata> items = ItemParser.Parse(memFile, files);
            ItemParser.Write(items);*/

            string headerFile = EXPORTED_PATH.Replace(".m2d", ".m2h");
            List<PackFileEntry> files = ReadFile(File.OpenRead(headerFile));
            var memFile = MemoryMappedFile.CreateFromFile(EXPORTED_PATH);
            List<MapEntityMetadata> entities = MapEntityParser.Parse(memFile, files);
            MapEntityParser.Write(entities);
        }

        private static List<PackFileEntry> ReadFile(Stream headerFile) {
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

        public static XmlReader GetReader(this MemoryMappedFile m2dFile, IPackFileHeader header) {
            return XmlReader.Create(new MemoryStream(CryptoManager.DecryptData(header, m2dFile)));
        }

        public static XmlDocument GetDocument(this MemoryMappedFile m2dFile, IPackFileHeader header) {
            var document = new XmlDocument();
            document.Load(new MemoryStream(CryptoManager.DecryptData(header, m2dFile)));
            return document;
        }
    }
}