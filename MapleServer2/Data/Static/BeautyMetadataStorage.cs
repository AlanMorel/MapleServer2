using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class BeautyMetadataStorage
    {
        private static readonly Dictionary<int, BeautyMetadata> shops = new Dictionary<int, BeautyMetadata>();

        public static void Init()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-beauty-metadata");
            List<BeautyMetadata> shopList = Serializer.Deserialize<List<BeautyMetadata>>(stream);
            foreach (BeautyMetadata shop in shopList)
            {
                shops[shop.ShopId] = shop;
            }
        }

        public static List<int> GetShopIds()
        {
            return new List<int>(shops.Keys);
        }

        public static BeautyMetadata GetShopById(int shopId)
        {
            return shops.GetValueOrDefault(shopId);
        }

        public static List<BeautyItem> GetItems(int shopId)
        {
            return shops.GetValueOrDefault(shopId)?.Items;
        }

        public static List<BeautyItem> GetGenderItems(int shopId, byte gender)
        {
            BeautyMetadata targetShop = shops.GetValueOrDefault(shopId);
            return targetShop.Items.Where(x => (x.Gender == gender) || (x.Gender == 2)).OrderByDescending(x => x.Flag).ToList();
        }
    }
}
