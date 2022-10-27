using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabasePrestiges : DatabaseTable
{
    public DatabasePrestiges() : base("prestiges") { }

    public long Insert(Prestige prestige)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            level = prestige.Level,
            exp = prestige.Exp,
            rewards_claimed = JsonConvert.SerializeObject(prestige.RewardsClaimed),
            missions = JsonConvert.SerializeObject(prestige.Missions)
        });
    }

    public void Update(Prestige prestige)
    {
        QueryFactory.Query(TableName).Where("id", prestige.Id).Update(new
        {
            level = prestige.Level,
            exp = prestige.Exp,
            rewards_claimed = JsonConvert.SerializeObject(prestige.RewardsClaimed),
            missions = JsonConvert.SerializeObject(prestige.Missions)
        });
    }
    
    public Prestige FindById(long id)
    {
        return ReadPrestige(QueryFactory.Query(TableName).Where("id", id).FirstOrDefault());
    }

    public bool Delete(long id)
    {
        return QueryFactory.Query(TableName).Where("id", id).Delete() == 1;
    }

    private static Prestige ReadPrestige(dynamic data)
    {
        return new(
            data.id, 
            data.level, 
            data.exp, 
            JsonConvert.DeserializeObject<List<int>>(data.rewards_claimed), 
            JsonConvert.DeserializeObject<List<PrestigeMission>>(data.missions));
    }
}
