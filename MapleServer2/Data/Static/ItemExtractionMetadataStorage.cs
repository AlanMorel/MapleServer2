using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class ItemExtractionMetadataStorage
    {
        private static readonly Dictionary<int, ItemExtractionMetadata> map = new Dictionary<int, ItemExtractionMetadata>();

        public static void Init()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-item-extraction-metadata");
            List<ItemExtractionMetadata> items = Serializer.Deserialize<List<ItemExtractionMetadata>>(stream);
            foreach (ItemExtractionMetadata item in items)
            {
                map[item.SourceItemId] = item;
            }
        }

        public static bool IsValid(int itemId)
        {
            return map.ContainsKey(itemId);
        }

        public static ItemExtractionMetadata GetMetadata(int itemId)
        {
            return map.GetValueOrDefault(itemId);
        }

        public static byte GetExtractionCount(int itemId)
        {
            ItemExtractionMetadata metadata = map.GetValueOrDefault(itemId);
            if (metadata == null)
            {
                return 0;
            }
            return map.GetValueOrDefault(itemId).TryCount;
        }
    }
}
