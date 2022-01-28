using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Managers;

public class BlackMarketManager
{
    private readonly Dictionary<long, BlackMarketListing> Listings;

    public BlackMarketManager()
    {
        Listings = new();
        List<BlackMarketListing> list = DatabaseManager.BlackMarketListings.FindAll();
        foreach (BlackMarketListing listing in list)
        {
            listing.Item.SetMetadataValues();
            AddListing(listing);
        }
    }

    public void AddListing(BlackMarketListing listing)
    {
        Listings.Add(listing.Id, listing);
    }

    public void RemoveListing(BlackMarketListing listing)
    {
        Listings.Remove(listing.Id);
    }

    public List<BlackMarketListing> GetListingsByCharacterId(long characterId)
    {
        return Listings.Values.Where(b => b.OwnerCharacterId == characterId).ToList();
    }

    public BlackMarketListing GetListingByItemUid(long uid)
    {
        return Listings.Values.FirstOrDefault(b => b.Item.Uid == uid);
    }

    public BlackMarketListing GetListingById(long listingId)
    {
        return Listings.Values.FirstOrDefault(x => x.Id == listingId);
    }

    public List<BlackMarketListing> GetSearchedListings(List<string> itemCategories, int minLevel, int maxLevel, int rarity, string name, JobFlag jobFlag,
        int minEnchantLevel, int maxEnchantLevel, byte minSockets, byte maxSockets, int startPage, long sort, bool searchStat, List<ItemStat> searchedStats)
    {
        List<BlackMarketListing> allResults = new();
        foreach (BlackMarketListing listing in Listings.Values)
        {
            Item item = listing.Item;

            if (TimeInfo.Now() > listing.ExpiryTimestamp ||
                !itemCategories.Contains(item.BlackMarketCategory) ||
                item.Level < minLevel ||
                item.Level > maxLevel ||
                item.Rarity < rarity ||
                item.Enchants < minEnchantLevel ||
                item.Enchants > maxEnchantLevel ||
                item.Stats.GemSockets.Count < minSockets ||
                item.Stats.GemSockets.Count > maxSockets ||
                !item.Name.ToLower().Contains(name.ToLower()))
            {
                continue;
            }

            // Check job
            if (!JobHelper.CheckJobFlagForJob(item.RecommendJobs, jobFlag))
            {
                continue;
            }

            if (!searchStat)
            {
                allResults.Add(listing);
                continue;
            }

            List<NormalStat> normalStats = new();
            List<SpecialStat> specialStats = new();
            foreach (ItemStat stat in item.Stats.BasicStats)
            {
                if (stat is NormalStat normalStat)
                {
                    normalStats.Add(normalStat);
                    continue;
                }
                specialStats.Add((SpecialStat) stat);
            }

            foreach (ItemStat stat in item.Stats.BonusStats)
            {
                if (stat is NormalStat normalStat)
                {
                    normalStats.Add(normalStat);
                    continue;
                }
                specialStats.Add((SpecialStat) stat);
            }

            // find if stats contains all values inside searchedStats
            bool containsAll = true;
            foreach (ItemStat searchedStat in searchedStats)
            {
                if (searchedStat is NormalStat normalStat)
                {
                    if (!normalStats.Any(x => x.ItemAttribute == normalStat.ItemAttribute && x.Flat >= normalStat.Flat && x.Percent >= normalStat.Percent))
                    {
                        containsAll = false;
                        break;
                    }
                }
                else if (searchedStat is SpecialStat specialStat)
                {
                    if (!specialStats.Any(x => x.ItemAttribute == specialStat.ItemAttribute && x.Flat >= specialStat.Flat && x.Percent >= specialStat.Percent))
                    {
                        containsAll = false;
                        break;
                    }
                }
            }

            if (containsAll)
            {
                allResults.Add(listing);
            }
        }

        BlackMarketSort blackmarketSort = (BlackMarketSort) sort;
        switch (blackmarketSort)
        {
            case BlackMarketSort.PriceLowToHigh:
                allResults = allResults.OrderBy(x => x.Price).ToList();
                break;
            case BlackMarketSort.PriceHighToLow:
                allResults = allResults.OrderByDescending(x => x.Price).ToList();
                break;
            case BlackMarketSort.LevelLowToHigh:
                allResults = allResults.OrderBy(x => x.Item.Level).ToList();
                break;
            case BlackMarketSort.LevelHighToLow:
                allResults = allResults.OrderByDescending(x => x.Item.Level).ToList();
                break;
        }

        int count = startPage * 7 - 7;
        int offset = count;
        int limit = 70 + Math.Min(0, count);

        return allResults.Skip(offset).Take(limit).ToList();
    }

    private enum BlackMarketSort : long
    {
        LevelLowToHigh = 11,
        LevelHighToLow = 12,
        PriceLowToHigh = 21,
        PriceHighToLow = 22
    }
}
