namespace MapleServer2.Types
{
    public class GuildRank
    {
        public string Name { get; set; }
        public int Rights { get; set; }

        public GuildRank(string name, int rights = 1)
        {
            Name = name;
            Rights = rights;
        }
    }

    [Flags]
    public enum GuildRights
    {
        Default = 1,
        CanInvite = 2,
        CanGuildNotice = 8,
        CanEditEmblem = 64,
        CanGuildMail = 128,
        CanStartMiniGame = 1024,
        CanGuildAlert = 2048
    }
}
