using MapleServer2.Database.Classes;

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
            CharacterId = player;
            GuildId = guild;
            CreationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount;
            Id = DatabaseGuildApplication.CreateGuildApplication(this);
        }

        public void Add(Player player, Guild guild)
        {
            player.GuildApplications.Add(this);
            guild.Applications.Add(this);
            DatabaseCharacter.Update(player);
        }

        public void Remove(Player player, Guild guild)
        {
            player.GuildApplications.Remove(this);
            guild.Applications.Remove(this);
            DatabaseGuildApplication.Delete(Id);
            DatabaseCharacter.Update(player);
        }
    }
}
