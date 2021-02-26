using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class ShopMetadataStorage
    {
        private static readonly Dictionary<int, ShopMetadata> shops = new Dictionary<int, ShopMetadata>();

        static ShopMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-shop-metadata");
            List<ShopMetadata> shopList = Serializer.Deserialize<List<ShopMetadata>>(stream);
            foreach (ShopMetadata shop in shopList)
            {
                shops[shop.Id] = shop;
            }
        }

        public static List<int> GetShopIds()
        {
            return new List<int>(shops.Keys);
        }

        public static ShopMetadata GetShop(int shopId)
        {
            return shops.GetValueOrDefault(shopId);
        }

        public static List<ShopItem> GetItems(int shopId)
        {
            return shops.GetValueOrDefault(shopId)?.Items;
        }

        public static ShopItem GetItem(int itemUid)
        {
            List<ShopItem> shopItems = shops.Values.SelectMany(x => x.Items).ToList();
            ShopItem item = shopItems.FirstOrDefault(x => x.UniqueId == itemUid);
            return item;
        }

        public static ShopMetadata GetShopByItemUid(int itemUid)
        {
            List<ShopMetadata> shopList = shops.Values.ToList();
            ShopMetadata shop = shopList.FirstOrDefault(x => x.Items.Exists(z => z.UniqueId == itemUid));
            return shop;
        }

    }
}
