using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseLevels : DatabaseTable
    {
        public DatabaseLevels() : base("levels") { }

        public long Insert(Levels levels)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                levels.Level,
                levels.Exp,
                levels.RestExp,
                levels.PrestigeLevel,
                levels.PrestigeExp,
                masteryexp = JsonConvert.SerializeObject(levels.MasteryExp)
            });
        }

        public void Update(Levels levels)
        {
            QueryFactory.Query(TableName).Where("id", levels.Id).Update(new
            {
                levels.Level,
                levels.Exp,
                levels.RestExp,
                levels.PrestigeLevel,
                levels.PrestigeExp,
                masteryexp = JsonConvert.SerializeObject(levels.MasteryExp)
            });
        }

        public bool Delete(long id) => QueryFactory.Query(TableName).Where("id", id).Delete() == 1;
    }
}
