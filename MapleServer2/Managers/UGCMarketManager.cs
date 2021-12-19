using Maple2Storage.Tools;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Managers;

public class UGCMarketManager
{
    private readonly Dictionary<long, UGCMarketItem> Items;

    public UGCMarketManager()
    {
        Items = new();
        List<UGCMarketItem> items = DatabaseManager.UGCMarketItems.FindAll();
        foreach (UGCMarketItem item in items)
        {
            AddListing(item);
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
        return Items.Values.Where(b => b.SellerCharacterId == characterId).ToList();
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
        List<UGCMarketItem> items = Items.Values.OrderBy(x => x.ListingExpirationTimestamp).ToList();
        return items.Where(x => x.Status == UGCMarketListingStatus.Active).Take(6).ToList();
    }

    //public BlackMarketListing GetListingByItemUid(long uid)
    //{
    //    return Listings.Values.FirstOrDefault(b => b.Item.Uid == uid);
    //}

    public UGCMarketItem FindById(long id)
    {
        return Items.Values.FirstOrDefault(x => x.Id == id);
    }
}
