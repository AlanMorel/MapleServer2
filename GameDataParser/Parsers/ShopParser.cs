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
                        UniqueId = 161251161,
                        ItemId = 31000145,
                        TokenType = ShopCurrencyType.HaviFruit,
                        Price = 51200,
                        SalePrice = 0,
                        ItemRank = 0,
                        Category = "HabiEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        UniqueId = 161251162,
                        ItemId = 31000146,
                        TokenType = ShopCurrencyType.HaviFruit,
                        Price = 76800,
                        SalePrice = 0,
                        ItemRank = 0,
                        Category = "HabiEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        UniqueId = 161251163,
                        ItemId = 20000521,
                        TokenType = ShopCurrencyType.HaviFruit,
                        Price = 25600,
                        SalePrice = 0,
                        ItemRank = 0,
                        Category = "HabiEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        UniqueId = 161251164,
                        ItemId = 20000522,
                        TokenType = ShopCurrencyType.HaviFruit,
                        Price = 25600,
                        SalePrice = 0,
                        ItemRank = 0,
                        Category = "HabiEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        UniqueId = 161251165,
                        ItemId = 20000523,
                        TokenType = ShopCurrencyType.HaviFruit,
                        Price = 25600,
                        SalePrice = 0,
                        ItemRank = 0,
                        Category = "HabiEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        UniqueId = 161251166,
                        ItemId = 20000524,
                        TokenType = ShopCurrencyType.HaviFruit,
                        Price = 25600,
                        SalePrice = 0,
                        ItemRank = 0,
                        Category = "HabiEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        UniqueId = 161251167,
                        ItemId = 20000525,
                        TokenType = ShopCurrencyType.HaviFruit,
                        Price = 25600,
                        SalePrice = 0,
                        ItemRank = 0,
                        Category = "HabiEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        UniqueId = 161251168,
                        ItemId = 20000597,
                        TokenType = ShopCurrencyType.HaviFruit,
                        Price = 1280000,
                        SalePrice = 0,
                        ItemRank = 0,
                        Category = "HabiEtc",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        UniqueId = 161251169,
                        ItemId = 11600806,
                        TokenType = ShopCurrencyType.HaviFruit,
                        Price = 204800,
                        SalePrice = 0,
                        ItemRank = 0,
                        Category = "HabiEquip",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        UniqueId = 161251170,
                        ItemId = 11600807,
                        TokenType = ShopCurrencyType.HaviFruit,
                        Price = 204800,
                        SalePrice = 0,
                        ItemRank = 0,
                        Category = "HabiEquip",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        UniqueId = 161251171,
                        ItemId = 11600808,
                        TokenType = ShopCurrencyType.HaviFruit,
                        Price = 204800,
                        SalePrice = 0,
                        ItemRank = 0,
                        Category = "HabiEquip",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        UniqueId = 161251172,
                        ItemId = 11600809,
                        TokenType = ShopCurrencyType.HaviFruit,
                        Price = 204800,
                        SalePrice = 0,
                        ItemRank = 0,
                        Category = "HabiEquip",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        UniqueId = 161251173,
                        ItemId = 11600810,
                        TokenType = ShopCurrencyType.HaviFruit,
                        Price = 204800,
                        SalePrice = 0,
                        ItemRank = 0,
                        Category = "HabiEquip",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        UniqueId = 161251174,
                        ItemId = 11600801,
                        TokenType = ShopCurrencyType.HaviFruit,
                        Price = 1536000,
                        SalePrice = 0,
                        ItemRank = 0,
                        Category = "HabiEquip",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        UniqueId = 161251175,
                        ItemId = 11600802,
                        TokenType = ShopCurrencyType.HaviFruit,
                        Price = 1536000,
                        SalePrice = 0,
                        ItemRank = 0,
                        Category = "HabiEquip",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        UniqueId = 161251176,
                        ItemId = 11600803,
                        TokenType = ShopCurrencyType.HaviFruit,
                        Price = 1536000,
                        SalePrice = 0,
                        ItemRank = 0,
                        Category = "HabiEquip",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        UniqueId = 161251177,
                        ItemId = 11600804,
                        TokenType = ShopCurrencyType.HaviFruit,
                        Price = 1536000,
                        SalePrice = 0,
                        ItemRank = 0,
                        Category = "HabiEquip",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        UniqueId = 161251178,
                        ItemId = 11600805,
                        TokenType = ShopCurrencyType.HaviFruit,
                        Price = 1536000,
                        SalePrice = 0,
                        ItemRank = 0,
                        Category = "HabiEquip",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        UniqueId = 161251179,
                        ItemId = 11300423,
                        TokenType = ShopCurrencyType.HaviFruit,
                        Price = 1792000,
                        SalePrice = 0,
                        ItemRank = 0,
                        Category = "HabiSkin",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        UniqueId = 161251180,
                        ItemId = 50600115,
                        TokenType = ShopCurrencyType.HaviFruit,
                        Price = 1536000,
                        SalePrice = 0,
                        ItemRank = 0,
                        Category = "HabiMount",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    },
                    new ShopItem
                    {
                        UniqueId = 161251181,
                        ItemId = 11304766,
                        TokenType = ShopCurrencyType.HaviFruit,
                        Price = 3072000,
                        SalePrice = 0,
                        ItemRank = 0,
                        Category = "HabiSkin",
                        Quantity = 1,
                        Flag = ShopItemFlag.None
                    }
                }
            };

            shops.Add(lazy);

            return shops;
        }
    }
}
