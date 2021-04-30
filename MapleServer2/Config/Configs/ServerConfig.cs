namespace MapleServer2.Config.Configs
{
    public class ServerConfig : ConfigBase<ServerConfig>
    {
        public string IP { get; set; } = "127.0.0.1";
        public int LoginPort { get; set; } = 20001;
        public int GamePort { get; set; } = 21001;
        public string Name { get; set; } = "Name";
    }
}
