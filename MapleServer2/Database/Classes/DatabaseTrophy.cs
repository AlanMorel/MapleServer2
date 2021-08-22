using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseTrophy : DatabaseTable
    {
        public DatabaseTrophy(string tableName) : base(tableName) { }

        public long Insert(Trophy trophy)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                trophy.Id,
                trophy.NextGrade,
                trophy.MaxGrade,
                trophy.Counter,
                trophy.Condition,
                trophy.IsDone,
                trophy.Type,
                Timestamps = JsonConvert.SerializeObject(trophy.Timestamps),
                trophy.CharacterId
            });
        }

        public Dictionary<int, Trophy> FindAllByCharacterId(long characterId)
        {
            IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("CharacterId", characterId).Get();
            Dictionary<int, Trophy> trophies = new Dictionary<int, Trophy>();
            foreach (dynamic data in results)
            {
                Trophy trophy = (Trophy) ReadTrophy(data);
                trophies.Add(trophy.Id, trophy);
            }
            return trophies;
        }

        public void Update(Trophy trophy)
        {
            QueryFactory.Query(TableName).Where("Uid", trophy.Uid).Update(new
            {
                trophy.Id,
                trophy.NextGrade,
                trophy.MaxGrade,
                trophy.Counter,
                trophy.Condition,
                trophy.IsDone,
                trophy.Type,
                Timestamps = JsonConvert.SerializeObject(trophy.Timestamps),
                trophy.CharacterId
            });
        }

        public bool Delete(long uid) => QueryFactory.Query(TableName).Where("Uid", uid).Delete() == 1;

        private static Trophy ReadTrophy(dynamic data) => new Trophy(data.Uid, data.Id, data.NextGrade, data.MaxGrade, data.Counter, data.Condition, data.IsDone, data.Type, JsonConvert.DeserializeObject<List<long>>(data.Timestamps), data.CharacterId);
    }
}
