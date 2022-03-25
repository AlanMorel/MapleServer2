using Maple2Storage.Enums;
using Maple2Storage.Tools;
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
            item.Item.SetMetadataValues();
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
        Items.Add(item.MarketId, item);
    }

    public void RemoveListing(UgcMarketItem item)
    {
        Items.Remove(item.MarketId);
    }

    public List<UgcMarketItem> GetItemsByCharacterId(long characterId)
    {
        return Items.Values.Where(b => b.SellerCharacterId == characterId).ToList();
    }

    public List<UgcMarketItem> GetPromoItems()
    {
        return Items.Values.Where(x => x.PromotionExpirationTimestamp > TimeInfo.Now() && x.Status == UgcMarketListingStatus.Active)
            .OrderBy(_ => Random.Shared.Next()).Take(12).ToList(); // 12 being the max the shop can display
    }

    public List<UgcMarketItem> GetNewestItems()
    {
        return Items.Values.OrderBy(x => x.ListingExpirationTimestamp).Where(x => x.Status == UgcMarketListingStatus.Active).Take(6).ToList();
    }

    public UgcMarketItem FindItemById(long id)
    {
        return Items.Values.FirstOrDefault(x => x.MarketId == id);
    }

    public IEnumerable<UgcMarketItem> FindItems(List<string> categories, GenderFlag genderFlag, JobFlag job, string searchString)
    {
        List<UgcMarketItem> items = new();
        foreach (UgcMarketItem item in Items.Values)
        {
            if (!categories.Contains(item.Item.Category) && categories.Count > 0)
            {
                continue;
            }

            if (item.Status != UgcMarketListingStatus.Active)
            {
                continue;
            }

            if (!item.Item.Ugc.Name.ToLower().Contains(searchString) && !item.Tags.Contains(searchString))
            {
                continue;
            }

            // Check items tags if any tags have the searched string
            bool validTag = false;
            foreach (string tag in item.Tags)
            {
                if (tag.ToLower().Contains(searchString.ToLower()))
                {
                    validTag = true;
                }
            }

            if (!validTag)
            {
                continue;
            }

            // check job
            if (!JobHelper.CheckJobFlagForJob(item.Item.RecommendJobs, job))
            {
                continue;
            }

            // check gender
            if (!MeretMarketHelper.CheckGender(genderFlag, item.Item.Id))
            {
                continue;
            }

            items.Add(item);
        }

        return items;
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
