using IniParser;
using IniParser.Model;

namespace MapleServer2.Config
{
    public static class ConfigHandler
    {
        public static IniData Data { get; set; }

        public static void Load(string file, string commentstring)
        {
            FileIniDataParser parser = new FileIniDataParser();
            parser.Parser.Configuration.CommentString = commentstring;
            Data = parser.ReadFile(file);
        }
    }
}
