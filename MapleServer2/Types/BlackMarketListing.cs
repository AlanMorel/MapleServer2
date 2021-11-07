using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class BlackMarketListing
{
    public long Id;
    public long ListedTimestamp;
    public long ExpiryTimestamp;
    public long Price;
    public Item Item;
    public int ListedQuantity;
    public long OwnerAccountId;
    public long OwnerCharacterId;
    public long Deposit;
    public BlackMarketListing() { }

    public BlackMarketListing(Player player, Item item, int listedQuantity, long price, long deposit)
    {
        ListedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        int sellEndTime = int.Parse(ConstantsMetadataStorage.GetConstant("BlackMarketSellEndDay")) * 86400;
        ExpiryTimestamp = ListedTimestamp + sellEndTime;
        Price = price;
        Deposit = deposit;
        Item = item;
        ListedQuantity = listedQuantity;
        OwnerAccountId = player.AccountId;
        OwnerCharacterId = player.CharacterId;
        Id = DatabaseManager.BlackMarketListings.Insert(this);
        GameServer.BlackMarketManager.AddListing(this);
    }

    public BlackMarketListing(long id, long listedTimestamp, long expiryTimestamp, long price, Item item, int listedQuantity, long ownerAccountId, long ownerCharacterId, long deposit)
    {
        Id = id;
        ListedTimestamp = listedTimestamp;
        ExpiryTimestamp = expiryTimestamp;
        Price = price;
        Item = item;
        ListedQuantity = listedQuantity;
        OwnerCharacterId = ownerAccountId;
        OwnerCharacterId = ownerCharacterId;
        Deposit = deposit;
    }
}
