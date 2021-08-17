using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class ItemOptionStaticMetadataStorage
    {
        private static readonly Dictionary<int, ItemOptionStaticMetadata> map = new Dictionary<int, ItemOptionStaticMetadata>();

        static ItemOptionStaticMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-item-option-static-metadata");
            List<ItemOptionStaticMetadata> items = Serializer.Deserialize<List<ItemOptionStaticMetadata>>(stream);
            foreach (ItemOptionStaticMetadata item in items)
            {
                map[item.Id] = item;
            }
        }

        public static bool IsValid(int id)
        {
            return map.ContainsKey(id);
        }

        public static ItemOptionsStatic GetMetadata(int id, int rarity)
        {
            ItemOptionStaticMetadata metadata = map.Values.FirstOrDefault(x => x.Id == id);
            if (metadata == null)
            {
                return null;
            }
            return metadata.ItemOptions.FirstOrDefault(x => x.Rarity == rarity);
        }
    }
}
