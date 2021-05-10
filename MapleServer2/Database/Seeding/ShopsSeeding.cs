using System;
using System.Collections.Generic;
using Maple2Storage.Enums;
using MapleServer2.Database.Types;

namespace MapleServer2.Database
{
    public static class ShopsSeeding
    {
        public static void Seed()
        {
            List<Shop> shops = new List<Shop>();

            /*
             * Shop Name: Rumi
             * Map: Lith Harbor
             */
            Shop rumi = new Shop()
            {
                TemplateId = 11000079,
                Id = 103,
                Category = 3,
                Name = "shop",
                RestrictSales = false,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>
                {
                    new ShopItem
                    {
                        ItemId = 63000000,
                        TokenType = ShopCurrencyType.Meso,
                        Price = 1000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "PS",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        ItemId = 40400020,
                        TokenType = ShopCurrencyType.Meso,
                        Price = 50000,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Flight",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        ItemId = 40100039,
                        TokenType = ShopCurrencyType.Meso,
                        Price = 1000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "SKC",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        ItemId = 20301053,
                        TokenType = ShopCurrencyType.Meso,
                        Price = 50000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "ETC",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        ItemId = 20301054,
                        TokenType = ShopCurrencyType.Meso,
                        Price = 50000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "ETC",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        ItemId = 20301057,
                        TokenType = ShopCurrencyType.Meso,
                        Price = 50000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "ETC",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        ItemId = 20000022,
                        TokenType = ShopCurrencyType.Meso,
                        Price = 750,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "PS",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        ItemId = 20000023,
                        TokenType = ShopCurrencyType.Meso,
                        Price = 2480,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "PS",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        ItemId = 20000024,
                        TokenType = ShopCurrencyType.Meso,
                        Price = 4280,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "PS",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        ItemId = 20000028,
                        TokenType = ShopCurrencyType.Meso,
                        Price = 210,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "PS",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        ItemId = 20000029,
                        TokenType = ShopCurrencyType.Meso,
                        Price = 980,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "PS",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        ItemId = 20000030,
                        TokenType = ShopCurrencyType.Meso,
                        Price = 2360,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "PS",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        ItemId = 20000031,
                        TokenType = ShopCurrencyType.Meso,
                        Price = 4160,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "PS",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        ItemId = 20000032,
                        TokenType = ShopCurrencyType.Meso,
                        Price = 5180,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "PS",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        ItemId = 20000033,
                        TokenType = ShopCurrencyType.Meso,
                        Price = 5670,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "PS",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    }
                }
            };

            shops.Add(rumi);

            /*
             * Shop Name: Lazy (Habi Shop)
             * Map: Queenstown
             */
            Shop lazy = new Shop()
            {
                TemplateId = 11003465,
                Id = 152,
                Category = 17,
                Name = "habishop",
                RestrictSales = false,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>
                {
                    new ShopItem
                    {
                        ItemId = 40100023,
                        TokenType = ShopCurrencyType.HaviFruit,
                        RequiredItemId = 0,
                        Price = 3000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "HabiEtc",
                        Quantity = 4000,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 50600188,
                        TokenType = ShopCurrencyType.HaviFruit,
                        RequiredItemId = 0,
                        Price = 8000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "HabiMount",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 31000145,
                        TokenType = ShopCurrencyType.HaviFruit,
                        RequiredItemId = 0,
                        Price = 200,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "HabiEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 31000146,
                        TokenType = ShopCurrencyType.HaviFruit,
                        RequiredItemId = 0,
                        Price = 300,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "HabiEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20000521,
                        TokenType = ShopCurrencyType.HaviFruit,
                        RequiredItemId = 0,
                        Price = 100,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "HabiEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20000522,
                        TokenType = ShopCurrencyType.HaviFruit,
                        RequiredItemId = 0,
                        Price = 100,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "HabiEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20000523,
                        TokenType = ShopCurrencyType.HaviFruit,
                        RequiredItemId = 0,
                        Price = 100,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "HabiEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20000524,
                        TokenType = ShopCurrencyType.HaviFruit,
                        RequiredItemId = 0,
                        Price = 100,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "HabiEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20000525,
                        TokenType = ShopCurrencyType.HaviFruit,
                        RequiredItemId = 0,
                        Price = 100,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "HabiEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20000597,
                        TokenType = ShopCurrencyType.HaviFruit,
                        RequiredItemId = 0,
                        Price = 5000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "HabiEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 11600806,
                        TokenType = ShopCurrencyType.HaviFruit,
                        RequiredItemId = 0,
                        Price = 800,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "HabiEquip",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 11600807,
                        TokenType = ShopCurrencyType.HaviFruit,
                        RequiredItemId = 0,
                        Price = 800,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "HabiEquip",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 11600808,
                        TokenType = ShopCurrencyType.HaviFruit,
                        RequiredItemId = 0,
                        Price = 800,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "HabiEquip",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 11600809,
                        TokenType = ShopCurrencyType.HaviFruit,
                        RequiredItemId = 0,
                        Price = 800,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "HabiEquip",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 11600810,
                        TokenType = ShopCurrencyType.HaviFruit,
                        RequiredItemId = 0,
                        Price = 800,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "HabiEquip",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 11600801,
                        TokenType = ShopCurrencyType.HaviFruit,
                        RequiredItemId = 0,
                        Price = 6000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "HabiEquip",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 11600802,
                        TokenType = ShopCurrencyType.HaviFruit,
                        RequiredItemId = 0,
                        Price = 6000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "HabiEquip",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 11600803,
                        TokenType = ShopCurrencyType.HaviFruit,
                        RequiredItemId = 0,
                        Price = 6000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "HabiEquip",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 11600804,
                        TokenType = ShopCurrencyType.HaviFruit,
                        RequiredItemId = 0,
                        Price = 6000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "HabiEquip",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 11600805,
                        TokenType = ShopCurrencyType.HaviFruit,
                        RequiredItemId = 0,
                        Price = 6000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "HabiEquip",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 11300423,
                        TokenType = ShopCurrencyType.HaviFruit,
                        RequiredItemId = 0,
                        Price = 7000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "HabiSkin",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 50600115,
                        TokenType = ShopCurrencyType.HaviFruit,
                        RequiredItemId = 0,
                        Price = 6000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "HabiMount",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 11304766,
                        TokenType = ShopCurrencyType.HaviFruit,
                        RequiredItemId = 0,
                        Price = 12000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "HabiSkin",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    }
                }
            };

            shops.Add(lazy);

            /*
             * Shop Name: Red Star Shop
             * Map: Queenstown
             */
            Shop redstarshop = new Shop()
            {
                TemplateId = 11001272,
                Id = 154,
                Category = 19,
                Name = "redstarshop",
                RestrictSales = false,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>
                {
                    new ShopItem
                    {
                        ItemId = 50600117,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000448,
                        Price = 3600,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "CoinMount",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 50600221,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000448,
                        Price = 3600,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "CoinMount",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20300227,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000448,
                        Price = 15,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "CoinEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20000547,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000448,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20000548,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000448,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20000549,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000448,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20000550,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000448,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20000552,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000448,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20000516,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000448,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinEtc",
                        Quantity = 10,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20000518,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000448,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinEtc",
                        Quantity = 10,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20000519,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000448,
                        Price = 75,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinEtc",
                        Quantity = 10,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20000520,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000448,
                        Price = 75,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinEtc",
                        Quantity = 10,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 50200865,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000448,
                        Price = 3,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinConstruct",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 50200866,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000448,
                        Price = 5,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinConstruct",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 50200867,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000448,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinConstruct",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 50200868,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000448,
                        Price = 1,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinConstruct",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 50200869,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000448,
                        Price = 1,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinConstruct",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 50400251,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000448,
                        Price = 5,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinConstruct",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 50400252,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000448,
                        Price = 17,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinConstruct",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 50400253,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000448,
                        Price = 17,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinConstruct",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 50400254,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000448,
                        Price = 2,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinConstruct",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    }
                }
            };

            shops.Add(redstarshop);

            /*
             * Shop Name: Blue Star Shop
             * Map: Queenstown
             */
            Shop bluestarshop = new Shop()
            {
                TemplateId = 11001270,
                Id = 153,
                Category = 19,
                Name = "bluestarshop",
                RestrictSales = false,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>
                {
                    new ShopItem
                    {
                        ItemId = 50600116,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000447,
                        Price = 3200,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "CoinMount",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 50600222,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000447,
                        Price = 3200,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "CoinMount",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20300227,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000447,
                        Price = 25,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "CoinEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20000561,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000447,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20000562,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000447,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20000563,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000447,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20000564,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000447,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20000512,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000447,
                        Price = 100,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinEtc",
                        Quantity = 100,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20000514,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000447,
                        Price = 100,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinEtc",
                        Quantity = 100,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 50200870,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000447,
                        Price = 12,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinConstruct",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 50200871,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000447,
                        Price = 5,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinConstruct",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 50200872,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000447,
                        Price = 3,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinConstruct",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 50200873,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000447,
                        Price = 3,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinConstruct",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 50200874,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000447,
                        Price = 17,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinConstruct",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 50200875,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000447,
                        Price = 12,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinConstruct",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 50200876,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000447,
                        Price = 17,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinConstruct",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 50200877,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000447,
                        Price = 3,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinConstruct",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 50400255,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000447,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinConstruct",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 50400256,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 30000447,
                        Price = 26,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "CoinConstruct",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    }
                }
            };

            shops.Add(bluestarshop);

            /*
             * Shop Name: Event Guide Bobby (event shop)
             * Map: Queenstown
             */
            Shop eventshop = new Shop()
            {
                TemplateId = 11001003,
                Id = 254,
                Category = 3,
                Name = "eventshop",
                RestrictSales = false,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
            };

            // shops.Add(eventshop);

            /*
             * Shop Name: Aricari Daily Quest (chest)
             * Map: Queenstown
             */
            Shop lushop = new Shop()
            {
                TemplateId = 11004454,
                Id = 151,
                Category = 3,
                Name = "lushop",
                RestrictSales = false,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
            };

            // shops.Add(lushop);

            /*
             * Shop Name: Beros (Battle Sim Operator)
             * Map: Queenstown
             */
            Shop beros = new Shop()
            {
                TemplateId = 11000122,
                Id = 177,
                Category = 3,
                Name = "chaosashop",
                RestrictSales = false,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
            };

            // shops.Add(beros);

            /*
             * Shop Name: Borka (Shadow Supply Shop)
             * Map: Queenstown
             */
            Shop borka = new Shop()
            {
                TemplateId = 11000748,
                Id = 135,
                Category = 3,
                Name = "karmashop",
                RestrictSales = false,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 40100024,
                        TokenType = ShopCurrencyType.Treva,
                        RequiredItemId = 0,
                        Price = 600,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "FUN",
                        Quantity = 25,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 34000063,
                        TokenType = ShopCurrencyType.Treva,
                        RequiredItemId = 0,
                        Price = 35000,
                        SalePrice = 0,
                        ItemRank = 2,
                        Category = "FUN",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 34000064,
                        TokenType = ShopCurrencyType.Treva,
                        RequiredItemId = 0,
                        Price = 35000,
                        SalePrice = 0,
                        ItemRank = 2,
                        Category = "FUN",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 34000065,
                        TokenType = ShopCurrencyType.Treva,
                        RequiredItemId = 0,
                        Price = 35000,
                        SalePrice = 0,
                        ItemRank = 2,
                        Category = "FUN",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 34000066,
                        TokenType = ShopCurrencyType.Treva,
                        RequiredItemId = 0,
                        Price = 35000,
                        SalePrice = 0,
                        ItemRank = 2,
                        Category = "FUN",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 40300001,
                        TokenType = ShopCurrencyType.Treva,
                        RequiredItemId = 0,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "FUN",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 40300002,
                        TokenType = ShopCurrencyType.Treva,
                        RequiredItemId = 0,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "FUN",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 40300005,
                        TokenType = ShopCurrencyType.Treva,
                        RequiredItemId = 0,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "FUN",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 40300006,
                        TokenType = ShopCurrencyType.Treva,
                        RequiredItemId = 0,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "FUN",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 40220141,
                        TokenType = ShopCurrencyType.Treva,
                        RequiredItemId = 0,
                        Price = 3000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "FUN",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 40220151,
                        TokenType = ShopCurrencyType.Treva,
                        RequiredItemId = 0,
                        Price = 3000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "FUN",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 40220161,
                        TokenType = ShopCurrencyType.Treva,
                        RequiredItemId = 0,
                        Price = 3000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "FUN",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 40220131,
                        TokenType = ShopCurrencyType.Treva,
                        RequiredItemId = 0,
                        Price = 3000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "FUN",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20000080,
                        TokenType = ShopCurrencyType.Treva,
                        RequiredItemId = 0,
                        Price = 30,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "PS",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 20000081,
                        TokenType = ShopCurrencyType.Treva,
                        RequiredItemId = 0,
                        Price = 30,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "PS",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    }
                }
            };

