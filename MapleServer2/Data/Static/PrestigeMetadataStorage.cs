using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class PrestigeMetadataStorage
    {
        private static readonly Dictionary<int, PrestigeReward> rewards = new Dictionary<int, PrestigeReward>();

        public static void Init()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-prestige-metadata");
            PrestigeMetadata metadata = Serializer.Deserialize<PrestigeMetadata>(stream);
            foreach (PrestigeReward reward in metadata.Rewards)
            {
                rewards.Add(reward.Level, reward);
            }
        }

        public static PrestigeReward GetReward(int level)
        {
            return rewards.GetValueOrDefault(level);
        }
    }
}
