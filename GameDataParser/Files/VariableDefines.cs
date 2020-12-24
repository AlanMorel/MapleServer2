using System.Configuration;

namespace GameDataParser.Files
{
    public static class VariableDefines
    {
        // Set path of the Xml.m2d, Xml.m2h & Exported.m2d, Exported.m2h
        public static string XML_PATH = ConfigurationManager.AppSettings["XML_PATH"];
        public static string EXPORTED_PATH = ConfigurationManager.AppSettings["EXPORTED_PATH"];
    }
}
