using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public static class DatabaseLevels
    {
        public static long CreateLevels(Levels levels)
        {
            return DatabaseManager.QueryFactory.Query("Levels").InsertGetId<long>(new
            {
                levels.Level,
                levels.Exp,
                levels.RestExp,
                levels.PrestigeLevel,
                levels.PrestigeExp,
                MasteryExp = JsonConvert.SerializeObject(levels.MasteryExp)
            });
        }

        public static void Update(Levels levels)
        {
            DatabaseManager.QueryFactory.Query("Levels").Where("Id", levels.Id).Update(new
            {
                levels.Level,
                levels.Exp,
                levels.RestExp,
                levels.PrestigeLevel,
                levels.PrestigeExp,
                MasteryExp = JsonConvert.SerializeObject(levels.MasteryExp)
            });
        }

        public static bool Delete(long id) => DatabaseManager.QueryFactory.Query("table").Where("Id", id).Delete() == 1;
    }
}
