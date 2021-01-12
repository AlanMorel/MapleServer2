using System.Collections.Generic;
using System.IO;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class PrestigeMetadataStorage
    {
        private static readonly Dictionary<int, PrestigeReward> rewards = new Dictionary<int, PrestigeReward>();

        static PrestigeMetadataStorage()
        {
            using FileStream stream = File.OpenRead("Maple2Storage/Resources/ms2-prestige-metadata");
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
