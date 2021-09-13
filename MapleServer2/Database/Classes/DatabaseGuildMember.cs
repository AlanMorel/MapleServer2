using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseGuildMember : DatabaseTable
    {
        public DatabaseGuildMember() : base("guildmembers") { }

        public void Insert(GuildMember guildMember)
        {
            QueryFactory.Query(TableName).Insert(new
            {
                guildMember.Id,
                guildMember.Motto,
                guildMember.Rank,
                guildMember.DailyContribution,
                guildMember.ContributionTotal,
                guildMember.DailyDonationCount,
                guildMember.AttendanceTimestamp,
                guildMember.JoinTimestamp,
                guildMember.GuildId
            });
        }

        public GuildMember FindById(long id) => QueryFactory.Query(TableName).Where("id", id).Get<GuildMember>().FirstOrDefault();

        public List<GuildMember> FindAllByGuildId(long guildId)
        {
            List<GuildMember> members = QueryFactory.Query(TableName).Where("guildid", guildId).Get<GuildMember>().ToList();
            foreach (GuildMember guildMember in members)
            {
                guildMember.Player = DatabaseManager.Characters.FindPartialPlayerById(guildMember.Id);
            }
            return members;
        }

        public void Update(GuildMember guildMember)
        {
            QueryFactory.Query(TableName).Where("id", guildMember.Id).Update(new
            {
                guildMember.Rank,
                guildMember.DailyContribution,
                guildMember.ContributionTotal,
                guildMember.DailyDonationCount,
                guildMember.AttendanceTimestamp,
                guildMember.JoinTimestamp,
                guildMember.Motto,
                guildMember.GuildId
            });
        }

        public bool Delete(long id) => QueryFactory.Query(TableName).Where("id", id).Delete() == 1;
    }
}
