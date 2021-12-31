using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseMushkingRoyaleStats : DatabaseTable
{
    public DatabaseMushkingRoyaleStats() : base("mushking_royale_stats") { }

    public long Insert(MushkingRoyaleStats stats)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            id = stats.Id,
            exp = stats.Exp,
            royale_level = stats.Level,
            silver_level_claimed_rewards = stats.SilverLevelClaimedRewards,
            gold_level_claimed_rewards = stats.GoldLevelClaimedRewards,
            active_gold_pass = stats.IsGoldPassActive
        });
    }

    public void Update(MushkingRoyaleStats stats)
    {
        QueryFactory.Query(TableName).Where("id", stats.Id).Update(new
        {
            exp = stats.Exp,
            royale_level = stats.Level,
            silver_level_claimed_rewards = stats.SilverLevelClaimedRewards,
            gold_level_claimed_rewards = stats.GoldLevelClaimedRewards,
            active_gold_pass = stats.IsGoldPassActive
        });
    }

    public MushkingRoyaleStats FindById(long id)
    {
        return ReadMushkingRoyaleStats(QueryFactory.Query(TableName).Where("id", id).FirstOrDefault());
    }

    public bool Delete(long uid)
    {
        return QueryFactory.Query(TableName).Where("id", uid).Delete() == 1;
    }

    private static MushkingRoyaleStats ReadMushkingRoyaleStats(dynamic data)
    {
        return new MushkingRoyaleStats(
            data.id,
            data.royale_level,
            data.exp,
            data.silver_level_claimed_rewards,
            data.gold_level_claimed_rewards,
            data.active_gold_pass
            );
    }
}
