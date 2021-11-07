using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseMesoMarketListing : DatabaseTable
{
    public DatabaseMesoMarketListing() : base("meso_market_listings") { }

    public long Insert(MesoMarketListing listing)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            listing_timestamp = listing.ListedTimestamp,
            expiry_timestamp = listing.ExpiryTimestamp,
            price = listing.Price,
            mesos = listing.Mesos,
            owner_account_id = listing.OwnerAccountId,
            owner_characteR_id = listing.OwnerCharacterId
        });
    }

    public List<MesoMarketListing> FindAll()
    {
        IEnumerable<dynamic> result = QueryFactory.Query(TableName).Get();
        List<MesoMarketListing> listings = new();

        foreach (dynamic data in result)
        {
            listings.Add(ReadListing(data));
        }
        return listings;
    }

    public bool Delete(long id)
    {
        return QueryFactory.Query(TableName).Where("id", id).Delete() == 1;
    }

    private static MesoMarketListing ReadListing(dynamic data)
    {
        return new()
        {
            Id = data.id,
            ListedTimestamp = data.listing_timestamp,
            ExpiryTimestamp = data.expiry_timestamp,
            Price = data.price,
            Mesos = data.mesos,
            OwnerAccountId = data.owner_account_id,
            OwnerCharacterId = data.owner_character_id
        };
    }
}