            shops.Add(borka);

            /*
             * Shop Name: hibeck (Shadow Equipment Shop)
             * Map: Queenstown
             */
            Shop hibeck = new Shop()
            {
                TemplateId = 11000750,
                Id = 136,
                Category = 3,
                Name = "karmaequip",
                RestrictSales = false,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
            };

            // shops.Add(hibeck);

            /*
             * Shop Name: Matz (Shadow Mount Shop)
             * Map: Queenstown
             */
            Shop matz = new Shop()
            {
                TemplateId = 11000122,
                Id = 142,
                Category = 3,
                Name = "karmaride",
                RestrictSales = false,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
            };

            // shops.Add(matz);

            /*
             * Shop Name: Rainbow Arc (Guild Token Shop)
             * Map: Queenstown
             */
            Shop rainbowArc = new Shop()
            {
                TemplateId = 11003463,
                Id = 168,
                Category = 28,
                Name = "guildtokenetc",
                RestrictSales = true,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 50600159,
                        TokenType = ShopCurrencyType.ValorToken,
                        RequiredItemId = 0,
                        Price = 2000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPRide",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 11800085,
                        TokenType = ShopCurrencyType.ValorToken,
                        ItemRank = 4,
                        Price = 800,
                        SalePrice = 0,
                        RequiredItemId = 0,
                        Category = "PVPAcc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 12100072,
                        TokenType = ShopCurrencyType.ValorToken,
                        RequiredItemId = 0,
                        Price = 800,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPAcc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                }
            };

