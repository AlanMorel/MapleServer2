using MapleServer2.Database;
using MapleServer2.Enums;
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
                AddListing(listing);
            }
        }

        public void AddListing(BlackMarketListing listing) => Listings.Add(listing.Id, listing);

        public void RemoveListing(BlackMarketListing listing) => Listings.Remove(listing.Id);

        public List<BlackMarketListing> GetListingsByCharacterId(long characterId) => Listings.Values.Where(b => b.OwnerCharacterId == characterId).ToList();

        public BlackMarketListing GetListingByItemUid(long uid) => Listings.Values.FirstOrDefault(b => b.Item.Uid == uid);

        public BlackMarketListing GetListingById(long listingId) => Listings.Values.FirstOrDefault(x => x.Id == listingId);

        public List<BlackMarketListing> GetSearchedListings(List<string> itemCategories, int minLevel, int maxLevel, int rarity, string name, JobFlag jobFlag,
            int minEnchantLevel, int maxEnchantLevel, byte minSockets, byte maxSockets, int startPage, long sort)
        {
            List<BlackMarketListing> allResults = new List<BlackMarketListing>();
            foreach (BlackMarketListing listing in Listings.Values)
            {
                Item item = listing.Item;

                if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() > listing.ExpiryTimestamp ||
                    !itemCategories.Contains(item.BlackMarketCategory) ||
                    item.Level < minLevel ||
                    item.Level > maxLevel ||
                    item.Rarity < rarity ||
                    item.Enchants < minEnchantLevel ||
                    item.Enchants > maxEnchantLevel ||
                    item.Stats.GemSockets.Count < minSockets ||
                    item.Stats.GemSockets.Count > maxSockets ||
                    !item.Name.ToLower().Contains(name))
                {
                    continue;
                }

                // Check job
                if (item.RecommendJobs != null)
                {
                    if (jobFlag != JobFlag.All)
                    {
                        // Doing a switch on this because Black Market does not allow you select multiple jobs
                        Job job = jobFlag switch
                        {
                            JobFlag.Beginner => Job.Beginner,
                            JobFlag.Knight => Job.Knight,
                            JobFlag.Berserker => Job.Berserker,
                            JobFlag.Wizard => Job.Wizard,
                            JobFlag.Priest => Job.Priest,
                            JobFlag.Archer => Job.Archer,
                            JobFlag.HeavyGunner => Job.HeavyGunner,
                            JobFlag.Thief => Job.Thief,
                            JobFlag.Assassin => Job.Assassin,
                            JobFlag.Runeblade => Job.Runeblade,
                            JobFlag.Striker => Job.Striker,
                            JobFlag.SoulBinder => Job.SoulBinder,
                            _ => Job.None,
                        };
                        if (!item.RecommendJobs.Contains(job))
                        {
                            continue;
                        }
                    }
                }

                allResults.Add(listing);

            }
            int count = startPage * 7 - 7;
            int offset = count;
            int limit = 70 + Math.Min(0, count);
            List<BlackMarketListing> results = allResults.Skip(offset).Take(limit).ToList();

            switch (sort)
            {
                case 21: // low to high price
                    results = results.OrderBy(x => x.Price).ToList();
                    break;
                case 22: // high to low price
                    results = results.OrderByDescending(x => x.Price).ToList();
                    break;
                case 11: // low to high level
                    results = results.OrderBy(x => x.Item.Level).ToList();
                    break;
                case 12: // high to low level
                    results = results.OrderByDescending(x => x.Item.Level).ToList();
                    break;
            }

            return results;
        }
    }
}
