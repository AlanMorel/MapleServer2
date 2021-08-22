using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseGuildApplication
    {
        private readonly string TableName = "guildapplications";

        public long Insert(GuildApplication guildApplication)
        {
            return DatabaseManager.QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                guildApplication.CharacterId,
                guildApplication.GuildId,
                guildApplication.CreationTimestamp
            });
        }

        public GuildApplication FindById(long id) => DatabaseManager.QueryFactory.Query(TableName).Where("Id", id).Get<GuildApplication>().FirstOrDefault();

        public List<GuildApplication> FindAllByGuildId(long guildId) => DatabaseManager.QueryFactory.Query(TableName).Where("GuildId", guildId).Get<GuildApplication>().ToList();

        public void Update(GuildApplication guildApplication)
        {
            DatabaseManager.QueryFactory.Query(TableName).Where("Id", guildApplication.Id).Update(new
            {
                guildApplication.CharacterId,
                guildApplication.GuildId,
                guildApplication.CreationTimestamp
            });
        }

        public bool Delete(long id) => DatabaseManager.QueryFactory.Query(TableName).Where("Id", id).Delete() == 1;
    }
}
