namespace MapleServer2.Types
{
    public class GuildService
    {
        public int Id { get; set; }
        public int Level { get; set; }

        public GuildService(int id, int level)
        {
            Id = id;
            Level = level;
        }
    }
}
