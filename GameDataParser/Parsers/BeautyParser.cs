using GameDataParser.Files;
using Maple2Storage.Enums;
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
                UniqueId = 17,
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
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450
                    },
                    new BeautyItem
                    {
                        ItemId = 10200002,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200003,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200004,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200005,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200014,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200015,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200016,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200019,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200020,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200023,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200028,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200029,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200034,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200035,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200036,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200037,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200038,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200039,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200041,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200043,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200045,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200046,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200049,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200050,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200051,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200052,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200053,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200062,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200063,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200065,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200068,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200069,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200071,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200073,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200074,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200075,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200077,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200079,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200080,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200081,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200082,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200089,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200090,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200092,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200093,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200097,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200098,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200101,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200104,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200105,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200106,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200116,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200118,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200119,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200120,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200122,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200125,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200130,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200131,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200139,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200141,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200144,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200149,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200150,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200153,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200154,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200155,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200164,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200165,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200166,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200167,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200168,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200169,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200173,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200175,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200177,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200179,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200183,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200185,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200194,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200195,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200196,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200197,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200198,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200200,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200202,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200204,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200206,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200208,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200210,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200212,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200214,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200216,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200218,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200220,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200222,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200225,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200227,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200229,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200233,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200235,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200237,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200239,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200241,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200243,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200245,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200249,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200253,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200255,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200257,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    // female hairs
                    new BeautyItem
                    {
                        ItemId = 10200006,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200007,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200008,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200009,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200010,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200011,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200012,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200013,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200017,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200018,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200021,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200022,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200024,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200025,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200026,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200027,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200030,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200031,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200040,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200042,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200044,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200047,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200048,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200054,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200055,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200056,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200057,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200058,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200059,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200060,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200061,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200064,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200066,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200067,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200070,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200072,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200076,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200078,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200083,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200084,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200085,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200086,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200087,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200088,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200094,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200095,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200096,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200100,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200102,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200107,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200108,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200109,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200110,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200111,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200113,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200114,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200115,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200117,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200123,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200124,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200126,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200128,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200135,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200136,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200137,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200138,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200140,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200145,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200146,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200147,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200148,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200151,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200152,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200157,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200158,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200159,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200160,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200161,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200162,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200163,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200172,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200174,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200176,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200178,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200180,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200184,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200186,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200189,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200190,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200191,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200192,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200193,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200199,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200201,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200203,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200205,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200209,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200211,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200213,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200221,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200223,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200224,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200226,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200228,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200234,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200236,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200240,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200242,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200244,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200256,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 450,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200258,
                        Gender = 1,
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
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200091,
                        Gender = 0,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200127,
                        Gender = 0,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200132,
                        Gender = 0,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200133,
                        Gender = 0,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200129,
                        Gender = 0,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200143,
                        Gender = 0,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200142,
                        Gender = 0,
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
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200156,
                        Gender = 1,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200215,
                        Gender = 1,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200219,
                        Gender = 1,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200238,
                        Gender = 1,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200134,
                        Gender = 1,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200207,
                        Gender = 1,
                        TokenType = ShopCurrencyType.Item,
                        RequiredItemId = 20300246,
                        TokenCost = 15
                    },
                    new BeautyItem
                    {
                        ItemId = 10200217,
                        Gender = 1,
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
                UniqueId = 21,
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
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200091,
                        Gender = 0,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200127,
                        Gender = 0,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200132,
                        Gender = 0,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200133,
                        Gender = 0,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200129,
                        Gender = 0,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200143,
                        Gender = 0,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200142,
                        Gender = 0,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    // female hairs      
                    new BeautyItem
                    {
                        ItemId = 10200254,
                        Gender = 1,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200232,
                        Gender = 1,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200250,
                        Gender = 1,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200156,
                        Gender = 1,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200215,
                        Gender = 1,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200219,
                        Gender = 1,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200238,
                        Gender = 1,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200134,
                        Gender = 1,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200207,
                        Gender = 1,
                        TokenType = ShopCurrencyType.Meret,
                    },
                    new BeautyItem
                    {
                        ItemId = 10200217,
                        Gender = 1,
                        TokenType = ShopCurrencyType.Meret,
                    }
                }
            };

            shops.Add(paulie);

            // Ren - Makeup
            BeautyMetadata ren = new BeautyMetadata()
            {
                ShopId = 505,
                UniqueId = 15,
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
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 0,
                    },
                    // male makeup
                    new BeautyItem
                    {
                        ItemId = 10400001,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400002,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400003,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400004,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400005,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400006,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400007,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400008,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400009,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400021,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400022,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400027,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400028,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400029,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400030,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400035,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400037,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400039,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400041,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400043,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400045,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400049,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400051,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400053,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400055,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400057,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400059,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400061,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400063,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400065,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400067,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400068,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400071,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400073,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400079,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400081,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400083,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400084,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400085,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400086,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400087,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400089,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400091,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400093,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400095,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400096,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400097,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400101,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400103,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400105,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400107,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400109,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400111,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400113,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400115,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400117,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400119,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400121,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400123,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400125,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400127,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400129,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400131,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400133,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400135,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400137,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400139,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400141,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400143,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400145,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400147,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400149,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400151,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400153,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400155,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400157,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400159,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400161,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400163,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400165,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400167,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400169,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400171,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400173,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400175,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400177,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400179,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400181,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400183,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400184,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400185,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400186,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400188,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400189,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400190,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400191,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400192,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400203,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400205,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400207,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400209,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400211,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400213,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400215,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400221,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400223,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    // female makeup
                    new BeautyItem
                    {
                        ItemId = 10400010,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400011,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400012,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400013,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400014,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400015,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400016,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400017,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400018,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400023,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400025,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400026,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400031,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400032,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400033,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400034,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400036,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400038,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400040,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400042,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400044,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400046,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400050,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400052,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400054,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400056,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400058,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400060,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400062,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400064,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400066,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400069,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400070,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400072,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400074,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400075,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400076,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400077,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400078,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400080,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400082,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400088,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400090,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400092,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400094,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400098,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400099,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400100,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400102,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400104,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400106,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400108,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400110,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400112,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400114,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400116,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400118,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400120,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400122,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400124,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400126,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400128,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400130,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400132,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400134,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400136,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400138,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400140,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400142,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400144,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400146,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400148,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400150,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400152,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400154,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400156,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400158,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400160,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400162,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400164,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400166,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400168,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400170,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400172,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400174,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400176,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400178,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400180,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400182,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400193,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400194,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400195,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400196,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400197,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400199,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400200,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400201,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400202,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400204,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400206,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400208,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400210,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400212,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400214,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400216,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400222,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 50,
                    },
                    new BeautyItem
                    {
                        ItemId = 10400224,
                        Gender = 1,
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
                UniqueId = 16,
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
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300002,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300005,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300007,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300013,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300014,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300015,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300016,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300017,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300019,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300021,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300023,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300025,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300027,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300029,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300031,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300033,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300035,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300037,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300039,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300041,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300043,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300045,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300047,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300049,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300050,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300051,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300054,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300057,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300063,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300065,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300067,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300069,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300071,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300073,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300075,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300077,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300079,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300081,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300083,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300085,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300089,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300091,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300093,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300096,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300098,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300100,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300102,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300104,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300106,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300108,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300110,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300111,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300114,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300116,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300118,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300120,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300122,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300124,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300126,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300128,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300130,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300132,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300134,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300136,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300138,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300140,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300142,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300144,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300148,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300150,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300152,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300154,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300156,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300158,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300160,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300162,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300164,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300166,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300172,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300174,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300176,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300178,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300180,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300182,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300184,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300188,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300190,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300192,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300194,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300196,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300198,
                        Gender = 0,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    // female faces
                    new BeautyItem
                    {
                        ItemId = 10300003,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300004,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300006,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300008,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300009,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300010,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300011,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300012,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300018,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300020,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300022,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300024,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300026,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300028,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300030,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300032,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300034,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300036,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300038,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300040,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300042,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300044,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300046,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300048,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300052,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300053,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300055,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300056,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300058,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300064,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300066,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300068,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300070,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300072,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300074,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300076,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300078,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300080,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300082,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300084,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300086,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300090,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300092,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300094,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300097,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300099,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300101,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300103,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300105,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300107,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300109,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300112,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300113,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300115,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300117,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300119,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300121,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300123,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300125,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300127,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300129,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300131,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300133,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300135,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300137,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300139,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300141,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300143,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300145,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300149,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300151,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300153,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300155,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300157,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300159,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300161,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300163,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300165,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300167,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300173,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300175,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300177,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300179,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300181,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300183,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300185,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300189,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300191,
                        Gender = 1,
                        TokenType = ShopCurrencyType.EventMeret,
                        TokenCost = 330,
                    },
                    new BeautyItem
                    {
                        ItemId = 10300193,
                        Gender = 1,
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
                UniqueId = 19,
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
                UniqueId = 18,
                BeautyCategory = BeautyCategory.Dye,
                BeautyType = BeautyShopType.Dye,
                VoucherId = 20300038,
                TokenType = ShopCurrencyType.Meret,
                TokenCost = 150
            };

            shops.Add(douglas);

            BeautyMetadata voucher601 = new BeautyMetadata()
            {
                ShopId = 601,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200103,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher601);

            BeautyMetadata voucher602 = new BeautyMetadata()
            {
                ShopId = 602,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200091,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher602);

            BeautyMetadata voucher603 = new BeautyMetadata()
            {
                ShopId = 603,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200096,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher603);

            BeautyMetadata voucher604 = new BeautyMetadata()
            {
                ShopId = 604,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200108,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher604);

            BeautyMetadata voucher605 = new BeautyMetadata()
            {
                ShopId = 605,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200109,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher605);

            BeautyMetadata voucher606 = new BeautyMetadata()
            {
                ShopId = 606,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200106,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher606);

            BeautyMetadata voucher607 = new BeautyMetadata()
            {
                ShopId = 607,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200086,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher607);

            BeautyMetadata voucher608 = new BeautyMetadata()
            {
                ShopId = 608,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200112,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher608);

            BeautyMetadata voucher609 = new BeautyMetadata()
            {
                ShopId = 609,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200116,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher609);

            BeautyMetadata voucher610 = new BeautyMetadata()
            {
                ShopId = 610,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200104,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher610);

            BeautyMetadata voucher611 = new BeautyMetadata()
            {
                ShopId = 611,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200113,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher611);

            BeautyMetadata voucher612 = new BeautyMetadata()
            {
                ShopId = 612,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200110,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher612);

            BeautyMetadata voucher613 = new BeautyMetadata()
            {
                ShopId = 613,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200105,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher613);

            BeautyMetadata voucher614 = new BeautyMetadata()
            {
                ShopId = 614,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200120,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher614);

            BeautyMetadata voucher615 = new BeautyMetadata()
            {
                ShopId = 615,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200117,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher615);

            BeautyMetadata voucher616 = new BeautyMetadata()
            {
                ShopId = 616,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200115,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher616);

            BeautyMetadata voucher617 = new BeautyMetadata()
            {
                ShopId = 617,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200125,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher617);

            BeautyMetadata voucher618 = new BeautyMetadata()
            {
                ShopId = 618,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200127,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher618);

            BeautyMetadata voucher619 = new BeautyMetadata()
            {
                ShopId = 619,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200125,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher619);

            BeautyMetadata voucher620 = new BeautyMetadata()
            {
                ShopId = 620,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200128,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher620);

            BeautyMetadata voucher621 = new BeautyMetadata()
            {
                ShopId = 621,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200137,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher621);

            BeautyMetadata voucher622 = new BeautyMetadata()
            {
                ShopId = 622,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200140,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher622);

            BeautyMetadata voucher623 = new BeautyMetadata()
            {
                ShopId = 623,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200139,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher623);

            BeautyMetadata voucher624 = new BeautyMetadata()
            {
                ShopId = 624,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200119,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher624);

            BeautyMetadata voucher626 = new BeautyMetadata()
            {
                ShopId = 626,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200177,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher626);

            BeautyMetadata voucher627 = new BeautyMetadata()
            {
                ShopId = 627,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200178,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher627);

            BeautyMetadata voucher628 = new BeautyMetadata()
            {
                ShopId = 628,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200179,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher628);

            BeautyMetadata voucher629 = new BeautyMetadata()
            {
                ShopId = 629,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200180,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher629);

            BeautyMetadata voucher630 = new BeautyMetadata()
            {
                ShopId = 630,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200183,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher630);

            BeautyMetadata voucher631 = new BeautyMetadata()
            {
                ShopId = 631,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200184,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher631);

            BeautyMetadata voucher632 = new BeautyMetadata()
            {
                ShopId = 632,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200130,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher632);

            BeautyMetadata voucher633 = new BeautyMetadata()
            {
                ShopId = 633,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200132,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher633);

            BeautyMetadata voucher634 = new BeautyMetadata()
            {
                ShopId = 634,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200134,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher634);

            BeautyMetadata voucher635 = new BeautyMetadata()
            {
                ShopId = 635,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200137,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher635);

            BeautyMetadata voucher636 = new BeautyMetadata()
            {
                ShopId = 636,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200114,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher636);

            BeautyMetadata voucher637 = new BeautyMetadata()
            {
                ShopId = 637,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200133,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher637);

            BeautyMetadata voucher638 = new BeautyMetadata()
            {
                ShopId = 638,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200129,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher638);

            BeautyMetadata voucher639 = new BeautyMetadata()
            {
                ShopId = 639,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200111,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher639);

            BeautyMetadata voucher640 = new BeautyMetadata()
            {
                ShopId = 640,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200146,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher640);

            BeautyMetadata voucher641 = new BeautyMetadata()
            {
                ShopId = 641,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200190,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher641);

            BeautyMetadata voucher642 = new BeautyMetadata()
            {
                ShopId = 642,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200191,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher642);

            BeautyMetadata voucher643 = new BeautyMetadata()
            {
                ShopId = 643,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200192,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher643);

            BeautyMetadata voucher644 = new BeautyMetadata()
            {
                ShopId = 644,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200193,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher644);

            BeautyMetadata voucher645 = new BeautyMetadata()
            {
                ShopId = 645,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200194,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher645);

            BeautyMetadata voucher646 = new BeautyMetadata()
            {
                ShopId = 646,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200195,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher646);

            BeautyMetadata voucher647 = new BeautyMetadata()
            {
                ShopId = 647,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200196,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher647);

            BeautyMetadata voucher648 = new BeautyMetadata()
            {
                ShopId = 648,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200197,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher648);

            BeautyMetadata voucher649 = new BeautyMetadata()
            {
                ShopId = 649,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200143,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher649);

            BeautyMetadata voucher650 = new BeautyMetadata()
            {
                ShopId = 650,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200148,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher650);

            BeautyMetadata voucher651 = new BeautyMetadata()
            {
                ShopId = 651,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200099,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher651);

            BeautyMetadata voucher652 = new BeautyMetadata()
            {
                ShopId = 652,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200092,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher652);

            BeautyMetadata voucher653 = new BeautyMetadata()
            {
                ShopId = 653,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200102,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher653);

            BeautyMetadata voucher654 = new BeautyMetadata()
            {
                ShopId = 654,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200107,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher654);

            BeautyMetadata voucher655 = new BeautyMetadata()
            {
                ShopId = 655,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200079,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher655);

            BeautyMetadata voucher656 = new BeautyMetadata()
            {
                ShopId = 656,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200200,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher656);

            BeautyMetadata voucher657 = new BeautyMetadata()
            {
                ShopId = 657,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200109,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher657);

            BeautyMetadata voucher658 = new BeautyMetadata()
            {
                ShopId = 658,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200201,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher658);

            BeautyMetadata voucher671 = new BeautyMetadata()
            {
                ShopId = 671,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200140,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher671);

            BeautyMetadata voucher672 = new BeautyMetadata()
            {
                ShopId = 672,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200139,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher672);

            BeautyMetadata voucher673 = new BeautyMetadata()
            {
                ShopId = 673,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200119,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher673);

            BeautyMetadata voucher659 = new BeautyMetadata()
            {
                ShopId = 659,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200202,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher659);

            BeautyMetadata voucher660 = new BeautyMetadata()
            {
                ShopId = 660,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200194,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher660);

            BeautyMetadata voucher661 = new BeautyMetadata()
            {
                ShopId = 661,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200203,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher661);

            BeautyMetadata voucher662 = new BeautyMetadata()
            {
                ShopId = 662,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200190,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher662);

            BeautyMetadata voucher663 = new BeautyMetadata()
            {
                ShopId = 663,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200196,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher663);

            BeautyMetadata voucher664 = new BeautyMetadata()
            {
                ShopId = 664,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200204,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher664);

            BeautyMetadata voucher665 = new BeautyMetadata()
            {
                ShopId = 665,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200192,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher665);

            BeautyMetadata voucher666 = new BeautyMetadata()
            {
                ShopId = 666,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200205,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher666);

            BeautyMetadata voucher667 = new BeautyMetadata()
            {
                ShopId = 667,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200206,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher667);

            BeautyMetadata voucher668 = new BeautyMetadata()
            {
                ShopId = 668,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200208,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher668);

            BeautyMetadata voucher669 = new BeautyMetadata()
            {
                ShopId = 669,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200209,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher669);

            BeautyMetadata voucher670 = new BeautyMetadata()
            {
                ShopId = 670,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200211,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher670);

            BeautyMetadata voucher690 = new BeautyMetadata()
            {
                ShopId = 690,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200138,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher690);

            BeautyMetadata voucher691 = new BeautyMetadata()
            {
                ShopId = 691,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200156,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher691);

            BeautyMetadata voucher692 = new BeautyMetadata()
            {
                ShopId = 692,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200148,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher692);

            BeautyMetadata voucher693 = new BeautyMetadata()
            {
                ShopId = 693,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200157,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher693);

            BeautyMetadata voucher694 = new BeautyMetadata()
            {
                ShopId = 694,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200098,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher694);

            BeautyMetadata voucher695 = new BeautyMetadata()
            {
                ShopId = 695,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200165,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher695);

            BeautyMetadata voucher696 = new BeautyMetadata()
            {
                ShopId = 696,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200164,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher696);

            BeautyMetadata voucher697 = new BeautyMetadata()
            {
                ShopId = 697,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200131,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher697);

            BeautyMetadata voucher698 = new BeautyMetadata()
            {
                ShopId = 698,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200136,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher698);

            BeautyMetadata voucher699 = new BeautyMetadata()
            {
                ShopId = 699,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200135,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher699);

            BeautyMetadata voucher700 = new BeautyMetadata()
            {
                ShopId = 700,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200207,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher700);

            BeautyMetadata voucher701 = new BeautyMetadata()
            {
                ShopId = 701,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200147,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher701);

            BeautyMetadata voucher702 = new BeautyMetadata()
            {
                ShopId = 702,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200142,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher702);

            BeautyMetadata voucher703 = new BeautyMetadata()
            {
                ShopId = 703,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200210,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher703);

            BeautyMetadata voucher704 = new BeautyMetadata()
            {
                ShopId = 704,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200223,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher704);

            BeautyMetadata voucher705 = new BeautyMetadata()
            {
                ShopId = 705,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200201,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher705);

            BeautyMetadata voucher706 = new BeautyMetadata()
            {
                ShopId = 706,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200205,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher706);

            BeautyMetadata voucher707 = new BeautyMetadata()
            {
                ShopId = 707,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200207,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher707);

            BeautyMetadata voucher708 = new BeautyMetadata()
            {
                ShopId = 708,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200209,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher708);

            BeautyMetadata voucher709 = new BeautyMetadata()
            {
                ShopId = 709,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200211,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher709);

            BeautyMetadata voucher710 = new BeautyMetadata()
            {
                ShopId = 710,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200224,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher710);

            BeautyMetadata voucher711 = new BeautyMetadata()
            {
                ShopId = 711,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200200,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher711);

            BeautyMetadata voucher712 = new BeautyMetadata()
            {
                ShopId = 712,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200204,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher712);

            BeautyMetadata voucher713 = new BeautyMetadata()
            {
                ShopId = 713,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200206,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher713);

            BeautyMetadata voucher714 = new BeautyMetadata()
            {
                ShopId = 714,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200208,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher714);

            BeautyMetadata voucher715 = new BeautyMetadata()
            {
                ShopId = 715,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200210,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher715);

            BeautyMetadata voucher716 = new BeautyMetadata()
            {
                ShopId = 716,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200220,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher716);

            BeautyMetadata voucher717 = new BeautyMetadata()
            {
                ShopId = 717,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200222,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher717);

            BeautyMetadata voucher718 = new BeautyMetadata()
            {
                ShopId = 718,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200157,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher718);

            BeautyMetadata voucher719 = new BeautyMetadata()
            {
                ShopId = 719,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200202,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher719);

            BeautyMetadata voucher720 = new BeautyMetadata()
            {
                ShopId = 720,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200203,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher720);

            BeautyMetadata voucher721 = new BeautyMetadata()
            {
                ShopId = 721,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200222,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher721);

            BeautyMetadata voucher722 = new BeautyMetadata()
            {
                ShopId = 722,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200214,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher722);

            BeautyMetadata voucher723 = new BeautyMetadata()
            {
                ShopId = 723,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200218,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher723);

            BeautyMetadata voucher724 = new BeautyMetadata()
            {
                ShopId = 724,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200223,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher724);

            BeautyMetadata voucher725 = new BeautyMetadata()
            {
                ShopId = 725,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200215,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher725);

            BeautyMetadata voucher726 = new BeautyMetadata()
            {
                ShopId = 726,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200219,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher726);

            BeautyMetadata voucher727 = new BeautyMetadata()
            {
                ShopId = 727,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200154,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher727);

            BeautyMetadata voucher728 = new BeautyMetadata()
            {
                ShopId = 728,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200138,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher728);

            BeautyMetadata voucher729 = new BeautyMetadata()
            {
                ShopId = 729,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200099,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher729);

            BeautyMetadata voucher730 = new BeautyMetadata()
            {
                ShopId = 730,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200116,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher730);

            BeautyMetadata voucher731 = new BeautyMetadata()
            {
                ShopId = 731,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200143,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher731);

            BeautyMetadata voucher732 = new BeautyMetadata()
            {
                ShopId = 732,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200141,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher732);

            BeautyMetadata voucher733 = new BeautyMetadata()
            {
                ShopId = 733,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200144,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher733);

            BeautyMetadata voucher734 = new BeautyMetadata()
            {
                ShopId = 734,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200167,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher734);

            BeautyMetadata voucher735 = new BeautyMetadata()
            {
                ShopId = 735,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200153,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher735);

            BeautyMetadata voucher736 = new BeautyMetadata()
            {
                ShopId = 736,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200168,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher736);

            BeautyMetadata voucher737 = new BeautyMetadata()
            {
                ShopId = 737,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200108,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher737);

            BeautyMetadata voucher738 = new BeautyMetadata()
            {
                ShopId = 738,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200109,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher738);

            BeautyMetadata voucher739 = new BeautyMetadata()
            {
                ShopId = 739,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200159,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher739);

            BeautyMetadata voucher740 = new BeautyMetadata()
            {
                ShopId = 740,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200163,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher740);

            BeautyMetadata voucher741 = new BeautyMetadata()
            {
                ShopId = 741,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200161,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher741);

            BeautyMetadata voucher742 = new BeautyMetadata()
            {
                ShopId = 742,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200198,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher742);

            BeautyMetadata voucher743 = new BeautyMetadata()
            {
                ShopId = 743,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200193,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher743);

            BeautyMetadata voucher744 = new BeautyMetadata()
            {
                ShopId = 744,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200145,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher744);

            BeautyMetadata voucher625 = new BeautyMetadata()
            {
                ShopId = 625,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10200242,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher626);

            BeautyMetadata voucher900 = new BeautyMetadata()
            {
                ShopId = 900,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10400192,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher900);

            BeautyMetadata voucher901 = new BeautyMetadata()
            {
                ShopId = 901,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10400193,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher901);

            BeautyMetadata voucher902 = new BeautyMetadata()
            {
                ShopId = 902,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10400183,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher902);

            BeautyMetadata voucher903 = new BeautyMetadata()
            {
                ShopId = 903,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10400194,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher903);

            BeautyMetadata voucher904 = new BeautyMetadata()
            {
                ShopId = 904,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10400203,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher904);

            BeautyMetadata voucher905 = new BeautyMetadata()
            {
                ShopId = 905,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10400204,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher905);

            BeautyMetadata voucher906 = new BeautyMetadata()
            {
                ShopId = 906,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10400223,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher906);

            BeautyMetadata voucher907 = new BeautyMetadata()
            {
                ShopId = 907,
                BeautyCategory = BeautyCategory.Standard,
                BeautyType = BeautyShopType.Hair,
                TokenType = ShopCurrencyType.Meso,
                Items = new List<BeautyItem>
                {
                    new BeautyItem
                    {
                        ItemId = 10400224,
                        TokenType = ShopCurrencyType.EventMeret,
                    }
                }
            };

            shops.Add(voucher907);

            return shops;
        }
    }
}
