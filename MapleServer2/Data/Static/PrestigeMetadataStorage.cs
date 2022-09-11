using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class PrestigeMetadataStorage
{
    private static readonly Dictionary<int, PrestigeReward> Rewards = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.Prestige);
        PrestigeMetadata metadata = Serializer.Deserialize<PrestigeMetadata>(stream);
        foreach (PrestigeReward reward in metadata.Rewards)
        {
            Rewards.Add(reward.Level, reward);
        }
    }

    public static PrestigeReward? GetReward(int level)
    {
        return Rewards.GetValueOrDefault(level);
    }
}
