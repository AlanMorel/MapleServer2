using System.Collections.Generic;
using Maple2Storage.Enums;

namespace MapleServer2.Database.Types
{
    public class Shop
    {
        public int Uid { get; set; }
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public int Category { get; set; }
        public string Name { get; set; }
        public ShopType ShopType { get; set; }
        public bool RestrictSales { get; set; }
        public bool CanRestock { get; set; }
        public long NextRestock { get; set; }
        public bool AllowBuyback { get; set; }
        public List<ShopItem> Items { get; set; }

        public Shop() { }
    }

    public class ShopItem
    {
        public int Uid;
        public int ItemId;
        public ShopCurrencyType TokenType;
        public int RequiredItemId;
        public int Price;
        public int SalePrice;
        public byte ItemRank;
        public int StockCount;
        public int StockPurchased;
        public int GuildTrophy;
        public string Category;
        public int RequiredAchievementId;
        public int RequiredAchievementGrade;
        public byte RequiredChampionshipGrade;
        public short RequiredChampionshipJoinCount;
        public byte RequiredGuildMerchantType;
        public short RequiredGuildMerchantLevel;
        public short Quantity;
        public ShopItemFlag Flag;
        public string TemplateName = "";
        public short RequiredQuestAlliance;
        public int RequiredFameGrade;
        public bool AutoPreviewEquip;

        public ShopItem() { }
    }
}
