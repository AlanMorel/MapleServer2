using System.IO;
using System.IO.MemoryMappedFiles;
using System.Xml;
using GameDataParser.Crypto;
using GameDataParser.Crypto.Stream;
using GameDataParser.Files.Export;

namespace GameDataParser
{
    internal static class Program
    {
        public static ItemMetadataExport Item = new ItemMetadataExport();
        public static MapMetadataExport MapEntity = new MapMetadataExport();
        public static SkillMetadataExport Skill = new SkillMetadataExport();

        private static void Main()
        {
            Item.Export();
            MapEntity.Export();
            Skill.Export();
        }

        public static XmlReader GetReader(this MemoryMappedFile m2dFile, IPackFileHeader header)
        {
            return XmlReader.Create(new MemoryStream(CryptoManager.DecryptData(header, m2dFile)));
        }

        public static XmlDocument GetDocument(this MemoryMappedFile m2dFile, IPackFileHeader header)
        {
            XmlDocument document = new XmlDocument();
            document.Load(new MemoryStream(CryptoManager.DecryptData(header, m2dFile)));
            return document;
        }
    }
}
