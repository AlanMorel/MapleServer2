using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class ExpMetadataStorage
    {
        private static readonly Dictionary<int, ExpMetadata> map = new Dictionary<int, ExpMetadata>();

        static ExpMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-exp-metadata");
            List<ExpMetadata> items = Serializer.Deserialize<List<ExpMetadata>>(stream);
            foreach (ExpMetadata item in items)
            {
                map[item.Level] = item;
            }
        }

        public static ExpMetadata GetMetadata(short level)
        {
            return map.GetValueOrDefault(level);
        }

        public static bool LevelExist(short level)
        {
            return map.ContainsKey(level);
        }

        public static long GetExpToLevel(short level)
        {
            return LevelExist(level) ? map.GetValueOrDefault(level).Experience : 0;
        }
    }
}
