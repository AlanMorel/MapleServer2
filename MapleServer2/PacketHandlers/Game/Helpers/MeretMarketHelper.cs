using Maple2Storage.Enums;
using MapleServer2.Data.Static;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game.Helpers;

public static class MeretMarketHelper
{
    public static List<MeretMarketItem> MarketItemsSorted(List<MeretMarketItem> items, MeretMarketSort marketSort)
    {
        return marketSort switch
        {
            // TODO: Handle Most Popular sorting.
            MeretMarketSort.MostPopularUgc or
            MeretMarketSort.MostPopularPremium or
            MeretMarketSort.TopSeller => items.OrderByDescending(x => x.SalesCount).ToList(),
            MeretMarketSort.PriceLowest => items.OrderBy(x => x.Price).ToList(),
            MeretMarketSort.PriceHighest => items.OrderByDescending(x => x.Price).ToList(),
            MeretMarketSort.MostRecent => items.OrderByDescending(x => x.CreationTimestamp).ToList(),
            _ => items,
        };
    }

    public static List<MeretMarketItem> TakeLimit(IEnumerable<MeretMarketItem> items, int startPage, byte itemsPerPage)
    {
        int count = startPage * itemsPerPage - itemsPerPage;
        int offset = count;
        int limit = 5 * itemsPerPage + Math.Min(0, count);

        return items.Skip(offset).Take(limit).ToList();
    }

    public static bool CheckGender(GenderFlag genderFlag, int itemId)
    {
        if (genderFlag.HasFlag(GenderFlag.Male) && genderFlag.HasFlag(GenderFlag.Female))
        {
            return true;
        }

        Gender itemGender = ItemMetadataStorage.GetLimitMetadata(itemId).Gender;

        return (itemGender == Gender.Male && genderFlag.HasFlag(GenderFlag.Male)) || (itemGender == Gender.Female && genderFlag.HasFlag(GenderFlag.Female));
    }
}
