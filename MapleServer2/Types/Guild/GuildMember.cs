using MapleServer2.Database;

namespace MapleServer2.Types
{
    public class GuildMember
    {
        public long Id;
        public Player Player { get; set; }
        public string Motto;
        public byte Rank { get; set; } // by index of guild ranks
        public int DailyContribution { get; set; }
        public int ContributionTotal { get; set; }
        public byte DailyDonationCount { get; set; }
        public long AttendanceTimestamp { get; set; }
        public long JoinTimestamp { get; set; }

        public GuildMember() { }

        public GuildMember(Player player, byte rank)
        {
            Id = player.CharacterId;
            Player = player;
            Rank = rank;
            Motto = "";
            JoinTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount;
            DatabaseManager.CreateGuildMember(this);
            player.GuildMember = this;
        }

        public void AddContribution(int contribution)
        {
            ContributionTotal += contribution;
            DailyContribution += contribution;
            DatabaseManager.Update(this);
        }
    }
}
