using IniParser.Model;

namespace MapleServer2.Config
{
    public interface IConfig
    {
        void LoadIniData(KeyDataCollection data);
    }
}
