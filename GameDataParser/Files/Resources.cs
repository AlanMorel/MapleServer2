using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using GameDataParser.Crypto.Common;

namespace GameDataParser.Files
{
    public class MetadataResources
    {
        public readonly M2dReader XmlReader;
        public readonly M2dReader ExportedReader;

        public MetadataResources()
        {
            string xmlHeaderPath = $"{Paths.INPUT}/Xml.m2h";
            string exportedHeaderPath = $"{Paths.INPUT}/Exported.m2h";

            XmlMemFile = MemoryMappedFile.CreateFromFile(xmlPath);
            ExportedMemFile = MemoryMappedFile.CreateFromFile(exportedPath);

            XmlFiles = FileList.ReadFile(File.OpenRead(xmlHeaderPath));
            ExportedFiles = FileList.ReadFile(File.OpenRead(exportedHeaderPath));
        }
    }
}
