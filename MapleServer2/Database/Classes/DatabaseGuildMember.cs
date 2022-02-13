using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseGuildMember : DatabaseTable
{
    public DatabaseGuildMember() : base("guild_members") { }

    public void Insert(GuildMember guildMember)
    {
        QueryFactory.Query(TableName).Insert(new
        {
            guildMember.Id,
            guildMember.Motto,
            guildMember.Rank,
            daily_contribution = guildMember.DailyContribution,
            contribution_total = guildMember.ContributionTotal,
            daily_donation_count = guildMember.DailyDonationCount,
            attendance_timestamp = guildMember.AttendanceTimestamp,
            join_timestamp = guildMember.JoinTimestamp,
            last_log_timestamp = guildMember.LastLogTimestamp,
            guild_id = guildMember.GuildId
        });
    }

    public GuildMember FindById(long id)
    {
        return QueryFactory.Query(TableName).Where("id", id).Get<GuildMember>().FirstOrDefault();
    }

    public List<GuildMember> FindAllByGuildId(long guildId)
    {
        List<GuildMember> members = new();
        IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("guild_id", guildId).Get();
        foreach (dynamic data in results)
        {
            members.Add(ReadGuildMember(data));
        }
        return members;
    }

    public void Update(GuildMember guildMember)
    {
        QueryFactory.Query(TableName).Where("id", guildMember.Id).Update(new
        {
            guildMember.Rank,
            daily_contribution = guildMember.DailyContribution,
            contribution_total = guildMember.ContributionTotal,
            daily_donation_count = guildMember.DailyDonationCount,
            attendance_timestamp = guildMember.AttendanceTimestamp,
            join_timestamp = guildMember.JoinTimestamp,
            last_log_timestamp = guildMember.LastLogTimestamp,
            guildMember.Motto,
            guild_id = guildMember.GuildId
        });
    }

    public bool Delete(long id)
    {
        return QueryFactory.Query(TableName).Where("id", id).Delete() == 1;
    }

    private static GuildMember ReadGuildMember(dynamic data)
    {
        return new GuildMember(data.id, data.rank, data.daily_contribution, data.contribution_total,
                                                        data.daily_donation_count, data.attendance_timestamp, data.join_timestamp, data.last_log_timestamp, data.guild_id, data.motto,
                                                        DatabaseManager.Characters.FindPartialPlayerById(data.id));
    }
}
