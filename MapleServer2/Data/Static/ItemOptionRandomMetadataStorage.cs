using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class ItemOptionRandomMetadataStorage
    {
        private static readonly Dictionary<int, ItemOptionRandomMetadata> map = new Dictionary<int, ItemOptionRandomMetadata>();

        public static void Init()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-item-option-random-metadata");
            List<ItemOptionRandomMetadata> items = Serializer.Deserialize<List<ItemOptionRandomMetadata>>(stream);
            foreach (ItemOptionRandomMetadata item in items)
            {
                map[item.Id] = item;
            }
        }

        public static bool IsValid(int id)
        {
            return map.ContainsKey(id);
        }

        public static ItemOptionRandom GetMetadata(int id, int rarity)
        {
            ItemOptionRandomMetadata metadata = map.Values.FirstOrDefault(x => x.Id == id);
            if (metadata == null)
            {
                return null;
            }
            return metadata.ItemOptions.FirstOrDefault(x => x.Rarity == rarity);
        }
    }
}
