using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class PrestigeRewardMetadataStorage
{
    private static readonly Dictionary<int, PrestigeRewardMetadata> Rewards = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.PrestigeReward);
        List<PrestigeRewardMetadata> rewardMetadata = Serializer.Deserialize<List<PrestigeRewardMetadata>>(stream);
        foreach (PrestigeRewardMetadata reward in rewardMetadata)
        {
            Rewards.Add(reward.Level, reward);
        }
    }

    public static PrestigeRewardMetadata? GetReward(int level)
    {
        return Rewards.GetValueOrDefault(level);
    }

    public static int GetTotalStatPoints(int level)
    {
        int count = 0;
        foreach (PrestigeRewardMetadata reward in Rewards.Values)
        {
            if (reward.Level <= level && reward.Type == "statPoint")
            {
                count += reward.Amount;
            }
        }

        return count;
    }
}
