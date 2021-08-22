using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseLevels
    {
        private readonly string TableName = "Levels";

        public long CreateLevels(Levels levels)
        {
            return DatabaseManager.QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                levels.Level,
                levels.Exp,
                levels.RestExp,
                levels.PrestigeLevel,
                levels.PrestigeExp,
                MasteryExp = JsonConvert.SerializeObject(levels.MasteryExp)
            });
        }

        public void Update(Levels levels)
        {
            DatabaseManager.QueryFactory.Query(TableName).Where("Id", levels.Id).Update(new
            {
                levels.Level,
                levels.Exp,
                levels.RestExp,
                levels.PrestigeLevel,
                levels.PrestigeExp,
                MasteryExp = JsonConvert.SerializeObject(levels.MasteryExp)
            });
        }

        public bool Delete(long id) => DatabaseManager.QueryFactory.Query(TableName).Where("Id", id).Delete() == 1;
    }
}
