using System.Configuration;

namespace GameDataParser.Files
{
    public class SetPath
    {
        // Set path of the Xml.m2d, Xml.m2h & Exported.m2d, Exported.m2h
        public string XML_PATH = ConfigurationManager.AppSettings["XML_PATH"];
        public string EXPORTED_PATH = ConfigurationManager.AppSettings["EXPORTED_PATH"];
    }
}
