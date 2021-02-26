using System;
using System.Collections.Generic;
using GameDataParser.Files;
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
                        UniqueId = 400000,
                        ItemId = 30000272,
                        TokenType = Currency.Meso,
                        Price = 500,
                        SalePrice = 1000,
                        ItemRank = 1,
                        Quantity = 1,
                        Category = "ETC",
                        Flag = ShopItemFlag.HalfPrice
                    }
                }
            };

            shops.Add(rumi);

            return shops;
        }
    }
}
