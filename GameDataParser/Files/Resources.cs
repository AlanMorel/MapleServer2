using Maple2.File.IO;

namespace GameDataParser.Files
{
    public class MetadataResources
    {
        public readonly M2dReader XmlReader;
        public readonly M2dReader ExportedReader;

        public MetadataResources()
        {
            string xmlPath = $"{Paths.INPUT}/Xml.m2d";
            string exportedPath = $"{Paths.INPUT}/Exported.m2d";

            XmlReader = new M2dReader(xmlPath);
            ExportedReader = new M2dReader(exportedPath);
        }
    }
}
