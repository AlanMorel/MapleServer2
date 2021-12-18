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
            seller_account_id = item.SellerAccountId,
            seller_character_id = item.SellerCharacterId,
            seller_character_name = item.SellerCharacterName,
            description = item.Description,
            tags = JsonConvert.SerializeObject(item.Tags)
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

    private static UGCMarketItem ReadUGCMarketItem(dynamic data)
    {
        return new UGCMarketItem(
            data.id,
            data.price,
            DatabaseManager.Items.FindByUid(data.item_uid),
            (UGCMarketListingStatus) data.status,
            data.creation_time,
            data.seller_account_id,
            data.seller_character_id,
            data.seller_character_name,
            data.description,
            JsonConvert.DeserializeObject<List<string>>(data.tags)
            );
    }
}
