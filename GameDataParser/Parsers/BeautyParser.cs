using System.Collections.Generic;
using GameDataParser.Files;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class BeautyParser : Exporter<List<BeautyMetadata>>
    {
        public BeautyParser() : base(null, "beauty") { }

        protected override List<BeautyMetadata> Parse()
        {
            List<BeautyMetadata> shops = new List<BeautyMetadata>();

            // Rosetta
            BeautyMetadata rosetta = new BeautyMetadata()
            {
                ShopId = 504,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                VoucherId = 20300035,
                TokenType = ShopCurrencyType.Meso,
                RequiredItemId = 0,
                TokenCost = 20000,
                Items = new List<BeautyItem>
                {
                    // male hairs
                    new BeautyItem
                    {
                        ItemId = 10200001,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450
                    },
                    new BeautyItem
                    {
                        ItemId = 10200002,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200003,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200004,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200005,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200014,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200015,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200016,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200019,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200020,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200023,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200028,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200029,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200034,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200035,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200036,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200037,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200038,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200039,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200041,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200043,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200045,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200046,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200049,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200050,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200051,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200052,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200053,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200062,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200063,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200065,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200068,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200069,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200071,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200073,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200074,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200075,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200077,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200079,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200080,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200081,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200082,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200089,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200090,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200092,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200093,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200097,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200098,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200101,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200104,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200105,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200106,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200116,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200118,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200119,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200120,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200122,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200125,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200130,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200131,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200139,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200141,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200144,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200149,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200150,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200153,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200154,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200155,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200164,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200165,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200166,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200167,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200168,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200169,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200173,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200175,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200177,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200179,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200183,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200185,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200194,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200195,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200196,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200197,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200198,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200200,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200202,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200204,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200206,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200208,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200210,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200212,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200214,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200216,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200218,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200220,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200222,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200225,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200227,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200229,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200233,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200235,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200237,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200239,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200241,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200243,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200245,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200249,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200253,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200255,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200257,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    // female hairs
                    new BeautyItem
                    {
                        ItemId = 10200006,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200007,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200008,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200009,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200010,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200011,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200012,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200013,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200017,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200018,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200021,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200022,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200024,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200025,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200026,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200027,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200030,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200031,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200040,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200042,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200044,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200047,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200048,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200054,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200055,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200056,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200057,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200058,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200059,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200060,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200061,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200064,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200066,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200067,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200070,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200072,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200076,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200078,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200083,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200084,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200085,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200086,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200087,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200088,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200094,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200095,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200096,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200100,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200102,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200107,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200108,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200109,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200110,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200111,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200113,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200114,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200115,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200117,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200123,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200124,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200126,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200128,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200135,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200136,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200137,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200138,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200140,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200145,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200146,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200147,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200148,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200151,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200152,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200157,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200158,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200159,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200160,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200161,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200162,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200163,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200172,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200174,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200176,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200178,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200180,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200184,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200186,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200189,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200190,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200191,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200192,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200193,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200199,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200201,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200203,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200205,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200209,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200211,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200213,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200221,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200223,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200224,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200226,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200228,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200234,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200236,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200240,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200242,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200244,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200256,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200258,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                }
            };
            shops.Add(rosetta);

            // Lolly - Chic Hair
            BeautyMetadata lolly = new BeautyMetadata()
            {
                ShopId = 509,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                VoucherId = 0,
                TokenType = ShopCurrencyType.Item,
                RequiredItemId = 20300246,
                TokenCost = 2,
                Items = new List<BeautyItem>()
                {
                    // male hairs
                    new BeautyItem
                    {
                        ItemId = 10200121,
                        Gender = 0,
                        Flag = ShopItemFlag.New,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200103,
                        Gender = 0,
                        Flag = ShopItemFlag.New,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200099,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200091,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200127,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200132,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200133,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200129,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200143,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200142,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    // female hairs      
                    new BeautyItem
                    {
                        ItemId = 10200254,
                        Gender = 1,
                        Flag = ShopItemFlag.New,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200232,
                        Gender = 1,
                        Flag = ShopItemFlag.New,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200250,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200156,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200215,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200219,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200238,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200134,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200207,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200217,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                }
            };

            shops.Add(lolly);

            // Paulie - Special Hair
            BeautyMetadata paulie = new BeautyMetadata()
            {
                ShopId = 508,
                BeautyCategory = BeautyCategory.Special,
                BeautyType = BeautyShopType.Hair,
                VoucherId = 20300244,
                TokenType = ShopCurrencyType.Meret,
                RequiredItemId = 0,
                TokenCost = 170,
                SpecialCost = 590,
                Items = new List<BeautyItem>()
                {
                    // male hairs
                    new BeautyItem
                    {
                        ItemId = 10200121,
                        Gender = 0,
                        Flag = ShopItemFlag.New,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200103,
                        Gender = 0,
                        Flag = ShopItemFlag.New,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200099,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200091,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200127,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200132,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200133,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200129,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200143,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200142,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    // female hairs      
                    new BeautyItem
                    {
                        ItemId = 10200254,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200232,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200250,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200156,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200215,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200219,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200238,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200134,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200207,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200217,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.Meret,
                    }
                }
            };

            shops.Add(paulie);

            // Ren - Makeup
            BeautyMetadata ren = new BeautyMetadata()
            {
                ShopId = 505,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Makeup,
                VoucherId = 20300244,
                TokenType = ShopCurrencyType.Meso,
                RequiredItemId = 0,
                TokenCost = 5000,
                Items = new List<BeautyItem>()
                {
                    // unisex makeup
                    new BeautyItem
                    {
                        ItemId = 10400000,
                        Gender = 2,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 0,
                    },
                    // male makeup
                    new BeautyItem
                    {
                        ItemId = 10400001,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400002,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400003,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400004,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400005,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400006,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400007,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400008,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400009,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400021,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400022,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400027,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400028,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400029,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400030,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400035,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400037,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400039,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400041,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400043,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400045,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400049,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400051,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400053,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400055,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400057,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400059,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400061,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400063,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400065,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400067,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400068,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400071,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400073,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400079,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400081,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400083,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400084,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400085,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400086,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400087,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400089,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400091,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400093,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400095,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400096,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400097,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400101,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400103,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400105,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400107,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400109,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400111,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400113,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400115,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400117,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400119,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400121,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400123,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400125,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400127,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400129,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400131,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400133,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400135,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400137,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400139,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400141,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400143,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400145,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400147,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400149,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400151,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400153,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400155,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400157,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400159,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400161,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400163,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400165,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400167,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400169,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400171,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400173,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400175,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400177,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400179,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400181,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400183,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400184,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400185,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400186,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400188,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400189,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400190,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400191,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400192,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400203,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400205,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400207,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400209,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400211,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400213,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400215,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400221,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400223,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    // female makeup
                    new BeautyItem
                    {
                        ItemId = 10400010,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400011,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400012,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400013,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400014,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400015,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400016,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400017,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400018,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400023,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400025,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400026,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400031,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400032,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400033,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400034,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400036,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400038,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400040,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400042,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400044,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400046,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400050,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400052,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400054,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400056,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400058,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400060,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400062,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400064,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400066,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400069,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400070,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400072,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400074,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400075,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400076,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400077,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400078,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400080,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400082,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400088,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400090,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400092,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400094,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400098,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400099,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400100,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400102,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400104,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400106,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400108,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400110,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400112,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400114,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400116,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400118,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400120,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400122,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400124,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400126,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400128,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400130,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400132,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400134,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400136,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400138,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400140,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400142,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400144,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400146,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400148,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400150,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400152,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400154,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400156,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400158,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400160,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400162,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400164,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400166,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400168,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400170,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400172,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400174,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400176,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400178,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400180,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400182,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400193,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400194,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400195,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400196,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400197,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400199,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400200,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400201,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400202,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400204,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400206,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400208,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400210,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400212,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400214,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400216,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400222,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400224,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                }
            };

            shops.Add(ren);

            // Dr Dixon
            BeautyMetadata dixon = new BeautyMetadata()
            {
                ShopId = 500,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Face,
                VoucherId = 20300036,
                TokenType = ShopCurrencyType.Meso,
                RequiredItemId = 0,
                TokenCost = 10000,
                Items = new List<BeautyItem>()
                {
                    // male faces
                    new BeautyItem
                    {
                        ItemId = 10300001,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300002,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300005,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300007,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300013,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300014,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300015,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300016,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300017,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300019,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300021,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300023,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300025,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300027,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300029,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300031,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300033,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300035,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300037,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300039,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300041,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300043,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300045,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300047,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300049,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300050,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300051,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300054,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300057,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300063,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300065,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300067,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300069,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300071,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300073,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300075,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300077,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300079,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300081,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300083,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300085,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300089,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300091,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300093,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300096,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300098,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300100,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300102,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300104,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300106,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300108,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300110,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300111,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300114,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300116,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300118,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300120,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300122,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300124,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300126,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300128,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300130,
                        Gender = 0,
                        Flag = ShopItemFlag.New,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300132,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300134,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300136,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300138,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300140,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300142,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300144,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300148,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300150,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300152,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300154,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300156,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300158,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300160,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300162,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300164,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300166,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300172,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300174,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300176,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300178,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300180,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300182,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300184,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300188,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300190,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300192,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300194,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300196,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300198,
                        Gender = 0,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    // female faces
                    new BeautyItem
                    {
                        ItemId = 10300003,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300004,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300006,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300008,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300009,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300010,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300011,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300012,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300018,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300020,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300022,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300024,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300026,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300028,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300030,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300032,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300034,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300036,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300038,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300040,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300042,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300044,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300046,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300048,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300052,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300053,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300055,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300056,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300058,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300064,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300066,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300068,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300070,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300072,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300074,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300076,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300078,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300080,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300082,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300084,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300086,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300090,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300092,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300094,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300097,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300099,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300101,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300103,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300105,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300107,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300109,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300112,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300113,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300115,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300117,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300119,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300121,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300123,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300125,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300127,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300129,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300131,
                        Gender = 1,
                        Flag = ShopItemFlag.New,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300133,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300135,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300137,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300139,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300141,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300143,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300145,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300149,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300151,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300153,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300155,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300157,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300159,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300161,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300163,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300165,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300167,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300173,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300175,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300177,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300179,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300181,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300183,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300185,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300189,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300191,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300193,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300195,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300197,
                        Gender = 1,
                        Flag = ShopItemFlag.None,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                }
            };

            shops.Add(dixon);

            // Dr Zenko = Skin
            BeautyMetadata zenko = new BeautyMetadata()
            {
                ShopId = 501,
                BeautyCategory = BeautyCategory.Dye,
                BeautyType = BeautyShopType.Skin,
                VoucherId = 20300042,
                TokenType = ShopCurrencyType.Meret,
                RequiredItemId = 0,
                TokenCost = 270,
            };

            shops.Add(zenko);

            // Mino - Save Hair
            BeautyMetadata mino = new BeautyMetadata()
            {
                ShopId = 510,
                BeautyCategory = BeautyCategory.Save,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meret,
                SpecialCost = 10,
            };

            shops.Add(mino);

            //Douglas - Dye Workshop
            BeautyMetadata douglas = new BeautyMetadata()
            {
                ShopId = 506,
                BeautyCategory = BeautyCategory.Dye,
                BeautyType = BeautyShopType.Dye,
                VoucherId = 20300038,
                TokenType = ShopCurrencyType.Meret,
                TokenCost = 150
            };

            shops.Add(douglas);

            return shops;
        }
    }
}
