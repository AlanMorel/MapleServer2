using Maple2Storage.Enums;

namespace MapleServer2.Database.Types
{
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
