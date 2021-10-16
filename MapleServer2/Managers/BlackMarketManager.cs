using MapleServer2.Database;
using MapleServer2.Types;

namespace MapleServer2.Managers
{
    public class BlackMarketManager
    {
        private readonly Dictionary<long, BlackMarketListing> Listings;

        public BlackMarketManager()
        {
            Listings = new Dictionary<long, BlackMarketListing>();
            List<BlackMarketListing> list = DatabaseManager.BlackMarketListings.FindAll();
            foreach (BlackMarketListing listing in list)
            {
                if (listing.ExpiryTimestamp < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                {
                    RemoveListing(listing);
                    DatabaseManager.BlackMarketListings.Delete(listing.Id);
                    continue;
                }

                AddListing(listing);
            }
        }

        public void AddListing(BlackMarketListing listing) => Listings.Add(listing.Id, listing);

        public void RemoveListing(BlackMarketListing listing) => Listings.Remove(listing.Id);

        public List<BlackMarketListing> GetListings(long characterId) => Listings.Values.Where(b => b.OwnerCharacterId == characterId).ToList();

        public List<BlackMarketListing> GetSearchedListings(List<string> itemCategories, int minLevel, int maxLevel, int rarity, string name)
        {
            List<BlackMarketListing> results = new List<BlackMarketListing>();
            foreach (BlackMarketListing listing in Listings.Values)
            {
                Item item = listing.Item;
                if (!itemCategories.Contains(item.BlackMarketCategory))
                {
                    continue;
                }

                if (item.Level < minLevel || 
                    item.Level > maxLevel ||
                    item.Rarity != rarity)
                {
                    continue;
                }

                results.Add(listing);

            }
            return results;
        }
    }
}
