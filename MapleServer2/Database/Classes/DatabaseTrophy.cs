using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseTrophy : DatabaseTable
{
    public DatabaseTrophy() : base("trophies") { }

    public long Insert(Trophy trophy)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            trophy.Id,
            next_grade = trophy.NextGrade,
            trophy.Counter,
            is_done = trophy.IsDone,
            last_reward = trophy.LastReward,
            timestamps = JsonConvert.SerializeObject(trophy.Timestamps),
            character_id = trophy.CharacterId == 0 ? null : (long?) trophy.CharacterId,
            account_id = trophy.AccountId == 0 ? null : (long?) trophy.AccountId
        });
    }

    public Dictionary<int, Trophy> FindAllByCharacterId(long characterId)
    {
        IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("character_id", characterId).Get();
        Dictionary<int, Trophy> trophies = new();
        foreach (dynamic data in results)
        {
            Trophy trophy = (Trophy) ReadTrophy(data);
            trophies.Add(trophy.Id, trophy);
        }
        return trophies;
    }

    public Dictionary<int, Trophy> FindAllByAccountId(long accountId)
    {
        IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("account_id", accountId).Get();
        Dictionary<int, Trophy> trophies = new();
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
            next_grade = trophy.NextGrade,
            trophy.Counter,
            is_done = trophy.IsDone,
            last_reward = trophy.LastReward,
            timestamps = JsonConvert.SerializeObject(trophy.Timestamps),
            character_id = trophy.CharacterId == 0 ? null : (long?) trophy.CharacterId,
            account_id = trophy.AccountId == 0 ? null : (long?) trophy.AccountId
        });
    }

    public bool Delete(long uid)
    {
        return QueryFactory.Query(TableName).Where("uid", uid).Delete() == 1;
    }

    private static Trophy ReadTrophy(dynamic data)
    {
        return new Trophy(data.uid, data.id, data.next_grade, data.counter, data.is_done, data.last_reward, JsonConvert.DeserializeObject<List<long>>(data.timestamps), data.character_id ?? 0, data.account_id ?? 0);
    }
}
