using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseTrophy : DatabaseTable
    {
        public DatabaseTrophy() : base("Trophies") { }

        public long Insert(Trophy trophy)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                trophy.Id,
                trophy.NextGrade,
                trophy.Counter,
                trophy.IsDone,
                trophy.LastReward,
                Timestamps = JsonConvert.SerializeObject(trophy.Timestamps),
                CharacterId = trophy.CharacterId == 0 ? null : (long?) trophy.CharacterId,
                AccountId = trophy.AccountId == 0 ? null : (long?) trophy.AccountId
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

        public Dictionary<int, Trophy> FindAllByAccountId(long accountId)
        {
            IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("AccountId", accountId).Get();
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
                trophy.Counter,
                trophy.IsDone,
                trophy.LastReward,
                Timestamps = JsonConvert.SerializeObject(trophy.Timestamps),
                CharacterId = trophy.CharacterId == 0 ? null : (long?) trophy.CharacterId,
                AccountId = trophy.AccountId == 0 ? null : (long?) trophy.AccountId
            });
        }

        public bool Delete(long uid) => QueryFactory.Query(TableName).Where("Uid", uid).Delete() == 1;

        private static Trophy ReadTrophy(dynamic data) => new Trophy(data.Uid, data.Id, data.NextGrade, data.Counter, data.IsDone, data.LastReward, JsonConvert.DeserializeObject<List<long>>(data.Timestamps), data.CharacterId ?? 0, data.AccountId ?? 0);
    }
}
