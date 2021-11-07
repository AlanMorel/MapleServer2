using MapleServer2.Database;
using MapleServer2.Types;

namespace MapleServer2.Managers;

public class MesoMarketManager
{
    private readonly Dictionary<long, MesoMarketListing> Listings;

    public MesoMarketManager()
    {
        Listings = new();
        List<MesoMarketListing> list = DatabaseManager.MesoMarketListings.FindAll();
        foreach (MesoMarketListing listing in list)
        {
            AddListing(listing);
        }
    }

    public void AddListing(MesoMarketListing listing)
    {
        Listings.Add(listing.Id, listing);
    }

    public void RemoveListing(MesoMarketListing listing)
    {
        Listings.Remove(listing.Id);
    }

    public List<MesoMarketListing> GetListingsByAccountId(long accountId)
    {
        return Listings.Values.Where(b => b.OwnerAccountId == accountId).ToList();
    }

    public MesoMarketListing GetListingById(long listingId)
    {
        return Listings.Values.FirstOrDefault(x => x.Id == listingId);
    }

    public List<MesoMarketListing> GetSearchedListings(long minMesoRange, long maxMesoRange)
    {
        return Listings.Values.Where(
            x => x.Mesos >= minMesoRange &&
            x.Mesos <= maxMesoRange &&
            DateTimeOffset.UtcNow.ToUnixTimeSeconds() < x.ExpiryTimestamp)
            .ToList();
    }
}
