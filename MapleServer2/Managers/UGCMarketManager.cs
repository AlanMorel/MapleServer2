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

    //public BlackMarketListing GetListingByItemUid(long uid)
    //{
    //    return Listings.Values.FirstOrDefault(b => b.Item.Uid == uid);
    //}

    //public BlackMarketListing GetListingById(long listingId)
    //{
    //    return Listings.Values.FirstOrDefault(x => x.Id == listingId);
    //}
}
