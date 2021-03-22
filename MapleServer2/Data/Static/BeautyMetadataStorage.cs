using System.Collections.Generic;
using System.IO;
using System.Linq;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class BeautyMetadataStorage
    {
        private static readonly Dictionary<int, BeautyMetadata> shops = new Dictionary<int, BeautyMetadata>();

        static BeautyMetadataStorage()
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

        public static BeautyMetadata GetCosmeticShopByItemId(int itemId)
        {
            List<BeautyMetadata> shopList = shops.Values.ToList();
            return shopList.FirstOrDefault(x => x.Items.Exists(z => z.ItemId == itemId) && x.BeautyCategory == BeautyCategory.Standard);
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
