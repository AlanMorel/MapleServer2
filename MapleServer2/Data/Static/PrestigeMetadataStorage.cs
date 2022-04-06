using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class PrestigeMetadataStorage
{
    private static readonly Dictionary<int, PrestigeReward> Rewards = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.Prestige}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        PrestigeMetadata metadata = Serializer.Deserialize<PrestigeMetadata>(stream);
        foreach (PrestigeReward reward in metadata.Rewards)
        {
            Rewards.Add(reward.Level, reward);
        }
    }

    public static PrestigeReward GetReward(int level)
    {
        return Rewards.GetValueOrDefault(level);
    }
}
