using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Data
{
    // Class for retrieving and storing shop data
    public static class ShopStorage
    {
        public static readonly Dictionary<int, NpcShop> Shops = new();
        public static readonly Dictionary<int, List<NpcShopItem>> ShopItems = new();

        static ShopStorage()
        {
            ShopItems.Add(103, new List<NpcShopItem>
            {
                new NpcShopItem
                {
                    UniqueId = 400000,
                    ItemId = 30000272,
                    TokenType = CurrencyType.Meso,
                    Price = 500,
                    SalePrice = 1000,
                    ItemRank = 1,
                    Quantity = 1,
                    StockCount = 0,
                    StockPurchased = 0,
                    Category = "ETC",
                    RequiredItemId = 0,
                    Flag = ShopItemFlag.None
                },
                new NpcShopItem
                {
                    UniqueId = 400001,
                    ItemId = 30000272,
                    TokenType = CurrencyType.Bank,
                    Price = 500,
                    SalePrice = 1000,
                    ItemRank = 1,
                    Quantity = 1,
                    StockCount = 0,
                    StockPurchased = 0,
                    Category = "ETC",
                    RequiredItemId = 0,
                    Flag = ShopItemFlag.New
                },
                new NpcShopItem
                {
                    UniqueId = 400002,
                    ItemId = 30000272,
                    TokenType = CurrencyType.Capsule,
                    Price = 500,
                    SalePrice = 1000,
                    ItemRank = 1,
                    Quantity = 1,
                    StockCount = 0,
                    StockPurchased = 0,
                    Category = "ETC",
                    RequiredItemId = 0,
                    Flag = ShopItemFlag.Event
                },
                new NpcShopItem
                {
                    UniqueId = 400003,
                    ItemId = 30000272,
                    TokenType = CurrencyType.Meret,
                    Price = 500,
                    SalePrice = 1000,
                    ItemRank = 1,
                    Quantity = 1,
                    StockCount = 0,
                    StockPurchased = 0,
                    Category = "ETC",
                    RequiredItemId = 0,
                    Flag = ShopItemFlag.Special
                },
                new NpcShopItem
                {
                    UniqueId = 400004,
                    ItemId = 30000272,
                    TokenType = CurrencyType.Rue,
                    Price = 500,
                    SalePrice = 1000,
                    ItemRank = 1,
                    Quantity = 1,
                    StockCount = 0,
                    StockPurchased = 0,
                    Category = "ETC",
                    RequiredItemId = 0,
                    Flag = ShopItemFlag.HalfPrice
                }
            });
            
            Shops.Add(103, new NpcShop
            {
                TemplateId = 11000079,
                Id = 103,
                Category = 3,
                Name = "shop",
                RestrictSales = false,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = GetItems(103)
            });

        }

        // Retrieves a specific item from a shop
        public static NpcShopItem GetItem(int itemUid)
        {
           List<NpcShopItem> shopItems = ShopItems.SelectMany(x => x.Value).ToList();
           NpcShopItem item = shopItems.FirstOrDefault(x => x.UniqueId == itemUid);
           return item;
        }

        // Retrieves list of items from a shop
        public static List<NpcShopItem> GetItems(int shopId)
        {
            return ShopItems.GetValueOrDefault(shopId);
        }

        // Retrieves a specific shop
        public static NpcShop GetShop(int shopId)
        {
            return Shops.GetValueOrDefault(shopId);
        }
        
        // Retrieves a shop by a given itemUid
        public static NpcShop GetShopByItemUid(int itemUid)
        {
            List<NpcShop> shops = Shops.Values.ToList();
            NpcShop shop = shops.FirstOrDefault(x => x.Items.Exists(x => x.UniqueId == itemUid));
            return shop;
        }
        
        // Adds new shop
        public static void AddShop(int id, NpcShop data)
        {
            Shops.Add(id, data);
        }

        // Updates a shop
        public static void UpdateShop(NpcShop data)
        {
            Shops.Remove(data.Id);
            Shops.Add(data.Id, data);
        }
    }
}
