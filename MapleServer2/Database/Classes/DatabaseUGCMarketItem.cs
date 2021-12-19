using Maple2Storage.Enums;
using MapleServer2.Database.Types;
using MapleServer2.Enums;
using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseUGCMarketItem : DatabaseTable
{
    public DatabaseUGCMarketItem() : base("ugc_market_items") { }

    public long Insert(UGCMarketItem item)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            id = item.Id,
            price = item.Price,
            item_uid = item.Item.Uid,
            status = item.Status,
            creation_time = item.CreationTimestamp,
            listing_expiration_time = item.ListingExpirationTimestamp,
            promotion_expiration_time = item.PromotionExpirationTimestamp,
            seller_account_id = item.SellerAccountId,
            seller_character_id = item.SellerCharacterId,
            seller_character_name = item.SellerCharacterName,
            sales_count = item.SalesCount,
            description = item.Description,
            tags = JsonConvert.SerializeObject(item.Tags)
        });
    }

    public void Update(UGCMarketItem item)
    {
        QueryFactory.Query(TableName).Where("id", item.Id).Update(new
        {
            sales_count = item.SalesCount,
            status = (byte) item.Status,
            promotion_expiration_time = item.PromotionExpirationTimestamp,
            listing_expiration_time = item.ListingExpirationTimestamp,
            tags = JsonConvert.SerializeObject(item.Tags),
            description = item.Description,
            price = item.Price
        });
    }

    public List<UGCMarketItem> FindAll()
    {
        IEnumerable<dynamic> result = QueryFactory.Query(TableName).Get();
        List<UGCMarketItem> listings = new();

        foreach (dynamic data in result)
        {
            listings.Add(ReadUGCMarketItem(data));
        }
        return listings;
    }

    public UGCMarketItem FindById(long id)
    {
        return ReadUGCMarketItem(QueryFactory.Query(TableName).Where("id", id).Get().FirstOrDefault());
    }

    public bool Delete(long uid)
    {
        return QueryFactory.Query(TableName).Where("id", uid).Delete() == 1;
    }

    private static UGCMarketItem ReadUGCMarketItem(dynamic data)
    {
        return new UGCMarketItem(
            data.id,
            data.price,
            DatabaseManager.Items.FindByUid(data.item_uid),
            (UGCMarketListingStatus) data.status,
            data.creation_time,
            data.listing_expiration_time,
            data.promotion_expiration_time,
            data.seller_account_id,
            data.seller_character_id,
            data.seller_character_name,
            data.description,
            data.sales_count,
            JsonConvert.DeserializeObject<List<string>>(data.tags)
            );
    }
}
