using System;
using System.Collections.Generic;
using GameDataParser.Files;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class ShopParser : Exporter<List<ShopMetadata>>
    {
        public ShopParser() : base(null, "shop") { }

        protected override List<ShopMetadata> Parse()
        {
            List<ShopMetadata> shops = new List<ShopMetadata>();

            /*
             * Shop Name: Rumi
             * Map: Lith Harbor
             */
            ShopMetadata rumi = new ShopMetadata()
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
                        UniqueId = 436270,
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
                        UniqueId = 436271,
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
                        UniqueId = 436272,
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
                        UniqueId = 436273,
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
                        UniqueId = 436274,
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
                        UniqueId = 436275,
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
                        UniqueId = 436276,
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
                        UniqueId = 436277,
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
                        UniqueId = 436278,
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
                        UniqueId = 436279,
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
                        UniqueId = 436280,
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
                        UniqueId = 436281,
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
                        UniqueId = 436282,
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
                        UniqueId = 436283,
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
                        UniqueId = 436284,
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
            ShopMetadata lazy = new ShopMetadata()
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
                        UniqueId = 161251159,
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
                        UniqueId = 161251160,
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
                        UniqueId = 161251161,
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
                        UniqueId = 161251162,
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
                        UniqueId = 161251163,
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
                        UniqueId = 161251164,
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
                        UniqueId = 161251165,
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
                        UniqueId = 161251166,
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
                        UniqueId = 161251167,
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
                        UniqueId = 161251168,
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
                        UniqueId = 161251169,
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
                        UniqueId = 161251170,
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
                        UniqueId = 161251171,
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
                        UniqueId = 161251172,
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
                        UniqueId = 161251173,
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
                        UniqueId = 161251174,
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
                        UniqueId = 161251175,
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
                        UniqueId = 161251176,
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
                        UniqueId = 161251177,
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
                        UniqueId = 161251178,
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
                        UniqueId = 161251179,
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
                        UniqueId = 161251180,
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
                        UniqueId = 161251181,
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
            ShopMetadata redstarshop = new ShopMetadata()
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
                        UniqueId = 896999,
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
                        UniqueId = 897000,
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
                        UniqueId = 897001,
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
                        UniqueId = 897002,
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
                        UniqueId = 897003,
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
                        UniqueId = 897004,
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
                        UniqueId = 897005,
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
                        UniqueId = 897006,
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
                        UniqueId = 897007,
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
                        UniqueId = 897008,
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
                        UniqueId = 897009,
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
                        UniqueId = 897010,
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
                        UniqueId = 897011,
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
                        UniqueId = 897012,
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
                        UniqueId = 897013,
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
                        UniqueId = 897014,
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
                        UniqueId = 897015,
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
                        UniqueId = 897016,
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
                        UniqueId = 897017,
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
                        UniqueId = 897018,
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
                        UniqueId = 897019,
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
            ShopMetadata bluestarshop = new ShopMetadata()
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
                        UniqueId = 896980,
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
                        UniqueId = 896981,
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
                        UniqueId = 896982,
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
                        UniqueId = 896983,
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
                        UniqueId = 896984,
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
                        UniqueId = 896985,
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
                        UniqueId = 896986,
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
                        UniqueId = 896987,
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
                        UniqueId = 896988,
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
                        UniqueId = 896989,
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
                        UniqueId = 896990,
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
                        UniqueId = 896991,
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
                        UniqueId = 896992,
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
                        UniqueId = 896993,
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
                        UniqueId = 896994,
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
                        UniqueId = 896995,
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
                        UniqueId = 896996,
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
                        UniqueId = 896997,
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
                        UniqueId = 896998,
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
            ShopMetadata eventshop = new ShopMetadata()
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
            ShopMetadata lushop = new ShopMetadata()
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
            ShopMetadata beros = new ShopMetadata()
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
            ShopMetadata borka = new ShopMetadata()
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
                        UniqueId = 7116778,
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
                        UniqueId = 7116779,
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
                        UniqueId = 7116780,
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
                        UniqueId = 7116781,
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
                        UniqueId = 7116782,
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
                        UniqueId = 7116783,
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
                        UniqueId = 7116784,
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
                        UniqueId = 7116785,
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
                        UniqueId = 7116786,
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
                        UniqueId = 7116787,
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
                        UniqueId = 7116788,
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
                        UniqueId = 7116789,
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
                        UniqueId = 7116790,
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
                        UniqueId = 7116791,
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
                        UniqueId = 7116792,
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
            ShopMetadata hibeck = new ShopMetadata()
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
            ShopMetadata matz = new ShopMetadata()
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
            ShopMetadata rainbowArc = new ShopMetadata()
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
                        UniqueId = 30360031,
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
                        UniqueId = 30360032,
                        ItemId = 11800085,
                        TokenType = ShopCurrencyType.ValorToken,
                        ItemRank = 4,
                        Category = "PVPAcc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360033,
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
                    new ShopItem
                    {
                        UniqueId = 30360034,
                        ItemId = 13100328,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPWeapon",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360035,
                        ItemId = 13200323,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPWeapon",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360036,
                        ItemId = 13300322,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPWeapon",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360037,
                        ItemId = 13400321,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPWeapon",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360038,
                        ItemId = 14000284,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPWeapon",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360039,
                        ItemId = 14100293,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPWeapon",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360040,
                        ItemId = 15000327,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 200000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPWeapon",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360041,
                        ItemId = 15100319,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 200000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPWeapon",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360042,
                        ItemId = 15200326,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 200000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPWeapon",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360043,
                        ItemId = 15300322,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 200000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPWeapon",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360044,
                        ItemId = 15400307,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 200000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPWeapon",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360045,
                        ItemId = 15500540,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 200000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPWeapon",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360046,
                        ItemId = 15600542,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 200000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPWeapon",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360047,
                        ItemId = 11301591,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360048,
                        ItemId = 11301592,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360049,
                        ItemId = 11301593,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360050,
                        ItemId = 11301594,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360051,
                        ItemId = 11301595,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360052,
                        ItemId = 11301596,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360053,
                        ItemId = 11301597,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360054,
                        ItemId = 11301598,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360055,
                        ItemId = 11301599,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360056,
                        ItemId = 11301600,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360057,
                        ItemId = 11301601,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360058,
                        ItemId = 11401215,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360059,
                        ItemId = 11401216,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360060,
                        ItemId = 11401217,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360061,
                        ItemId = 11401218,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360062,
                        ItemId = 11401219,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360063,
                        ItemId = 11401220,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360064,
                        ItemId = 11401221,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360065,
                        ItemId = 11401222,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360066,
                        ItemId = 11401223,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360067,
                        ItemId = 11401224,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360068,
                        ItemId = 11401225,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360069,
                        ItemId = 11501096,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360070,
                        ItemId = 11501097,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360071,
                        ItemId = 11501098,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360072,
                        ItemId = 11501099,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360073,
                        ItemId = 11501100,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360074,
                        ItemId = 11501101,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360075,
                        ItemId = 11501102,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360076,
                        ItemId = 11501103,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360077,
                        ItemId = 11501104,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360078,
                        ItemId = 11501105,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360079,
                        ItemId = 11501106,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360080,
                        ItemId = 11601378,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360081,
                        ItemId = 11601379,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360082,
                        ItemId = 11601380,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360083,
                        ItemId = 11601381,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360084,
                        ItemId = 11601382,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360085,
                        ItemId = 11601383,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360086,
                        ItemId = 11601384,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360087,
                        ItemId = 11601385,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360088,
                        ItemId = 11601386,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360089,
                        ItemId = 11601387,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360090,
                        ItemId = 11601388,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360091,
                        ItemId = 11701492,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360092,
                        ItemId = 11701493,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360093,
                        ItemId = 11701494,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360094,
                        ItemId = 11701495,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360095,
                        ItemId = 11701496,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360096,
                        ItemId = 11701497,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360097,
                        ItemId = 11701498,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360098,
                        ItemId = 11701499,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360099,
                        ItemId = 11701500,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360100,
                        ItemId = 11701501,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360101,
                        ItemId = 11701502,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPArmor",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360102,
                        ItemId = 11200106,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPAcc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360103,
                        ItemId = 11800150,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPAcc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360104,
                        ItemId = 11900128,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPAcc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360105,
                        ItemId = 12000117,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPAcc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    },
                    new ShopItem
                    {
                        UniqueId = 30360106,
                        ItemId = 12100118,
                        TokenType = ShopCurrencyType.Meso,
                        RequiredItemId = 0,
                        Price = 100000,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "PVPAcc",
                        Quantity = 1,
                        Flag = ShopItemFlag.New
                    }
                }
            };

            shops.Add(rainbowArc);

            /*
             * Shop Name: Spartan (Dungeon Helper Shop)
             * Map: Queenstown
             */
            ShopMetadata spartan = new ShopMetadata()
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
            ShopMetadata stefan = new ShopMetadata()
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
                        UniqueId = 4096999,
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
                        UniqueId = 4097000,
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
                        UniqueId = 4097001,
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
                        UniqueId = 4097002,
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
                        UniqueId = 4097003,
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
                        UniqueId = 4097004,
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
                        UniqueId = 4097005,
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
                        UniqueId = 4097006,
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
                        UniqueId = 4097007,
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
                        UniqueId = 4097008,
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
                        UniqueId = 4097009,
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
                        UniqueId = 4097010,
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
                        UniqueId = 4097011,
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
                        UniqueId = 4097012,
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
                        UniqueId = 4097013,
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
                        UniqueId = 4097014,
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
                        UniqueId = 4097015,
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
                        UniqueId = 4097016,
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
                        UniqueId = 4097017,
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
                        UniqueId = 4097018,
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
                        UniqueId = 4097019,
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
                        UniqueId = 4097020,
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
                        UniqueId = 4097021,
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
                        UniqueId = 4097022,
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
                        UniqueId = 4097023,
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
                        UniqueId = 4097024,
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
                        UniqueId = 4097025,
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
                        UniqueId = 4097026,
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
                        UniqueId = 4097027,
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
                        UniqueId = 4097028,
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
                        UniqueId = 4097029,
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
                        UniqueId = 4097030,
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
                        UniqueId = 4097031,
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
                        UniqueId = 4097032,
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
                        UniqueId = 4097033,
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
                        UniqueId = 4097034,
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
                        UniqueId = 4097035,
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
                        UniqueId = 4097036,
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
                        UniqueId = 4097037,
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
                        UniqueId = 4097038,
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
                        UniqueId = 4097039,
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
                        UniqueId = 4097040,
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
                        UniqueId = 4097041,
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
                        UniqueId = 4097042,
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
                        UniqueId = 4097043,
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
                        UniqueId = 4097044,
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
                        UniqueId = 4097045,
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


            return shops;
        }
    }
}
