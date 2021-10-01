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

        public ShopItem(int uid, bool auto_preview_equip, string category, byte flag, int guild_trophy, int item_id, byte item_rank, int price, short quantity, int required_achievement_grade, int required_achievement_id, byte required_championship_grade, short required_championship_join_count, int required_fame_grade, short required_guild_merchant_level, byte required_guild_merchant_type, int required_item_id, short required_quest_alliance, int sale_price, int stock_count, int stock_purchased, string template_name, byte token_type)
        {
            Uid = uid;
            AutoPreviewEquip = auto_preview_equip;
            Category = category;
            Flag = (ShopItemFlag) flag;
            GuildTrophy = guild_trophy;
            ItemId = item_id;
            ItemRank = item_rank;
            Price = price;
            RequiredAchievementGrade = required_achievement_grade;
            RequiredAchievementId = required_achievement_id;
            RequiredChampionshipGrade = required_championship_grade;
            RequiredChampionshipJoinCount = required_championship_join_count;
            RequiredFameGrade = required_fame_grade;
            RequiredGuildMerchantLevel = required_guild_merchant_level;
            RequiredGuildMerchantType = required_guild_merchant_type;
            RequiredItemId = required_item_id;
            RequiredQuestAlliance = required_quest_alliance;
            SalePrice = sale_price;
            StockCount = stock_count;
            StockPurchased = stock_purchased;
            TemplateName = template_name;
            TokenType = (ShopCurrencyType) token_type;
            Quantity = quantity;
        }
    }
}
