using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using GameDataParser.Crypto.Common;

namespace GameDataParser.Files
{
    public class MetadataResources
    {
        public List<PackFileEntry> XmlFiles;
        public List<PackFileEntry> ExportedFiles;

        public MemoryMappedFile XmlMemFile;
        public MemoryMappedFile ExportedMemFile;

        public MetadataResources()
        {
            string xmlPath = $"{Paths.INPUT}/Xml.m2d";
            string exportedPath = $"{Paths.INPUT}/Exported.m2d";

            string xmlHeaderPath = $"{Paths.INPUT}/Xml.m2h";
            string exportedHeaderPath = $"{Paths.INPUT}/Exported.m2h";

            XmlMemFile = MemoryMappedFile.CreateFromFile(xmlPath);
            ExportedMemFile = MemoryMappedFile.CreateFromFile(exportedPath);

            XmlFiles = FileList.ReadFile(File.OpenRead(xmlHeaderPath));
            ExportedFiles = FileList.ReadFile(File.OpenRead(exportedHeaderPath));
        }
    }
}
