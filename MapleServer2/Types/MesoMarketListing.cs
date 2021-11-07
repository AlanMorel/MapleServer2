using MapleServer2.Database;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class MesoMarketListing
{
    public long Id;
    public long OwnerAccountId;
    public long OwnerCharacterId;
    public long ListedTimestamp;
    public long ExpiryTimestamp;
    public long Price;
    public long Mesos;
    public MesoMarketListing() { }

    public MesoMarketListing(Player player, long price, long mesos)
    {
        OwnerAccountId = player.AccountId;
        OwnerCharacterId = player.CharacterId;
        ListedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        ExpiryTimestamp = ListedTimestamp + 172800; // 2 days TODO: Change to value from constant.xml
        Price = price;
        Mesos = mesos;
        Id = DatabaseManager.MesoMarketListings.Insert(this);
        GameServer.MesoMarketManager.AddListing(this);
    }

    public MesoMarketListing(long id, long ownerAccountId, long ownerCharacterId, long listedTimestamp, long expiryTimestamp, long price, long mesos)
    {
        Id = id;
        OwnerAccountId = ownerAccountId;
        OwnerCharacterId = ownerCharacterId;
        ListedTimestamp = listedTimestamp;
        ExpiryTimestamp = expiryTimestamp;
        Price = price;
        Mesos = mesos;
    }
}
