using MapleServer2.Database;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class GuildApplication
    {
        public long Id { get; }
        public long GuildId { get; set; }
        public long CharacterId { get; set; }
        public long CreationTimestamp { get; }

        public GuildApplication() { }
        public GuildApplication(long player, long guild)
        {
            Id = GuidGenerator.Long();
            CharacterId = player;
            GuildId = guild;
            CreationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount;
            DatabaseManager.CreateGuildApplication(this);
        }

        public void Add(Player player, Guild guild)
        {
            player.GuildApplications.Add(this);
            guild.Applications.Add(this);
            DatabaseManager.Update(guild);
        }

        public void Remove(Player player, Guild guild)
        {
            player.GuildApplications.Remove(this);
            guild.Applications.Remove(this);
            DatabaseManager.Delete(this);
            DatabaseManager.Update(guild);
        }
    }
}
