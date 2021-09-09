using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseGuildApplication : DatabaseTable
    {
        public DatabaseGuildApplication() : base("GuildApplications") { }

        public long Insert(GuildApplication guildApplication)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                guildApplication.CharacterId,
                guildApplication.GuildId,
                guildApplication.CreationTimestamp
            });
        }

        public GuildApplication FindById(long id) => QueryFactory.Query(TableName).Where("Id", id).Get<GuildApplication>().FirstOrDefault();

        public List<GuildApplication> FindAllByGuildId(long guildId) => QueryFactory.Query(TableName).Where("GuildId", guildId).Get<GuildApplication>().ToList();

        public void Update(GuildApplication guildApplication)
        {
            QueryFactory.Query(TableName).Where("Id", guildApplication.Id).Update(new
            {
                guildApplication.CharacterId,
                guildApplication.GuildId,
                guildApplication.CreationTimestamp
            });
        }

        public bool Delete(long id) => QueryFactory.Query(TableName).Where("Id", id).Delete() == 1;
    }
}
