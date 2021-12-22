using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class UGCMarketItem
{
    public long Id;
    public long Price;
    public Item Item;
    public UGCMarketListingStatus Status;
    public long CreationTimestamp;
    public long ListingExpirationTimestamp;
    public long PromotionExpirationTimestamp;
    public long SellerAccountId;
    public long SellerCharacterId;
    public string SellerCharacterName;
    public string Description;
    public int SalesCount;
    public List<string> Tags = new();

    public UGCMarketItem() { }

    public UGCMarketItem(Item item, long price, Player player, List<string> tags, string description, bool promote)
    {
        Item = item;
        Price = price;
        Status = UGCMarketListingStatus.Active;
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
        Id = DatabaseManager.UGCMarketItems.Insert(this);
        GameServer.UGCMarketManager.AddListing(this);
    }

    public UGCMarketItem(long id, long price, Item item, UGCMarketListingStatus status, long creationTimestamp, long listingExpirationTimestamp, long promotionExpirationTimestamp,
        long sellerAccountId, long sellerCharacterId, string sellerCharacterName, string description, int salesCount, List<string> tags)
    {
        Id = id;
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
