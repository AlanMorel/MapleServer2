using System;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class GuildMember
    {
        public long CharacterId { get; }
        public Player Player { get; set; }
        public string Motto = "";
        public byte Rank { get; set; } // by index of guild ranks
        public int DailyContribution { get; set; }
        public int ContributionTotal { get; set; }
        public byte DailyDonationCount { get; set; }
        public long AttendanceTimestamp { get; set; }
        public long JoinTimestamp { get; set; }

        public GuildMember() { }

        public GuildMember(Player player)
        {
            CharacterId = player.CharacterId;
            Player = player;
            Rank = 5;
            JoinTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount;
        }

        public void AddContribution(int contribution)
        {
            ContributionTotal += contribution;
            DailyContribution += contribution;
        }
    }
}
