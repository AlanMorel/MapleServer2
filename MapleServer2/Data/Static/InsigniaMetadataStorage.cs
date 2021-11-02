using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class InsigniaMetadataStorage
    {
        private static readonly Dictionary<int, InsigniaMetadata> map = new Dictionary<int, InsigniaMetadata>();

        public static void Init()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-insignia-metadata");
            List<InsigniaMetadata> items = Serializer.Deserialize<List<InsigniaMetadata>>(stream);
            foreach (InsigniaMetadata item in items)
            {
                map[item.InsigniaId] = item;
            }
        }

        public static bool IsValid(int insigniaId)
        {
            return map.ContainsKey(insigniaId);
        }

        public static InsigniaMetadata GetMetadata(int insigniaId)
        {
            return map.GetValueOrDefault(insigniaId);
        }

        public static int GetTitleId(int insigniaId)
        {
            return map.GetValueOrDefault(insigniaId).TitleId;
        }

        public static string GetConditionType(int insigniaId)
        {
            return map.GetValueOrDefault(insigniaId).ConditionType;
        }
    }
}
