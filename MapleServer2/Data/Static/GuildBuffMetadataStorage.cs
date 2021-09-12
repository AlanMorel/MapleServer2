using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class GuildBuffMetadataStorage
    {
        private static readonly Dictionary<int, GuildBuffMetadata> buffs = new Dictionary<int, GuildBuffMetadata>();

        public static void Init()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-guild-buff-metadata");
            List<GuildBuffMetadata> items = Serializer.Deserialize<List<GuildBuffMetadata>>(stream);
            foreach (GuildBuffMetadata item in items)
            {
                buffs[item.BuffId] = item;
            }
        }

        public static bool IsValid(int buffId)
        {
            return buffs.ContainsKey(buffId);
        }

        public static List<int> GetBuffList()
        {
            List<int> buffIds = new List<int>();
            foreach (GuildBuffMetadata buffmetadata in buffs.Values)
            {
                buffIds.Add(buffmetadata.BuffId);
            }
            return buffIds;
        }

        public static GuildBuffLevel GetGuildBuffLevelData(int buffId, int buffLevel)
        {
            GuildBuffMetadata metadata = buffs.GetValueOrDefault(buffId);
            return metadata.Levels.FirstOrDefault(x => x.Level == buffLevel);
        }
    }
}
