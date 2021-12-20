using MapleServer2.Enums;
using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseUGCMarketSale : DatabaseTable
{
    public DatabaseUGCMarketSale() : base("ugc_market_sales") { }

    public long Insert(UGCMarketSale sale)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            price = sale.Price,
            profit = sale.Profit,
            item_name = sale.ItemName,
            sold_time = sale.SoldTimestamp,
            seller_character_id = sale.SellerCharacterId
        });
    }

    public List<UGCMarketSale> FindAll()
    {
        IEnumerable<dynamic> result = QueryFactory.Query(TableName).Get();
        List<UGCMarketSale> listings = new();

        foreach (dynamic data in result)
        {
            listings.Add(ReadUGCMarketSale(data));
        }
        return listings;
    }

    public UGCMarketSale FindById(long id)
    {
        return ReadUGCMarketSale(QueryFactory.Query(TableName).Where("id", id).Get().FirstOrDefault());
    }

    public bool Delete(long uid)
    {
        return QueryFactory.Query(TableName).Where("id", uid).Delete() == 1;
    }

    private static UGCMarketSale ReadUGCMarketSale(dynamic data)
    {
        return new UGCMarketSale(
            data.id,
            data.price,
            data.profit,
            data.item_name,
            data.sold_time,
            data.seller_character_id
            );
    }
}
