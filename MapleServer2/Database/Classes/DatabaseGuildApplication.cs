using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseGuildApplication
    {
        public static long CreateGuildApplication(GuildApplication guildApplication)
        {
            return DatabaseManager.QueryFactory.Query("guildapplications").InsertGetId<long>(new
            {
                guildApplication.CharacterId,
                guildApplication.GuildId,
                guildApplication.CreationTimestamp
            });
        }

        public static GuildApplication FindById(long id) => DatabaseManager.QueryFactory.Query("guildapplications").Where("Id", id).Get<GuildApplication>().FirstOrDefault();

        public static List<GuildApplication> FindAllByGuildId(long guildId) => DatabaseManager.QueryFactory.Query("guildapplications").Where("GuildId", guildId).Get<GuildApplication>().ToList();

        public static void Update(GuildApplication guildApplication)
        {
            DatabaseManager.QueryFactory.Query("table").Where("Id", guildApplication.Id).Update(new
            {
                guildApplication.CharacterId,
                guildApplication.GuildId,
                guildApplication.CreationTimestamp
            });
        }

        public static bool Delete(long id) => DatabaseManager.QueryFactory.Query("guildapplications").Where("Id", id).Delete() == 1;
    }
}
