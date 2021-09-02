using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class FurnishingShopMetadataStorage
    {
        private static readonly Dictionary<int, FurnishingShopMetadata> map = new Dictionary<int, FurnishingShopMetadata>();

        public static void Init()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-furnishing-shop-metadata");
            List<FurnishingShopMetadata> items = Serializer.Deserialize<List<FurnishingShopMetadata>>(stream);
            foreach (FurnishingShopMetadata item in items)
            {
                map[item.ItemId] = item;
            }
        }

        public static bool IsValid(int itemId)
        {
            return map.ContainsKey(itemId);
        }

        public static FurnishingShopMetadata GetMetadata(int itemId)
        {
            return map.GetValueOrDefault(itemId);
        }
    }
}
