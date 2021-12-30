using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class SurvivalSilverPassRewardMetadataStorage
{
    private static readonly Dictionary<int, SurvivalSilverPassRewardMetadata> SurvivalSilverPassRewardMetadatas = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-survival-silver-pass-reward-metadata");
        List<SurvivalSilverPassRewardMetadata> items = Serializer.Deserialize<List<SurvivalSilverPassRewardMetadata>>(stream);
        foreach (SurvivalSilverPassRewardMetadata item in items)
        {
            SurvivalSilverPassRewardMetadatas[item.Level] = item;
        }
    }

    public static List<SurvivalSilverPassRewardMetadata> GetMetadatas(int royaleLevel, int silverLevelClaimedRewards)
    {
        return SurvivalSilverPassRewardMetadatas.Values.Where(x => x.Level > silverLevelClaimedRewards && x.Level <= royaleLevel).ToList();
    }
}
