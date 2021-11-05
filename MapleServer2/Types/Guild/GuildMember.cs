using MapleServer2.Database;

namespace MapleServer2.Types;

public class GuildMember
{
    public long Id;
    public Player Player { get; private set; }
    public string Motto;
    public byte Rank { get; set; } // by index of guild ranks
    public int DailyContribution { get; set; }
    public int ContributionTotal { get; set; }
    public byte DailyDonationCount { get; set; }
    public long AttendanceTimestamp { get; set; }
    public long JoinTimestamp { get; set; }
    public long GuildId;

    public GuildMember(long id, byte rank, int dailyContribution, int contributionTotal, byte dailyDonationCount, long attendanceTimestamp, long joinTimestamp, long guildId, string motto, Player player)
    {
        Id = id;
        Motto = motto;
        Rank = rank;
        DailyContribution = dailyContribution;
        ContributionTotal = contributionTotal;
        DailyDonationCount = dailyDonationCount;
        AttendanceTimestamp = attendanceTimestamp;
        JoinTimestamp = joinTimestamp;
        GuildId = guildId;
        Player = player;
    }

    public GuildMember(Player player, byte rank, long guildId)
    {
        Id = player.CharacterId;
        Player = player;
        Rank = rank;
        Motto = "";
        JoinTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount;
        GuildId = guildId;
        DatabaseManager.GuildMembers.Insert(this);
    }

    public void AddContribution(int contribution)
    {
        ContributionTotal += contribution;
        DailyContribution += contribution;
        DatabaseManager.GuildMembers.Update(this);
    }
}
