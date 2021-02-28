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

            return shops;
        }
    }
}
