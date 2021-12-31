using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class SurvivalGoldPassRewardMetadataStorage
{
    private static readonly Dictionary<int, SurvivalGoldPassRewardMetadata> SurvivalGoldPassRewardMetadatas = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-survival-gold-pass-reward-metadata");
        List<SurvivalGoldPassRewardMetadata> items = Serializer.Deserialize<List<SurvivalGoldPassRewardMetadata>>(stream);
        foreach (SurvivalGoldPassRewardMetadata item in items)
        {
            SurvivalGoldPassRewardMetadatas[item.Level] = item;
        }
    }

    public static List<SurvivalGoldPassRewardMetadata> GetMetadatas(int royaleLevel, int goldLevelClaimedRewards)
    {
        return SurvivalGoldPassRewardMetadatas.Values.Where(x => x.Level > goldLevelClaimedRewards && x.Level <= royaleLevel).ToList();
    }
}
