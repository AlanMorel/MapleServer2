using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseGuildApplication : DatabaseTable
    {
        public DatabaseGuildApplication() : base("guildapplications") { }

        public long Insert(GuildApplication guildApplication)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                guildApplication.CharacterId,
                guildApplication.GuildId,
                guildApplication.CreationTimestamp
            });
        }

        public GuildApplication FindById(long id) => QueryFactory.Query(TableName).Where("id", id).Get<GuildApplication>().FirstOrDefault();

        public List<GuildApplication> FindAllByGuildId(long guildId) => QueryFactory.Query(TableName).Where("guildid", guildId).Get<GuildApplication>().ToList();

        public void Update(GuildApplication guildApplication)
        {
            QueryFactory.Query(TableName).Where("id", guildApplication.Id).Update(new
            {
                guildApplication.CharacterId,
                guildApplication.GuildId,
                guildApplication.CreationTimestamp
            });
        }

        public bool Delete(long id) => QueryFactory.Query(TableName).Where("id", id).Delete() == 1;
    }
}
