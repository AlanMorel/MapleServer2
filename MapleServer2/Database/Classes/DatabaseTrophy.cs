using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseTrophy : DatabaseTable
    {
        public DatabaseTrophy() : base("trophies") { }

        public long Insert(Trophy trophy)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                trophy.Id,
                trophy.NextGrade,
                trophy.Counter,
                trophy.IsDone,
                trophy.LastReward,
                timestamps = JsonConvert.SerializeObject(trophy.Timestamps),
                characterid = trophy.CharacterId == 0 ? null : (long?) trophy.CharacterId,
                accountid = trophy.AccountId == 0 ? null : (long?) trophy.AccountId
            });
        }

        public Dictionary<int, Trophy> FindAllByCharacterId(long characterId)
        {
            IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("characterid", characterId).Get();
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
            IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("accountid", accountId).Get();
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
            QueryFactory.Query(TableName).Where("uid", trophy.Uid).Update(new
            {
                trophy.Id,
                trophy.NextGrade,
                trophy.Counter,
                trophy.IsDone,
                trophy.LastReward,
                timestamps = JsonConvert.SerializeObject(trophy.Timestamps),
                characterid = trophy.CharacterId == 0 ? null : (long?) trophy.CharacterId,
                accountid = trophy.AccountId == 0 ? null : (long?) trophy.AccountId
            });
        }

        public bool Delete(long uid) => QueryFactory.Query(TableName).Where("uid", uid).Delete() == 1;

        private static Trophy ReadTrophy(dynamic data) => new Trophy(data.uid, data.id, data.nextgrade, data.counter, data.isdone, data.lastreward, JsonConvert.DeserializeObject<List<long>>(data.timestamps), data.characterid ?? 0, data.accountid ?? 0);
    }
}
