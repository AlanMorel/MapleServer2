using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using GameDataParser.Crypto.Common;

namespace GameDataParser.Files
{
    public class MetadataResources
    {
        public List<PackFileEntry> xmlFiles;
        public List<PackFileEntry> exportedFiles;

        public MemoryMappedFile xmlMemFile;
        public MemoryMappedFile exportedMemFile;

        public MetadataResources()
        {
            string xmlPath = $"{Paths.INPUT}/Xml.m2d";
            string exportedPath = $"{Paths.INPUT}/Exported.m2d";

            string xmlHeaderPath = $"{Paths.INPUT}/Xml.m2h";
            string exportedHeaderPath = $"{Paths.INPUT}/Exported.m2h";

            this.xmlMemFile = MemoryMappedFile.CreateFromFile(xmlPath);
            this.exportedMemFile = MemoryMappedFile.CreateFromFile(exportedPath);

            this.xmlFiles = FileList.ReadFile(File.OpenRead(xmlHeaderPath));
            this.exportedFiles = FileList.ReadFile(File.OpenRead(exportedHeaderPath));
        }
    }
}
