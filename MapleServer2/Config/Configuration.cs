using MapleServer2.Config.Configs;

namespace MapleServer2.Config
{
    public class Configuration
    {
        public static Configuration Current { get; set; }
        public ServerConfig Server { get; set; }
    }
}
