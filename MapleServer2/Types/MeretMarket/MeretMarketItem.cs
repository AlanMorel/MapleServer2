using Maple2Storage.Enums;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Enums;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public abstract class MeretMarketItem
{
    public long MarketId;
    public long Price;
    public int SalesCount;
    public long CreationTimestamp;
}

public class PremiumMarketItem : MeretMarketItem
{
    public readonly MeretMarketCategory Category;
    public readonly MeretMarketSection Section;
    public readonly string ItemName = "";
    public readonly int ItemId;
    public readonly byte Rarity;
    public readonly int Quantity;
    public readonly int BonusQuantity;
    public readonly MeretMarketItemFlag Flag;
    public readonly MeretMarketCurrencyType TokenType;
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
    public readonly bool IsPromo;
    public readonly string PromoName = "";
    public readonly MeretMarketPromoFlag PromoFlag;
    public readonly bool ShowSaleTime;
    public readonly long PromoBannerBeginTime; // time when you can buy (buy banner still up)
    public readonly long PromoBannerEndTime; // time when you can't buy (expiration. banner still up)
    public readonly List<PremiumMarketItem> AdditionalQuantities;

    public PremiumMarketItem(dynamic data)
    {
        MarketId = data.market_id;
        Section = (MeretMarketSection) data.section;
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
        PromoBannerBeginTime = data.promo_banner_begin_time;
        PromoBannerEndTime = data.promo_banner_end_time;
        PromoFlag = (MeretMarketPromoFlag) data.promo_flag;
        PromoName = data.promo_name;
        Quantity = data.quantity;
        Rarity = data.rarity;
        RequiredAchievementGrade = data.required_achievement_grade;
        RequiredAchievementId = data.required_achievement_id;
        RestockUnavailable = data.restock_unavailable;
        Price = data.price;
        SalePrice = data.sale_price;
        SellBeginTime = data.sell_begin_time;
        SellEndTime = data.sell_end_time;
        ShowSaleTime = data.show_sale_time;
        TokenType = (MeretMarketCurrencyType) data.token_type;
        SalesCount = data.sales_count;
        CreationTimestamp = data.creation_time;
        AdditionalQuantities = new();
    }
}

public class UgcMarketItem : MeretMarketItem
{
    public Item Item;
    public UgcMarketListingStatus Status;
    public long ListingExpirationTimestamp;
    public long PromotionExpirationTimestamp;
    public long SellerAccountId;
    public long SellerCharacterId;
    public string SellerCharacterName;
    public string Description;
    public List<string> Tags = new();

    public UgcMarketItem(Item item, long price, Player player, List<string> tags, string description, bool promote)
    {
        Item = item;
        Price = price;
        Status = UgcMarketListingStatus.Active;
        SellerAccountId = player.AccountId;
        SellerCharacterId = player.CharacterId;
        SellerCharacterName = player.Name;
        Tags = tags;
        Description = description;
        CreationTimestamp = TimeInfo.Now();
        ListingExpirationTimestamp = long.Parse(ConstantsMetadataStorage.GetConstant("UGCShopSaleDay")) * 86400 + TimeInfo.Now();
        if (promote)
        {
            PromotionExpirationTimestamp = long.Parse(ConstantsMetadataStorage.GetConstant("UGCShopAdHour")) * 3600 + ListingExpirationTimestamp;
        }
        MarketId = DatabaseManager.UgcMarketItems.Insert(this);
        GameServer.UgcMarketManager.AddListing(this);
    }

    public UgcMarketItem(long id, long price, Item item, UgcMarketListingStatus status, long creationTimestamp, long listingExpirationTimestamp, long promotionExpirationTimestamp,
        long sellerAccountId, long sellerCharacterId, string sellerCharacterName, string description, int salesCount, List<string> tags)
    {
        MarketId = id;
        Price = price;
        Item = item;
        Status = status;
        CreationTimestamp = creationTimestamp;
        ListingExpirationTimestamp = listingExpirationTimestamp;
        PromotionExpirationTimestamp = promotionExpirationTimestamp;
        SellerAccountId = sellerAccountId;
        SellerCharacterId = sellerCharacterId;
        SellerCharacterName = sellerCharacterName;
        Description = description;
        SalesCount = salesCount;
        Tags = tags;
    }
}
