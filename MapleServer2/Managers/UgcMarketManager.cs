using Maple2Storage.Enums;
using Maple2Storage.Tools;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Managers;

public class UgcMarketManager
{
    private readonly Dictionary<long, UgcMarketItem> Items;
    private readonly Dictionary<long, UgcMarketSale> Sales;

    public UgcMarketManager()
    {
        Items = new();
        List<UgcMarketItem> items = DatabaseManager.UgcMarketItems.FindAll();
        foreach (UgcMarketItem item in items)
        {
            AddListing(item);
        }

        Sales = new();
        List<UgcMarketSale> sales = DatabaseManager.UgcMarketSales.FindAll();
        foreach (UgcMarketSale sale in sales)
        {
            AddSale(sale);
        }
    }

    public void AddListing(UgcMarketItem item)
    {
        Items.Add(item.Id, item);
    }

    public void RemoveListing(UgcMarketItem item)
    {
        Items.Remove(item.Id);
    }

    public List<UgcMarketItem> GetItemsByCharacterId(long characterId)
    {
        return Items.Values.Where(b => b.SellerCharacterId == characterId).ToList();
    }

    public List<UgcMarketItem> GetPromoItems()
    {
        return Items.Values.Where(x => x.PromotionExpirationTimestamp > TimeInfo.Now() && x.Status == UgcMarketListingStatus.Active)
            .OrderBy(_ => RandomProvider.Get().Next()).Take(12).ToList(); // 12 being the max the shop can display
    }

    public List<UgcMarketItem> GetNewestItems()
    {
        return Items.Values.OrderBy(x => x.ListingExpirationTimestamp).Where(x => x.Status == UgcMarketListingStatus.Active).Take(6).ToList();
    }

    public UgcMarketItem FindItemById(long id)
    {
        return Items.Values.FirstOrDefault(x => x.Id == id);
    }

    public List<UgcMarketItem> FindItemsByCategory(List<string> categories, GenderFlag genderFlag, JobFlag job, short sort)
    {
        List<UgcMarketItem> items = new();
        foreach (UgcMarketItem item in Items.Values)
        {
            if (!categories.Contains(item.Item.Category) || item.Status != UgcMarketListingStatus.Active)
            {
                continue;
            }

            // check job
            if (!JobHelper.CheckJobFlagForJob(item.Item.RecommendJobs, job))
            {
                continue;
            }

            // check gender
            Gender itemGender = ItemMetadataStorage.GetGender(item.Item.Id);
            if (!genderFlag.HasFlag(GenderFlag.Male) && !genderFlag.HasFlag(GenderFlag.Female))
            {
                Gender gender = genderFlag.HasFlag(GenderFlag.Male) ? Gender.Male : Gender.Female;
            }

            items.Add(item);
        }

        UgcMarketSort marketSort = (UgcMarketSort) sort;

        switch (marketSort)
        {
            // TODO: Handle Most Popular sorting.
            case UgcMarketSort.MostPopular:
            case UgcMarketSort.TopSeller:
                items = items.OrderByDescending(x => x.SalesCount).ToList();
                break;
            case UgcMarketSort.MostRecent:
                items = items.OrderByDescending(x => x.CreationTimestamp).ToList();
                break;
        }

        return items;
    }

    private enum UgcMarketSort : short
    {
        MostPopular = 1,
        MostRecent = 4,
        TopSeller = 6
    }

    public void AddSale(UgcMarketSale sale)
    {
        Sales.Add(sale.Id, sale);
    }

    public void RemoveSale(UgcMarketSale sale)
    {
        Sales.Remove(sale.Id);
    }

    public List<UgcMarketSale> GetSalesByCharacterId(long characterId)
    {
        return Sales.Values.Where(b => b.SellerCharacterId == characterId).ToList();
    }

    public UgcMarketSale FindSaleById(long id)
    {
        return Sales.Values.FirstOrDefault(x => x.Id == id);
    }
}
