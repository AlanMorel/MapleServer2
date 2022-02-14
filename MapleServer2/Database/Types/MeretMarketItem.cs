using Maple2Storage.Enums;
using MapleServer2.Enums;

namespace MapleServer2.Database.Types;

public class MeretMarketItem
{
    public readonly int MarketId;
    public readonly MeretMarketCategory Category;
    public readonly string ItemName = "";
    public readonly int ItemId;
    public readonly byte Rarity;
    public readonly int Quantity;
    public readonly int BonusQuantity;
    public readonly MeretMarketItemFlag Flag;
    public readonly MeretMarketCurrencyType TokenType;
    public readonly long Price;
    public readonly long SalePrice;
    public readonly int Duration; // in days
    public readonly long SellBeginTime;
    public readonly long SellEndTime;
    public readonly JobFlag JobRequirement;
    public readonly short MinLevelRequirement;
    public readonly short MaxLevelRequirement;
    public readonly int RequiredAchievementId;
    public readonly int RequiredAchievementGrade;
    public readonly bool PCCafe;
    public readonly bool RestockUnavailable;
    public readonly int ParentMarketId;
    public readonly long BannerId;
    public Banner Banner;
    public readonly string PromoName = "";
    public readonly MeretMarketPromoFlag PromoFlag;
    public readonly bool ShowSaleTime;
    public readonly long PromoBannerBeginTime; // time when you can buy (buy banner still up)
    public readonly long PromoBannerEndTime; // time when you can't buy (expiration. banner still up)
    public readonly List<MeretMarketItem> AdditionalQuantities;

    public MeretMarketItem(dynamic data)
    {
        MarketId = data.market_id;
        BannerId = data.banner_id ?? 0;
        BonusQuantity = data.bonus_quantity;
        Category = (MeretMarketCategory) data.category;
        Duration = data.duration;
        Flag = (MeretMarketItemFlag) data.flag;
        ItemId = data.item_id;
        ItemName = data.item_name;
        JobRequirement = (JobFlag) data.job_requirement;
        MaxLevelRequirement = data.max_level_requirement;
        MinLevelRequirement = data.min_level_requirement;
        PCCafe = data.pc_cafe;
        ParentMarketId = data.parent_market_id;
        Price = data.price;
        PromoBannerBeginTime = data.promo_banner_begin_time;
        PromoBannerEndTime = data.promo_banner_end_time;
        PromoFlag = (MeretMarketPromoFlag) data.promo_flag;
        PromoName = data.promo_name;
        Quantity = data.quantity;
        Rarity = data.rarity;
        RequiredAchievementGrade = data.required_achievement_grade;
        RequiredAchievementId = data.required_achievement_id;
        RestockUnavailable = data.restock_unavailable;
        SalePrice = data.sale_price;
        SellBeginTime = data.sell_begin_time;
        SellEndTime = data.sell_end_time;
        ShowSaleTime = data.show_sale_time;
        TokenType = (MeretMarketCurrencyType) data.token_type;
        AdditionalQuantities = new();
    }
}
