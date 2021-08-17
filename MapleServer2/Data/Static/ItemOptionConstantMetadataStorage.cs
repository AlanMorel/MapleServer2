using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class ItemOptionConstantMetadataStorage
    {
        private static readonly Dictionary<int, ItemOptionConstantMetadata> map = new Dictionary<int, ItemOptionConstantMetadata>();

        static ItemOptionConstantMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-item-option-constant-metadata");
            List<ItemOptionConstantMetadata> items = Serializer.Deserialize<List<ItemOptionConstantMetadata>>(stream);
            foreach (ItemOptionConstantMetadata item in items)
            {
                map[item.Id] = item;
            }
        }

        public static bool IsValid(int id)
        {
            return map.ContainsKey(id);
        }

        public static ItemOptionsConstant GetMetadata(int id, int rarity)
        {
            ItemOptionConstantMetadata metadata = map.Values.FirstOrDefault(x => x.Id == id);
            if (metadata == null)
            {
                return null;
            }
            return metadata.ItemOptions.FirstOrDefault(x => x.Rarity == rarity);
        }
    }
}
