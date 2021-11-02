using Maple2Storage.Enums;

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
        public long BannerId;
        public Banner Banner;
        public string PromoName = "";
        public MeretMarketPromoFlag PromoFlag;
        public bool ShowSaleTime;
        public long PromoBannerBeginTime; // time when you can buy (buy banner still up)
        public long PromoBannerEndTime; // time when you can't buy (expiration. banner still up)
        public List<MeretMarketItem> AdditionalQuantities = new List<MeretMarketItem>();

        public MeretMarketItem() { }

        public MeretMarketItem(int marketId, long bannerId, int bonusQuantity, int category, int duration, byte flag, int itemId, string itemName, int jobRequirement, short maxLevelRequirement, short minLevelRequirement, bool pcCafe, int parentMarketId, long price, long promoBannerBeginTime, long promoBannerEndTime, int promoFlag, string promoName, int quantity, byte rarity, int requiredAchievementGrade, int requiredAchievementId, bool restockUnavailable, long salePrice, long sellBeginTime, long sellEndTime, bool showSaleTime, byte tokenType)
        {
            MarketId = marketId;
            BannerId = bannerId;
            BonusQuantity = bonusQuantity;
            Category = (MeretMarketCategory) category;
            Duration = duration;
            Flag = (MeretMarketItemFlag) flag;
            ItemId = itemId;
            ItemName = itemName;
            JobRequirement = (MeretMarketJobRequirement) jobRequirement;
            MaxLevelRequirement = maxLevelRequirement;
            MinLevelRequirement = minLevelRequirement;
            PCCafe = pcCafe;
            ParentMarketId = parentMarketId;
            Price = price;
            PromoBannerBeginTime = promoBannerBeginTime;
            PromoBannerEndTime = promoBannerEndTime;
            PromoFlag = (MeretMarketPromoFlag) promoFlag;
            PromoName = promoName;
            Quantity = quantity;
            Rarity = rarity;
            RequiredAchievementGrade = requiredAchievementGrade;
            RequiredAchievementId = requiredAchievementId;
            RestockUnavailable = restockUnavailable;
            SalePrice = salePrice;
            SellBeginTime = sellBeginTime;
            SellEndTime = sellEndTime;
            ShowSaleTime = showSaleTime;
            TokenType = (MeretMarketCurrencyType) tokenType;
        }
    }
}
