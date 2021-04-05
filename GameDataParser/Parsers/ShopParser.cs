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

            ShopMetadata holidaycapsule2019 = new ShopMetadata()
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
                        UniqueId = 10097000,
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

            ShopMetadata duckycapsule1 = new ShopMetadata()
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
                        UniqueId = 10100100,
                        ItemId = 11220311,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100101,
                        ItemId = 11220312,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100102,
                        ItemId = 11320311,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100103,
                        ItemId = 11320312,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100104,
                        ItemId = 11620311,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100105,
                        ItemId = 11620312,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100106,
                        ItemId = 11720311,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100107,
                        ItemId = 11720312,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100108,
                        ItemId = 11820311,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100109,
                        ItemId = 11820312,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100110,
                        ItemId = 12220311,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100111,
                        ItemId = 12220312,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100112,
                        ItemId = 11220001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100113,
                        ItemId = 11220002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100114,
                        ItemId = 11320001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100115,
                        ItemId = 11320002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100116,
                        ItemId = 11620001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100117,
                        ItemId = 11620002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100118,
                        ItemId = 11720001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100119,
                        ItemId = 11720002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100120,
                        ItemId = 11820001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100121,
                        ItemId = 11820002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100122,
                        ItemId = 12220001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100123,
                        ItemId = 12220002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100124,
                        ItemId = 11320095,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100125,
                        ItemId = 11320096,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100126,
                        ItemId = 11620095,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100127,
                        ItemId = 11620096,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100128,
                        ItemId = 11720095,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100129,
                        ItemId = 11720096,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100130,
                        ItemId = 12220095,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100131,
                        ItemId = 12220096,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100132,
                        ItemId = 11120153,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100133,
                        ItemId = 11120154,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100134,
                        ItemId = 11220147,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100135,
                        ItemId = 11220152,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100136,
                        ItemId = 11320145,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100137,
                        ItemId = 11320152,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100138,
                        ItemId = 11320147,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100139,
                        ItemId = 11320150,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100140,
                        ItemId = 12220152,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100141,
                        ItemId = 11420147,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100142,
                        ItemId = 11520147,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100143,
                        ItemId = 11720147,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100144,
                        ItemId = 11720152,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100145,
                        ItemId = 11820147,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100146,
                        ItemId = 11820152,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100147,
                        ItemId = 12220143,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100148,
                        ItemId = 12220144,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    }
                }
            };

            shops.Add(duckycapsule1);

            ShopMetadata duckycapsule2 = new ShopMetadata()
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
                        UniqueId = 10100200,
                        ItemId = 11220003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100201,
                        ItemId = 11220004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100202,
                        ItemId = 11320003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100203,
                        ItemId = 11320004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100204,
                        ItemId = 11620003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100205,
                        ItemId = 11620004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100206,
                        ItemId = 11720003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100207,
                        ItemId = 11720004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100208,
                        ItemId = 11820003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100209,
                        ItemId = 11820004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100210,
                        ItemId = 12220003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100211,
                        ItemId = 12220004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100212,
                        ItemId = 11020005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100213,
                        ItemId = 11020006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100214,
                        ItemId = 11320005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100215,
                        ItemId = 11320006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100216,
                        ItemId = 11620005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100217,
                        ItemId = 11620006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100218,
                        ItemId = 11720005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100219,
                        ItemId = 11720006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100220,
                        ItemId = 11820005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100221,
                        ItemId = 11820006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100222,
                        ItemId = 12220005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100223,
                        ItemId = 12220006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100224,
                        ItemId = 11320097,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100225,
                        ItemId = 11320098,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100226,
                        ItemId = 11620097,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100227,
                        ItemId = 11620098,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100228,
                        ItemId = 11720097,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100229,
                        ItemId = 11720098,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100230,
                        ItemId = 12220097,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100231,
                        ItemId = 12220098,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100232,
                        ItemId = 11020313,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100233,
                        ItemId = 11020314,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100234,
                        ItemId = 11120159,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100235,
                        ItemId = 11120160,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100236,
                        ItemId = 11320313,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100237,
                        ItemId = 11320314,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100238,
                        ItemId = 11720157,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100239,
                        ItemId = 11720158,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100240,
                        ItemId = 11720287,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100241,
                        ItemId = 11720288,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100242,
                        ItemId = 11820157,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100243,
                        ItemId = 11820158,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100244,
                        ItemId = 11820313,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100245,
                        ItemId = 11820314,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100246,
                        ItemId = 12220159,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100247,
                        ItemId = 12220160,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100248,
                        ItemId = 12220313,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100249,
                        ItemId = 12220314,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    }
                },
            };

            shops.Add(duckycapsule2);

            ShopMetadata duckycapsule3 = new ShopMetadata()
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
                        UniqueId = 10100300,
                        ItemId = 11220007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100301,
                        ItemId = 11220008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100302,
                        ItemId = 11320007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100303,
                        ItemId = 11320008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100304,
                        ItemId = 11620007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100305,
                        ItemId = 11620008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100306,
                        ItemId = 11720007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100307,
                        ItemId = 11720008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100308,
                        ItemId = 11820007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100309,
                        ItemId = 11820008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100310,
                        ItemId = 12220007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100311,
                        ItemId = 12220008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100312,
                        ItemId = 11220009,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100313,
                        ItemId = 11220010,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100314,
                        ItemId = 11320009,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100315,
                        ItemId = 11320010,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100316,
                        ItemId = 11620009,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100317,
                        ItemId = 11620010,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100318,
                        ItemId = 11720009,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100319,
                        ItemId = 11720010,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100320,
                        ItemId = 11820009,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100321,
                        ItemId = 11820010,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100322,
                        ItemId = 12220009,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100323,
                        ItemId = 12220010,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100324,
                        ItemId = 11120099,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100325,
                        ItemId = 11120100,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100326,
                        ItemId = 11320099,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100327,
                        ItemId = 11320100,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100328,
                        ItemId = 11620099,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100329,
                        ItemId = 11620100,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100330,
                        ItemId = 11720099,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100331,
                        ItemId = 11720100,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100332,
                        ItemId = 12220099,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100333,
                        ItemId = 12220100,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100334,
                        ItemId = 11320161,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100335,
                        ItemId = 11320162,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100336,
                        ItemId = 11320315,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100337,
                        ItemId = 11320316,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100338,
                        ItemId = 11720305,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100339,
                        ItemId = 11720306,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100340,
                        ItemId = 11820161,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100341,
                        ItemId = 11820162,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100342,
                        ItemId = 11820163,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100343,
                        ItemId = 11820164,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100344,
                        ItemId = 12220157,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100345,
                        ItemId = 12220158,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100346,
                        ItemId = 12220301,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100347,
                        ItemId = 12220302,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100348,
                        ItemId = 12220315,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100349,
                        ItemId = 12220316,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                }
            };

            shops.Add(duckycapsule3);

            ShopMetadata duckycapsuleseries1 = new ShopMetadata()
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
                        UniqueId = 10100400,
                        ItemId = 11220317,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100401,
                        ItemId = 11220318,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100402,
                        ItemId = 11320317,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100403,
                        ItemId = 11320318,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100404,
                        ItemId = 11620317,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100405,
                        ItemId = 11620318,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100406,
                        ItemId = 11720317,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100407,
                        ItemId = 11720318,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100408,
                        ItemId = 11820317,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100409,
                        ItemId = 11820318,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100410,
                        ItemId = 12220317,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100411,
                        ItemId = 12220318,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100412,
                        ItemId = 11220013,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100413,
                        ItemId = 11220014,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100414,
                        ItemId = 11320013,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100415,
                        ItemId = 11320014,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100416,
                        ItemId = 11620013,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100417,
                        ItemId = 11620014,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100418,
                        ItemId = 11720013,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100419,
                        ItemId = 11720014,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100420,
                        ItemId = 11820013,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100421,
                        ItemId = 11820014,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100422,
                        ItemId = 12220013,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100423,
                        ItemId = 12220014,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100424,
                        ItemId = 11220101,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100425,
                        ItemId = 11220102,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100426,
                        ItemId = 11320101,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100427,
                        ItemId = 11320102,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100428,
                        ItemId = 11620101,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100429,
                        ItemId = 11620102,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100430,
                        ItemId = 11720101,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100431,
                        ItemId = 11720102,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100432,
                        ItemId = 12220101,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100433,
                        ItemId = 12220102,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100434,
                        ItemId = 11320167,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100435,
                        ItemId = 11320168,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100436,
                        ItemId = 11620167,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100437,
                        ItemId = 11620168,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100438,
                        ItemId = 11720167,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100439,
                        ItemId = 11720168,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100440,
                        ItemId = 12220167,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100441,
                        ItemId = 12220168,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100442,
                        ItemId = 11320169,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100443,
                        ItemId = 11320170,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100444,
                        ItemId = 11320172,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100445,
                        ItemId = 11320173,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100446,
                        ItemId = 11820169,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100447,
                        ItemId = 11820170,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                }
            };

            shops.Add(duckycapsuleseries1);

            ShopMetadata duckycapsuleseries2 = new ShopMetadata()
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
                        UniqueId = 10100500,
                        ItemId = 11220367,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100501,
                        ItemId = 11220368,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100502,
                        ItemId = 11320367,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100503,
                        ItemId = 11320368,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100504,
                        ItemId = 11620367,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100505,
                        ItemId = 11620368,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100506,
                        ItemId = 11720367,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100507,
                        ItemId = 11720368,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100508,
                        ItemId = 11820367,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100509,
                        ItemId = 11820368,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100510,
                        ItemId = 12220367,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100511,
                        ItemId = 12220368,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100512,
                        ItemId = 11220017,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100513,
                        ItemId = 11220018,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100514,
                        ItemId = 11320017,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100515,
                        ItemId = 11320018,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100516,
                        ItemId = 11620017,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100517,
                        ItemId = 11620018,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100518,
                        ItemId = 11720017,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100519,
                        ItemId = 11720018,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100520,
                        ItemId = 11820017,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100521,
                        ItemId = 11820018,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100522,
                        ItemId = 12220017,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100523,
                        ItemId = 12220018,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100524,
                        ItemId = 11320103,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100525,
                        ItemId = 11320104,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100526,
                        ItemId = 11620103,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100527,
                        ItemId = 11620104,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100528,
                        ItemId = 11720103,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100529,
                        ItemId = 11720104,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100530,
                        ItemId = 11820103,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100531,
                        ItemId = 11820104,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100532,
                        ItemId = 12220103,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100533,
                        ItemId = 12220104,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100534,
                        ItemId = 11320175,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100535,
                        ItemId = 11320176,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100536,
                        ItemId = 11320181,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100537,
                        ItemId = 11320182,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100538,
                        ItemId = 11420175,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100539,
                        ItemId = 11420176,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100540,
                        ItemId = 11420177,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100541,
                        ItemId = 11420178,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100542,
                        ItemId = 11720175,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100543,
                        ItemId = 11720176,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100544,
                        ItemId = 11820175,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100545,
                        ItemId = 11820176,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100546,
                        ItemId = 11820181,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100547,
                        ItemId = 11820182,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100548,
                        ItemId = 12220307,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100549,
                        ItemId = 12220308,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                }
            };

            shops.Add(duckycapsuleseries2);

            ShopMetadata duckycapsuleseries3 = new ShopMetadata()
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
                        UniqueId = 10100600,
                        ItemId = 11220023,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100601,
                        ItemId = 11220024,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100602,
                        ItemId = 11320023,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100603,
                        ItemId = 11320024,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100604,
                        ItemId = 11620023,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100605,
                        ItemId = 11620024,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100606,
                        ItemId = 11720023,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100607,
                        ItemId = 11720024,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100608,
                        ItemId = 11820023,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100609,
                        ItemId = 11820024,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100610,
                        ItemId = 12220023,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100611,
                        ItemId = 12220024,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100612,
                        ItemId = 11220033,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100613,
                        ItemId = 11220034,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100614,
                        ItemId = 11320033,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100615,
                        ItemId = 11320034,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100616,
                        ItemId = 11620033,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100617,
                        ItemId = 11620034,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100618,
                        ItemId = 11720033,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100619,
                        ItemId = 11720034,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100620,
                        ItemId = 11820033,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100621,
                        ItemId = 11820034,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100622,
                        ItemId = 12220033,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100623,
                        ItemId = 12220034,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },

                    new ShopItem
                    {
                        UniqueId = 10100624,
                        ItemId = 11220107,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100625,
                        ItemId = 11220108,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100626,
                        ItemId = 11320107,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100627,
                        ItemId = 11320108,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100628,
                        ItemId = 11620107,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100629,
                        ItemId = 11620108,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100630,
                        ItemId = 11720107,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100631,
                        ItemId = 11720108,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100632,
                        ItemId = 12220107,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100633,
                        ItemId = 12220108,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100634,
                        ItemId = 11020193,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100635,
                        ItemId = 11020194,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100636,
                        ItemId = 11020201,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100637,
                        ItemId = 11020202,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100638,
                        ItemId = 11020197,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100639,
                        ItemId = 11020198,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100640,
                        ItemId = 11220199,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100641,
                        ItemId = 11220200,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100642,
                        ItemId = 11320193,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100643,
                        ItemId = 11320194,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100644,
                        ItemId = 11620201,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100645,
                        ItemId = 11620202,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100646,
                        ItemId = 11720319,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100647,
                        ItemId = 11720320,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100648,
                        ItemId = 12220319,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100649,
                        ItemId = 12220320,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                }
            };

            shops.Add(duckycapsuleseries3);

            ShopMetadata duckycapsuleseries4 = new ShopMetadata()
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
                        UniqueId = 10100700,
                        ItemId = 11220031,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100701,
                        ItemId = 11220032,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100702,
                        ItemId = 11320031,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100703,
                        ItemId = 11320032,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100704,
                        ItemId = 11620031,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100705,
                        ItemId = 11620032,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100706,
                        ItemId = 11720031,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100707,
                        ItemId = 11720032,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100708,
                        ItemId = 11820031,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100709,
                        ItemId = 11820032,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100710,
                        ItemId = 12220031,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100711,
                        ItemId = 12220032,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100712,
                        ItemId = 11220041,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100713,
                        ItemId = 11220042,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100714,
                        ItemId = 11320041,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100715,
                        ItemId = 11320042,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100716,
                        ItemId = 11620041,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100717,
                        ItemId = 11620042,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100718,
                        ItemId = 11720041,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100719,
                        ItemId = 11720042,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100720,
                        ItemId = 11820041,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100721,
                        ItemId = 11820042,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100722,
                        ItemId = 12220041,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100723,
                        ItemId = 12220042,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },

                    new ShopItem
                    {
                        UniqueId = 10100724,
                        ItemId = 11320109,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100725,
                        ItemId = 11320110,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100726,
                        ItemId = 11620109,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100727,
                        ItemId = 11620110,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100728,
                        ItemId = 11720109,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100729,
                        ItemId = 11720110,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100730,
                        ItemId = 11820109,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100731,
                        ItemId = 11820110,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100732,
                        ItemId = 12220109,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100733,
                        ItemId = 12220110,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100734,
                        ItemId = 11320185,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100735,
                        ItemId = 11320186,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100736,
                        ItemId = 11320187,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100737,
                        ItemId = 11320188,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100738,
                        ItemId = 11320189,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100739,
                        ItemId = 11320190,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100740,
                        ItemId = 11320191,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100741,
                        ItemId = 11320192,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100742,
                        ItemId = 11820185,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100743,
                        ItemId = 11820186,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100744,
                        ItemId = 11820187,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100745,
                        ItemId = 11820188,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100746,
                        ItemId = 11820189,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100747,
                        ItemId = 11820190,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100748,
                        ItemId = 11820191,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100749,
                        ItemId = 11820192,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                }
            };

            shops.Add(duckycapsuleseries4);

            ShopMetadata duckycapsuleseries5 = new ShopMetadata()
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
                        UniqueId = 10100800,
                        ItemId = 11220027,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100801,
                        ItemId = 11220028,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100802,
                        ItemId = 11320027,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100803,
                        ItemId = 11320028,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100804,
                        ItemId = 11620027,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100805,
                        ItemId = 11620028,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100806,
                        ItemId = 11720027,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100807,
                        ItemId = 11720028,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100808,
                        ItemId = 11820027,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100809,
                        ItemId = 11820028,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100810,
                        ItemId = 12220027,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100811,
                        ItemId = 12220028,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100812,
                        ItemId = 11220089,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100813,
                        ItemId = 11220090,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100814,
                        ItemId = 11320089,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100815,
                        ItemId = 11320090,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100816,
                        ItemId = 11620089,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100817,
                        ItemId = 11620090,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100818,
                        ItemId = 11720089,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100819,
                        ItemId = 11720090,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100820,
                        ItemId = 11820089,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100821,
                        ItemId = 11820090,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100822,
                        ItemId = 12220089,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100823,
                        ItemId = 12220090,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },

                    new ShopItem
                    {
                        UniqueId = 10100824,
                        ItemId = 11120351,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100825,
                        ItemId = 11120352,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100826,
                        ItemId = 11320351,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100827,
                        ItemId = 11320352,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100828,
                        ItemId = 11620351,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100829,
                        ItemId = 11620352,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100830,
                        ItemId = 11720351,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100831,
                        ItemId = 11720352,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100832,
                        ItemId = 12220351,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100833,
                        ItemId = 12220352,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100834,
                        ItemId = 11120207,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100835,
                        ItemId = 11120208,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100836,
                        ItemId = 11320205,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100837,
                        ItemId = 11320206,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100838,
                        ItemId = 11820205,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100839,
                        ItemId = 11820206,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100840,
                        ItemId = 12220193,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100841,
                        ItemId = 12220194,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100842,
                        ItemId = 11320203,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100843,
                        ItemId = 11320204,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100844,
                        ItemId = 11620203,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100845,
                        ItemId = 11620204,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100846,
                        ItemId = 11720203,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100847,
                        ItemId = 11720204,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100848,
                        ItemId = 12220203,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100849,
                        ItemId = 12220204,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                }
            };

            shops.Add(duckycapsuleseries5);

            ShopMetadata duckycapsuleseries6 = new ShopMetadata()
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
                        UniqueId = 10100900,
                        ItemId = 11220053,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100901,
                        ItemId = 11220054,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100902,
                        ItemId = 11320053,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100903,
                        ItemId = 11320054,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100904,
                        ItemId = 11620053,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100905,
                        ItemId = 11620054,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100906,
                        ItemId = 11720053,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100907,
                        ItemId = 11720054,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100908,
                        ItemId = 11820053,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100909,
                        ItemId = 11820054,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100910,
                        ItemId = 12220053,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100911,
                        ItemId = 12220054,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100912,
                        ItemId = 11220081,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100913,
                        ItemId = 11220082,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100914,
                        ItemId = 11320081,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100915,
                        ItemId = 11320082,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100916,
                        ItemId = 11620081,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100917,
                        ItemId = 11620082,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100918,
                        ItemId = 11720081,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100919,
                        ItemId = 11720082,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100920,
                        ItemId = 11820081,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100921,
                        ItemId = 11820082,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100922,
                        ItemId = 12220081,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100923,
                        ItemId = 12220082,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },

                    new ShopItem
                    {
                        UniqueId = 10100924,
                        ItemId = 11220115,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100925,
                        ItemId = 11220116,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100926,
                        ItemId = 11320115,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100927,
                        ItemId = 11320116,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100928,
                        ItemId = 11620115,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100929,
                        ItemId = 11620116,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100930,
                        ItemId = 11720115,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100931,
                        ItemId = 11720116,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100932,
                        ItemId = 12220115,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100933,
                        ItemId = 12220116,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100934,
                        ItemId = 11320209,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100935,
                        ItemId = 11320210,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100936,
                        ItemId = 11720209,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100937,
                        ItemId = 11720210,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100938,
                        ItemId = 12220209,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100939,
                        ItemId = 12220210,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100940,
                        ItemId = 11320211,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100941,
                        ItemId = 11320212,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100942,
                        ItemId = 11320221,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100943,
                        ItemId = 11320222,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100944,
                        ItemId = 11320319,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100945,
                        ItemId = 11320320,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100946,
                        ItemId = 11320321,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100947,
                        ItemId = 11320322,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100948,
                        ItemId = 11820211,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10100949,
                        ItemId = 11820212,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201009,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                }
            };

            shops.Add(duckycapsuleseries6);

            ShopMetadata duckycapsuleseries7 = new ShopMetadata()
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
                        UniqueId = 10101000,
                        ItemId = 11220047,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101001,
                        ItemId = 11220048,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101002,
                        ItemId = 11320047,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101003,
                        ItemId = 11320048,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101004,
                        ItemId = 11620047,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101005,
                        ItemId = 11620048,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101006,
                        ItemId = 11720047,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101007,
                        ItemId = 11720048,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101008,
                        ItemId = 11820047,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101009,
                        ItemId = 11820048,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101010,
                        ItemId = 12220047,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101011,
                        ItemId = 12220048,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt01",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101012,
                        ItemId = 11020039,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101013,
                        ItemId = 11020040,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 32,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101014,
                        ItemId = 11320039,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101015,
                        ItemId = 11320040,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101016,
                        ItemId = 11620039,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101017,
                        ItemId = 11620040,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101018,
                        ItemId = 11720039,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101019,
                        ItemId = 11720040,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 33,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101020,
                        ItemId = 11820039,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101021,
                        ItemId = 11820040,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 34,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101022,
                        ItemId = 12220039,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101023,
                        ItemId = 12220040,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 35,
                        ItemRank = 4,
                        Category = "Excellecnt02",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },

                    new ShopItem
                    {
                        UniqueId = 10101024,
                        ItemId = 11020111,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101025,
                        ItemId = 11020112,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101026,
                        ItemId = 11120111,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101027,
                        ItemId = 11120112,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101028,
                        ItemId = 11320111,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101029,
                        ItemId = 11320112,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101030,
                        ItemId = 11720111,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101031,
                        ItemId = 11720112,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101032,
                        ItemId = 12220111,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101033,
                        ItemId = 12220112,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Elite",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101034,
                        ItemId = 11320219,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101035,
                        ItemId = 11320220,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101036,
                        ItemId = 11320323,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101037,
                        ItemId = 11320324,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101038,
                        ItemId = 11320401,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101039,
                        ItemId = 11320402,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101040,
                        ItemId = 11420401,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101041,
                        ItemId = 11420402,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101042,
                        ItemId = 11720401,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101043,
                        ItemId = 11720402,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101044,
                        ItemId = 11620401,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101045,
                        ItemId = 11620402,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101046,
                        ItemId = 11520403,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101047,
                        ItemId = 11520404,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101048,
                        ItemId = 11820217,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10101049,
                        ItemId = 11820218,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22201010,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Normal",
                        Quantity = 1,
                        TokenName = "22201001",
                        AutoPreviewEquip = true
                    },
                }
            };

            shops.Add(duckycapsuleseries7);

            ShopMetadata slimevarietycapsule1 = new ShopMetadata()
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
                        UniqueId = 10200100,
                        ItemId = 40410001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200101,
                        ItemId = 40410002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200102,
                        ItemId = 50610001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200103,
                        ItemId = 50620001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200104,
                        ItemId = 70401001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200105,
                        ItemId = 70501001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200106,
                        ItemId = 70601001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200107,
                        ItemId = 70711001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200108,
                        ItemId = 70310001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200109,
                        ItemId = 70210001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200110,
                        ItemId = 20211001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 25,
                        ItemRank = 4,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200111,
                        ItemId = 20201001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200112,
                        ItemId = 20201002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200113,
                        ItemId = 20201003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200114,
                        ItemId = 20201004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200115,
                        ItemId = 20201005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200116,
                        ItemId = 20201006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200117,
                        ItemId = 20201007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200118,
                        ItemId = 21101001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200119,
                        ItemId = 20802001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202001,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                }
            };
            shops.Add(slimevarietycapsule1);

            ShopMetadata slimecapsuleseries1 = new ShopMetadata()
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
                        UniqueId = 10200200,
                        ItemId = 40410003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200201,
                        ItemId = 40410004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200202,
                        ItemId = 50610002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200203,
                        ItemId = 50620002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200204,
                        ItemId = 70401002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200205,
                        ItemId = 70501002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200206,
                        ItemId = 70601002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200207,
                        ItemId = 70711002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200208,
                        ItemId = 70310002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200209,
                        ItemId = 70210002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200210,
                        ItemId = 20211002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 25,
                        ItemRank = 4,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200211,
                        ItemId = 20201029,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200212,
                        ItemId = 20201009,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200213,
                        ItemId = 20201010,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200214,
                        ItemId = 20201011,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200215,
                        ItemId = 20201012,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200216,
                        ItemId = 20201013,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200217,
                        ItemId = 20201014,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200218,
                        ItemId = 21101002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200219,
                        ItemId = 20802002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202002,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                }
            };

            shops.Add(slimecapsuleseries1);

            ShopMetadata slimecapsuleseries2 = new ShopMetadata()
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
                        UniqueId = 10200300,
                        ItemId = 40410005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200301,
                        ItemId = 40410006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200302,
                        ItemId = 50610003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200303,
                        ItemId = 50620003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200304,
                        ItemId = 70401003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200305,
                        ItemId = 70501003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200306,
                        ItemId = 70601003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200307,
                        ItemId = 70711003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200308,
                        ItemId = 70310003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200309,
                        ItemId = 70210003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200310,
                        ItemId = 20211003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 25,
                        ItemRank = 4,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200311,
                        ItemId = 20201015,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200312,
                        ItemId = 20201016,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200313,
                        ItemId = 20201017,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200314,
                        ItemId = 20201018,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200315,
                        ItemId = 20201019,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200316,
                        ItemId = 20201020,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200317,
                        ItemId = 20201021,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200318,
                        ItemId = 21101003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200319,
                        ItemId = 20802003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202003,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                }
            };

            shops.Add(slimecapsuleseries2);

            ShopMetadata slimecapsuleseries3 = new ShopMetadata()
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
                        UniqueId = 10200400,
                        ItemId = 40410007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200401,
                        ItemId = 40410008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200402,
                        ItemId = 50610004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200403,
                        ItemId = 50620004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200404,
                        ItemId = 70401004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200405,
                        ItemId = 70501004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200406,
                        ItemId = 70601004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200407,
                        ItemId = 70711004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200408,
                        ItemId = 70310004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200409,
                        ItemId = 70210004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200410,
                        ItemId = 20211004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 25,
                        ItemRank = 4,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200411,
                        ItemId = 20201022,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200412,
                        ItemId = 20201023,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200413,
                        ItemId = 20201024,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200414,
                        ItemId = 20201025,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200415,
                        ItemId = 20201026,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200416,
                        ItemId = 20201027,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200417,
                        ItemId = 20201028,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200418,
                        ItemId = 21101004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200419,
                        ItemId = 20802004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202004,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                }
            };

            shops.Add(slimecapsuleseries3);

            ShopMetadata slimecapsuleseries4 = new ShopMetadata()
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
                        UniqueId = 10200500,
                        ItemId = 40410009,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200501,
                        ItemId = 40410010,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200502,
                        ItemId = 50610005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200503,
                        ItemId = 50620005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200504,
                        ItemId = 70401005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200505,
                        ItemId = 70501005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200506,
                        ItemId = 70601005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200507,
                        ItemId = 70711005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200508,
                        ItemId = 70310005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200509,
                        ItemId = 70210005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200510,
                        ItemId = 20211005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 25,
                        ItemRank = 4,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200511,
                        ItemId = 20201008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200512,
                        ItemId = 20201030,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200513,
                        ItemId = 20201031,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200514,
                        ItemId = 20201032,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200515,
                        ItemId = 20201033,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200516,
                        ItemId = 20201034,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200517,
                        ItemId = 20201035,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200518,
                        ItemId = 21101001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200519,
                        ItemId = 20802005,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202005,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                }
            };

            shops.Add(slimecapsuleseries4);

            ShopMetadata slimecapsuleseries5 = new ShopMetadata()
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
                        UniqueId = 10200600,
                        ItemId = 40410013,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200601,
                        ItemId = 40410014,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200602,
                        ItemId = 50610007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200603,
                        ItemId = 50620007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200604,
                        ItemId = 70401006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200605,
                        ItemId = 70501006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200606,
                        ItemId = 70601007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200607,
                        ItemId = 70711007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200608,
                        ItemId = 70310007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200609,
                        ItemId = 70210007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200610,
                        ItemId = 20211007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 25,
                        ItemRank = 4,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200611,
                        ItemId = 20201043,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200612,
                        ItemId = 20201044,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200613,
                        ItemId = 20201045,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200614,
                        ItemId = 20201046,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200615,
                        ItemId = 20201047,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200616,
                        ItemId = 20201048,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200617,
                        ItemId = 20201049,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200618,
                        ItemId = 21101003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200619,
                        ItemId = 20802007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202006,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                }
            };

            shops.Add(slimecapsuleseries5);

            ShopMetadata slimecapsuleseries6 = new ShopMetadata()
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
                        UniqueId = 10200700,
                        ItemId = 40410011,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200701,
                        ItemId = 40410012,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200702,
                        ItemId = 50610006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200703,
                        ItemId = 50620006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200704,
                        ItemId = 70401007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200705,
                        ItemId = 70501007,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200706,
                        ItemId = 70601006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200707,
                        ItemId = 70711006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200708,
                        ItemId = 70310006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200709,
                        ItemId = 70210006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200710,
                        ItemId = 20211006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 25,
                        ItemRank = 4,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200711,
                        ItemId = 20201036,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200712,
                        ItemId = 20201037,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200713,
                        ItemId = 20201038,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200714,
                        ItemId = 20201039,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200715,
                        ItemId = 20201040,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200716,
                        ItemId = 20201041,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200717,
                        ItemId = 20201042,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200718,
                        ItemId = 21101002,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200719,
                        ItemId = 20802006,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202007,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                }
            };

            shops.Add(slimecapsuleseries6);

            ShopMetadata slimecapsuleseries7 = new ShopMetadata()
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
                        UniqueId = 10200800,
                        ItemId = 40410015,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200801,
                        ItemId = 40410016,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 50,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200802,
                        ItemId = 50610008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200803,
                        ItemId = 50620008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 40,
                        SalePrice = 0,
                        ItemRank = 4,
                        Category = "Ride",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200804,
                        ItemId = 70401008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200805,
                        ItemId = 70501008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200806,
                        ItemId = 70601008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200807,
                        ItemId = 70711008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 10,
                        SalePrice = 0,
                        ItemRank = 3,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200808,
                        ItemId = 70310008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200809,
                        ItemId = 70210008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 8,
                        SalePrice = 0,
                        ItemRank = 1,
                        Category = "Badge",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200810,
                        ItemId = 20211008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 25,
                        ItemRank = 4,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200811,
                        ItemId = 20201050,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200812,
                        ItemId = 20201013,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200813,
                        ItemId = 20201051,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200814,
                        ItemId = 20201052,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200815,
                        ItemId = 20201053,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200816,
                        ItemId = 20201054,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200817,
                        ItemId = 20201055,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 4,
                        ItemRank = 1,
                        Category = "Action",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200818,
                        ItemId = 21101004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                    new ShopItem
                    {
                        UniqueId = 10200819,
                        ItemId = 20802008,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22202008,
                        Price = 2,
                        ItemRank = 2,
                        Category = "ETC",
                        Quantity = 1,
                        TokenName = "22202001",
                    },
                }
            };

            shops.Add(slimecapsuleseries7);

            ShopMetadata holyidaycapsule = new ShopMetadata()
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
                        UniqueId = 10420100,
                        ItemId = 11220353,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 35,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420101,
                        ItemId = 11220354,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 35,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420102,
                        ItemId = 11320353,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420103,
                        ItemId = 11320354,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420104,
                        ItemId = 11620353,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420105,
                        ItemId = 11620354,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420106,
                        ItemId = 11720353,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420107,
                        ItemId = 11720354,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420108,
                        ItemId = 11820353,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420109,
                        ItemId = 11820354,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420110,
                        ItemId = 12220353,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420111,
                        ItemId = 12220354,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420112,
                        ItemId = 11220387,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 35,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420113,
                        ItemId = 11220388,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 35,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420114,
                        ItemId = 11320387,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420115,
                        ItemId = 11320388,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420116,
                        ItemId = 11620387,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420117,
                        ItemId = 11620388,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420118,
                        ItemId = 11720387,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420119,
                        ItemId = 11720388,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420120,
                        ItemId = 11820387,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420121,
                        ItemId = 11820388,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420122,
                        ItemId = 12220387,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420123,
                        ItemId = 12220388,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420124,
                        ItemId = 11220361,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 35,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420125,
                        ItemId = 11220362,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 35,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420126,
                        ItemId = 11320361,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420127,
                        ItemId = 11320361,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420128,
                        ItemId = 11620361,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420129,
                        ItemId = 11620362,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420130,
                        ItemId = 11720361,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420131,
                        ItemId = 11720362,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420132,
                        ItemId = 11820361,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420133,
                        ItemId = 11820362,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420134,
                        ItemId = 12220361,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420135,
                        ItemId = 12220362,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420136,
                        ItemId = 70910001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 25,
                        ItemRank = 4,
                        Category = "Other",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420137,
                        ItemId = 20710001,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 28,
                        ItemRank = 4,
                        Category = "Other",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420138,
                        ItemId = 20303116,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 36,
                        ItemRank = 4,
                        Category = "Other",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420139,
                        ItemId = 20303117,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 36,
                        ItemRank = 4,
                        Category = "Other",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420140,
                        ItemId = 34000056,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Other",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420141,
                        ItemId = 34000087,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204201,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Other",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                }
            };

            shops.Add(holyidaycapsule);

            ShopMetadata valentinescapsule = new ShopMetadata()
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
                        UniqueId = 10420200,
                        ItemId = 11220355,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 35,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420201,
                        ItemId = 11220356,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 35,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420202,
                        ItemId = 11320355,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420203,
                        ItemId = 11320356,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420204,
                        ItemId = 11620355,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420205,
                        ItemId = 11620356,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420206,
                        ItemId = 11720355,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420207,
                        ItemId = 11720356,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420208,
                        ItemId = 11820355,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420209,
                        ItemId = 11820356,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420210,
                        ItemId = 12220355,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420211,
                        ItemId = 12220356,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary01",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420212,
                        ItemId = 11220357,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 35,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420213,
                        ItemId = 11220358,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 35,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420214,
                        ItemId = 11320357,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420215,
                        ItemId = 11320358,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420216,
                        ItemId = 11620357,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420217,
                        ItemId = 11620358,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420218,
                        ItemId = 11720357,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420219,
                        ItemId = 11720358,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420220,
                        ItemId = 11820357,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420221,
                        ItemId = 11820358,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420222,
                        ItemId = 12220357,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420223,
                        ItemId = 12220358,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary02",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420226,
                        ItemId = 11320419,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420227,
                        ItemId = 11320420,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 36,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420228,
                        ItemId = 11620419,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420229,
                        ItemId = 11620420,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420230,
                        ItemId = 11720419,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420231,
                        ItemId = 11720420,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420232,
                        ItemId = 11820419,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420233,
                        ItemId = 11820420,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 38,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420234,
                        ItemId = 12220419,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420235,
                        ItemId = 12220420,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 56,
                        ItemRank = 5,
                        Category = "Legendary03",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420236,
                        ItemId = 70910004,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 25,
                        ItemRank = 4,
                        Category = "Other",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420237,
                        ItemId = 20710003,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 28,
                        ItemRank = 4,
                        Category = "Other",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420240,
                        ItemId = 34000098,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Other",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420241,
                        ItemId = 34000092,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 24,
                        ItemRank = 3,
                        Category = "Other",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420238,
                        ItemId = 20303087,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 36,
                        ItemRank = 4,
                        Category = "Other",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                    new ShopItem
                    {
                        UniqueId = 10420239,
                        ItemId = 20303162,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 22204202,
                        Price = 36,
                        ItemRank = 4,
                        Category = "Other",
                        Quantity = 1,
                        TokenName = "22204001",
                        AutoPreviewEquip = true
                    },
                }
            };

            shops.Add(valentinescapsule);

            return shops;
        }
    }
}