            shops.Add(rainbowArc);

            /*
             * Shop Name: Spartan (Dungeon Helper Shop)
             * Map: Queenstown
             */
            Shop spartan = new Shop()
            {
                TemplateId = 11003560,
                Id = 196,
                Category = 6,
                Name = "dungeonhelp",
                RestrictSales = false,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>() { }
            };

            // shops.Add(spartan);

            /*
             * Shop Name: Stefan (Music Shop)
             * Map: Tria
             */
            Shop stefan = new Shop()
            {
                TemplateId = 11001320,
                Id = 160,
                Category = 20,
                Name = "musicshop",
                RestrictSales = false,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 11300428,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 4000000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Skin",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 11850034,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 4000000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Skin",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 34000078,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 70000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Instrument",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 34000002,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 70000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Instrument",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 34000082,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Instrument",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 34000083,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Instrument",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 34000084,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Instrument",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 34000005,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 250000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Instrument",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 34000006,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 250000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Instrument",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 34000007,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 250000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Instrument",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35100001,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 20000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35100026,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 40000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35100011,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 50000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35100027,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35100028,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 125000,
                        SalePrice = 0,
                        ItemRank = 2,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35100029,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 250000,
                        SalePrice = 0,
                        ItemRank = 2,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000001,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 6700,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000029,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 8200,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000003,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 9900,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000004,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 9900,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000005,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 7700,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000006,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 5500,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000007,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 4900,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000008,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 9200,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000009,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 8300,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000010,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 8300,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000011,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 4800,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000012,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 5300,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000013,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 8600,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000014,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 6000,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000015,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 6100,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000016,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 6100,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000017,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 9900,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000018,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 8900,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000019,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 6800,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000020,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 6600,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000021,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 18300,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000022,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 18300,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000023,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 18300,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000024,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 18300,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000025,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 18300,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000026,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 3700,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000027,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 3700,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000028,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 13300,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000030,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 1900,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000031,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 1900,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        ItemId = 35000032,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 1900,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Score",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    }
                }
            };

            shops.Add(stefan);

            Shop holidaycapsule2019 = new Shop()
            {
                TemplateId = 11001321,
                Id = 104201,
                Category = 104201,
                Name = "shopetc",
                ShopType = ShopType.Capsule,
                RestrictSales = false,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = false,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 11220353,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 35,
                        SalePrice = 0,
                        ItemRank = 5,
                        Category = "PS",
                        Quantity = 1,
                        Flag = ShopItemFlag.None,
                    }
                }
            };

            shops.Add(holidaycapsule2019);

            Shop duckycapsule1 = new Shop()
            {
                TemplateId = 0,
                Id = 101001,
                Category = 101001,
                Name = "shopetc",
                ShopType = ShopType.Capsule,
                RestrictSales = true,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 11220311,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220312,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320311,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320312,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620311,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620312,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720311,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720312,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820311,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820312,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220311,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220312,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320095,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320096,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620095,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620096,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720095,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720096,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220095,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220096,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11120153,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11120154,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220147,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220152,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320145,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320152,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320147,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320150,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220152,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11420147,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11520147,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720147,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720152,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820147,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820152,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220143,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220144,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    }
                }
            };

            shops.Add(duckycapsule1);

            Shop duckycapsule2 = new Shop()
            {
                TemplateId = 0,
                Id = 101002,
                Category = 101002,
                Name = "shopetc",
                ShopType = ShopType.Capsule,
                RestrictSales = true,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 11220003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11020005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11020006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320097,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320098,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620097,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620098,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720097,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720098,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220097,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220098,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11020313,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11020314,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11120159,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11120160,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320313,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320314,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720157,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720158,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720287,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720288,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820157,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820158,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820313,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820314,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220159,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220160,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220313,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220314,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    }
                },
            };

            shops.Add(duckycapsule2);

            Shop duckycapsule3 = new Shop()
            {
                TemplateId = 0,
                Id = 101003,
                Category = 101003,
                Name = "shopetc",
                ShopType = ShopType.Capsule,
                RestrictSales = true,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 11220007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220009,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220010,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320009,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320010,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620009,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620010,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720009,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720010,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820009,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820010,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220009,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220010,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11120099,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11120100,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320099,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320100,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620099,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620100,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720099,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720100,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220099,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220100,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320161,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320162,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320315,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320316,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720305,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720306,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820161,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820162,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820163,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820164,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220157,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220158,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220301,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220302,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220315,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220316,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                }
            };

            shops.Add(duckycapsule3);

            Shop duckycapsuleseries1 = new Shop()
            {
                TemplateId = 0,
                Id = 101004,
                Category = 101004,
                Name = "shopetc",
                ShopType = ShopType.Capsule,
                RestrictSales = true,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 11220317,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220318,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320317,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320318,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620317,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620318,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720317,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720318,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820317,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820318,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220317,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220318,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220013,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220014,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320013,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320014,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620013,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620014,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720013,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720014,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820013,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820014,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220013,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220014,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220101,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220102,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320101,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320102,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620101,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620102,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720101,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720102,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220101,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220102,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320167,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320168,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620167,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620168,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720167,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720168,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220167,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220168,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320169,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320170,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320172,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320173,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820169,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820170,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                }
            };

            shops.Add(duckycapsuleseries1);

            Shop duckycapsuleseries2 = new Shop()
            {
                TemplateId = 0,
                Id = 101005,
                Category = 101005,
                Name = "shopetc",
                ShopType = ShopType.Capsule,
                RestrictSales = true,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 11220367,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220368,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320367,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320368,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620367,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620368,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720367,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720368,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820367,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820368,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220367,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220368,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220017,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220018,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320017,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320018,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620017,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620018,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720017,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720018,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820017,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820018,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220017,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220018,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320103,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320104,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620103,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620104,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720103,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720104,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820103,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820104,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220103,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220104,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320175,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320176,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320181,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320182,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11420175,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11420176,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11420177,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11420178,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720175,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720176,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820175,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820176,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820181,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820182,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220307,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220308,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                }
            };

            shops.Add(duckycapsuleseries2);

            Shop duckycapsuleseries3 = new Shop()
            {
                TemplateId = 0,
                Id = 101006,
                Category = 101006,
                Name = "shopetc",
                ShopType = ShopType.Capsule,
                RestrictSales = true,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 11220023,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220024,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320023,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320024,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620023,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620024,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720023,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720024,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820023,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820024,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220023,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220024,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220033,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220034,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320033,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320034,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620033,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620034,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720033,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720034,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820033,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820034,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220033,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220034,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },

                    new ShopItem
                    {
                        ItemId = 11220107,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220108,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320107,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320108,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620107,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620108,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720107,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720108,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220107,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220108,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11020193,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11020194,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11020201,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11020202,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11020197,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11020198,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220199,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220200,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320193,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320194,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620201,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620202,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720319,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720320,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220319,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220320,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                }
            };

            shops.Add(duckycapsuleseries3);

            Shop duckycapsuleseries4 = new Shop()
            {
                TemplateId = 0,
                Id = 101007,
                Category = 101007,
                Name = "shopetc",
                ShopType = ShopType.Capsule,
                RestrictSales = true,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 11220031,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220032,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320031,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320032,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620031,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620032,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720031,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720032,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820031,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820032,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220031,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220032,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220041,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220042,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320041,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320042,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620041,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620042,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720041,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720042,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820041,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820042,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220041,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220042,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },

                    new ShopItem
                    {
                        ItemId = 11320109,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320110,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620109,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620110,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720109,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720110,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820109,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820110,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220109,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220110,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320185,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320186,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320187,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320188,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320189,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320190,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320191,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320192,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820185,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820186,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820187,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820188,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820189,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820190,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820191,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820192,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                }
            };

            shops.Add(duckycapsuleseries4);

            Shop duckycapsuleseries5 = new Shop()
            {
                TemplateId = 0,
                Id = 101008,
                Category = 101008,
                Name = "shopetc",
                ShopType = ShopType.Capsule,
                RestrictSales = true,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 11220027,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220028,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320027,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320028,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620027,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620028,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720027,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720028,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820027,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820028,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220027,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220028,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220089,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220090,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320089,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320090,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620089,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620090,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720089,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720090,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820089,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820090,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220089,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220090,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },

                    new ShopItem
                    {
                        ItemId = 11120351,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11120352,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320351,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320352,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620351,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620352,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720351,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720352,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220351,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220352,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11120207,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11120208,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320205,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320206,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820205,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820206,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220193,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220194,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320203,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320204,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620203,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620204,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720203,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720204,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220203,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220204,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                }
            };

            shops.Add(duckycapsuleseries5);

            Shop duckycapsuleseries6 = new Shop()
            {
                TemplateId = 0,
                Id = 101009,
                Category = 101009,
                Name = "shopetc",
                ShopType = ShopType.Capsule,
                RestrictSales = true,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 11220053,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220054,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320053,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320054,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620053,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620054,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720053,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720054,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820053,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820054,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220053,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220054,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220081,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220082,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320081,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320082,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620081,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620082,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720081,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720082,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820081,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820082,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220081,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220082,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },

                    new ShopItem
                    {
                        ItemId = 11220115,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220116,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320115,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320116,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620115,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620116,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720115,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720116,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220115,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220116,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320209,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320210,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720209,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720210,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220209,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220210,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320211,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320212,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320221,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320222,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320319,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320320,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320321,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320322,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820211,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820212,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                }
            };

            shops.Add(duckycapsuleseries6);

            Shop duckycapsuleseries7 = new Shop()
            {
                TemplateId = 0,
                Id = 101010,
                Category = 101010,
                Name = "shopetc",
                ShopType = ShopType.Capsule,
                RestrictSales = true,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 11220047,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220048,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320047,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320048,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620047,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620048,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720047,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720048,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820047,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820048,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220047,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220048,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11020039,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11020040,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320039,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320040,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620039,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620040,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720039,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720040,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820039,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820040,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220039,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220040,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },

                    new ShopItem
                    {
                        ItemId = 11020111,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11020112,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11120111,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11120112,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320111,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320112,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720111,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720112,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220111,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220112,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320219,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320220,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320323,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320324,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320401,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320402,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11420401,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11420402,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720401,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720402,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620401,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620402,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11520403,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11520404,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820217,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820218,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TemplateName = "22201001",
                        AutoPreviewEquip = true
                    },
                }
            };

            shops.Add(duckycapsuleseries7);

            Shop slimevarietycapsule1 = new Shop()
            {
                TemplateId = 0,
                Id = 102001,
                Category = 102001,
                Name = "shopetc",
                ShopType = ShopType.Capsule,
                RestrictSales = true,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 40410001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 40410002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 50610001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 50620001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70401001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70501001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70601001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70711001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70310001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70210001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20211001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 25,
                        ItemRank = 4,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 21101001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20802001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                }
            };
            shops.Add(slimevarietycapsule1);

            Shop slimecapsuleseries1 = new Shop()
            {
                TemplateId = 0,
                Id = 102002,
                Category = 102001,
                Name = "shopetc",
                ShopType = ShopType.Capsule,
                RestrictSales = true,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 40410003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 40410004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 50610002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 50620002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70401002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70501002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70601002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70711002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70310002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70210002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20211002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 25,
                        ItemRank = 4,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201029,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201009,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201010,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201011,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201012,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201013,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201014,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 21101002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20802002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                }
            };

            shops.Add(slimecapsuleseries1);

            Shop slimecapsuleseries2 = new Shop()
            {
                TemplateId = 0,
                Id = 102003,
                Category = 102001,
                Name = "shopetc",
                ShopType = ShopType.Capsule,
                RestrictSales = true,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 40410005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 40410006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 50610003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 50620003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70401003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70501003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70601003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70711003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70310003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70210003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20211003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 25,
                        ItemRank = 4,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201015,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201016,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201017,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201018,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201019,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201020,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201021,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 21101003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20802003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                }
            };

            shops.Add(slimecapsuleseries2);

            Shop slimecapsuleseries3 = new Shop()
            {
                TemplateId = 0,
                Id = 102004,
                Category = 102001,
                Name = "shopetc",
                ShopType = ShopType.Capsule,
                RestrictSales = true,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 40410007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 40410008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 50610004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 50620004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70401004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70501004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70601004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70711004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70310004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70210004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20211004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 25,
                        ItemRank = 4,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201022,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201023,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201024,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201025,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201026,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201027,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201028,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 21101004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20802004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                }
            };

            shops.Add(slimecapsuleseries3);

            Shop slimecapsuleseries4 = new Shop()
            {
                TemplateId = 0,
                Id = 102005,
                Category = 102001,
                Name = "shopetc",
                ShopType = ShopType.Capsule,
                RestrictSales = true,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 40410009,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 40410010,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 50610005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 50620005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70401005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70501005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70601005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70711005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70310005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70210005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20211005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 25,
                        ItemRank = 4,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201030,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201031,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201032,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201033,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201034,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201035,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 21101001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20802005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                }
            };

            shops.Add(slimecapsuleseries4);

            Shop slimecapsuleseries5 = new Shop()
            {
                TemplateId = 0,
                Id = 102006,
                Category = 102001,
                Name = "shopetc",
                ShopType = ShopType.Capsule,
                RestrictSales = true,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 40410013,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 40410014,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 50610007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 50620007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70401006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70501006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70601007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70711007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70310007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70210007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20211007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 25,
                        ItemRank = 4,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201043,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201044,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201045,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201046,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201047,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201048,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201049,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 21101003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20802007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                }
            };

            shops.Add(slimecapsuleseries5);

            Shop slimecapsuleseries6 = new Shop()
            {
                TemplateId = 0,
                Id = 102007,
                Category = 102001,
                Name = "shopetc",
                ShopType = ShopType.Capsule,
                RestrictSales = true,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 40410011,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 40410012,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 50610006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 50620006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70401007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70501007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70601006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70711006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70310006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70210006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20211006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 25,
                        ItemRank = 4,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201036,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201037,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201038,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201039,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201040,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201041,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201042,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 21101002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20802006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                }
            };

            shops.Add(slimecapsuleseries6);

            Shop slimecapsuleseries7 = new Shop()
            {
                TemplateId = 0,
                Id = 102008,
                Category = 102001,
                Name = "shopetc",
                ShopType = ShopType.Capsule,
                RestrictSales = true,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 40410015,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 40410016,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 50610008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 50620008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70401008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70501008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70601008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70711008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70310008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 70210008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20211008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 25,
                        ItemRank = 4,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201050,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201013,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201051,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201052,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201053,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201054,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20201055,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 21101004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                    new ShopItem
                    {
                        ItemId = 20802008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TemplateName = "22202001",
                    },
                }
            };

            shops.Add(slimecapsuleseries7);

            Shop holyidaycapsule = new Shop()
            {
                TemplateId = 0,
                Id = 104201,
                Category = 104201,
                Name = "shopetc",
                ShopType = ShopType.Capsule,
                RestrictSales = true,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 11220353,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 35,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220354,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 35,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320353,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320354,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620353,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620354,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720353,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720354,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820353,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820354,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220353,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220354,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220387,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 35,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220388,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 35,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320387,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320388,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620387,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620388,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720387,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720388,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820387,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820388,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220387,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220388,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220361,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 35,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220362,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 35,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320361,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320361,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620361,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620362,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720361,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720362,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820361,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820362,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220361,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220362,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 70910001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 4,
                        Category = "Other",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 20710001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 28,
                        ItemRank = 4,
                        Category = "Other",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 20303116,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 36,
                        ItemRank = 4,
                        Category = "Other",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 20303117,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 36,
                        ItemRank = 4,
                        Category = "Other",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 34000056,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Other",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 34000087,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Other",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                }
            };

            shops.Add(holyidaycapsule);

            Shop valentinescapsule = new Shop()
            {
                TemplateId = 0,
                Id = 104202,
                Category = 104202,
                Name = "shopetc",
                ShopType = ShopType.Capsule,
                RestrictSales = true,
                CanRestock = false,
                NextRestock = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                AllowBuyback = true,
                Items = new List<ShopItem>()
                {
                    new ShopItem
                    {
                        ItemId = 11220355,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 35,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220356,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 35,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320355,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320356,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620355,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620356,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720355,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720356,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820355,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820356,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220355,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220356,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220357,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 35,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11220358,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 35,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320357,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320358,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620357,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620358,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720357,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720358,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820357,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820358,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220357,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220358,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320419,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11320420,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620419,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11620420,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720419,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11720420,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820419,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 11820420,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220419,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 12220420,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 70910004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 4,
                        Category = "Other",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 20710003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 28,
                        ItemRank = 4,
                        Category = "Other",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 34000098,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Other",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 34000092,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Other",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 20303087,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 36,
                        ItemRank = 4,
                        Category = "Other",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        ItemId = 20303162,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 36,
                        ItemRank = 4,
                        Category = "Other",
                        Quantity = 1,
                        TemplateName = "22204001",
                        AutoPreviewEquip = true
                    },
                }
            };

            shops.Add(valentinescapsule);

            DatabaseManager.InsertShops(shops);
        }
    }
}
