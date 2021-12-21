using Maple2Storage.Enums;
using Maple2Storage.Tools;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Managers;

public class UGCMarketManager
{
    private readonly Dictionary<long, UGCMarketItem> Items;
    private readonly Dictionary<long, UGCMarketSale> Sales;

    public UGCMarketManager()
    {
        Items = new();
        List<UGCMarketItem> items = DatabaseManager.UGCMarketItems.FindAll();
        foreach (UGCMarketItem item in items)
        {
            AddListing(item);
        }

        Sales = new();
        List<UGCMarketSale> sales = DatabaseManager.UGCMarketSales.FindAll();
        foreach (UGCMarketSale sale in sales)
        {
            AddSale(sale);
        }
    }

    public void AddListing(UGCMarketItem item)
    {
        Items.Add(item.Id, item);
    }

    public void RemoveListing(UGCMarketItem item)
    {
        Items.Remove(item.Id);
    }

    public List<UGCMarketItem> GetItemsByCharacterId(long characterId)
    {
        return new(Items.Values.Where(b => b.SellerCharacterId == characterId).ToList());
    }

    public List<UGCMarketItem> GetPromoItems()
    {
        List<UGCMarketItem> items = Items.Values.Where(x => x.PromotionExpirationTimestamp > TimeInfo.Now() && x.Status == UGCMarketListingStatus.Active).ToList();
        Random random = RandomProvider.Get();
        items = items.OrderBy(x => random.Next()).ToList();
        return items.Take(12).ToList(); // 12 being the max the shop can display
    }

    public List<UGCMarketItem> GetNewestItems()
    {
        List<UGCMarketItem> items = new(Items.Values.OrderBy(x => x.ListingExpirationTimestamp).ToList());
        return items.Where(x => x.Status == UGCMarketListingStatus.Active).Take(6).ToList();
    }

    public UGCMarketItem FindItemById(long id)
    {
        return Items.Values.FirstOrDefault(x => x.Id == id);
    }

    public List<UGCMarketItem> FindItemsByCategory(List<string> categories, GenderFlag genderFlag, JobFlag job, short sort)
    {
        List<UGCMarketItem> items = new();
        foreach (UGCMarketItem item in Items.Values)
        {
            if (!categories.Contains(item.Item.Category) || item.Status != UGCMarketListingStatus.Active)
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
                Gender gender;
                if (genderFlag.HasFlag(GenderFlag.Male))
                {
                    gender = Gender.Male;
                }
                else
                {
                    gender = Gender.Female;
                }

                if (item.Item.Gender != gender || itemGender != Gender.Neutral)
                {
                    continue;
                }
            }

            items.Add(item);
        }

        UGCMarketSort marketSort = (UGCMarketSort) sort;

        switch (marketSort)
        {
            // TODO: Handle Most Popular sorting.
            case UGCMarketSort.MostPopular:
            case UGCMarketSort.TopSeller:
                items = items.OrderByDescending(x => x.SalesCount).ToList();
                break;
            case UGCMarketSort.MostRecent:
                items = items.OrderByDescending(x => x.CreationTimestamp).ToList();
                break;
        }

        return items;
    }

    private enum UGCMarketSort : short
    {
        MostPopular = 1,
        MostRecent = 4,
        TopSeller = 6,
    }

    public void AddSale(UGCMarketSale sale)
    {
        Sales.Add(sale.Id, sale);
    }

    public void RemoveSale(UGCMarketSale sale)
    {
        Sales.Remove(sale.Id);
    }

    public List<UGCMarketSale> GetSalesByCharacterId(long characterId)
    {
        return new(Sales.Values.Where(b => b.SellerCharacterId == characterId).ToList());
    }

    public UGCMarketSale FindSaleById(long id)
    {
        return Sales.Values.FirstOrDefault(x => x.Id == id);
    }
}
