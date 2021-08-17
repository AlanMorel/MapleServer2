using Maple2Storage.Types;

namespace MapleServer2.Database.Types
{
    public class MeretMarketItem
    {
        public int MarketId;
        public MeretMarketCategory Category;
        public string ItemName = "";
        public int ItemId;
        public byte Rarity;
        public int Quantity;
        public int BonusQuantity;
        public MeretMarketItemFlag Flag;
        public MeretMarketCurrencyType TokenType;
        public long Price;
        public long SalePrice;
        public int Duration; // in days
        public long SellBeginTime;
        public long SellEndTime;
        public MeretMarketJobRequirement JobRequirement;
        public short MinLevelRequirement;
        public short MaxLevelRequirement;
        public int RequiredAchievementId;
        public int RequiredAchievementGrade;
        public bool PCCafe = false;
        public bool RestockUnavailable = false;
        public int ParentMarketId;
        public Banner Banner;
        public string PromoName = "";
        public MeretMarketPromoFlag PromoFlag;
        public bool ShowSaleTime;
        public long PromoBannerBeginTime; // time when you can buy (buy banner still up)
        public long PromoBannerEndTime; // time when you can't buy (expiration. banner still up)
        public List<MeretMarketItem> AdditionalQuantities = new List<MeretMarketItem>();

        public MeretMarketItem() { }

    }
}
