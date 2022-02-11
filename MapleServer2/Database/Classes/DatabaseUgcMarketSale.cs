using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseUgcMarketSale : DatabaseTable
{
    public DatabaseUgcMarketSale() : base("ugc_market_sales") { }

    public long Insert(UgcMarketSale sale)
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

    public List<UgcMarketSale> FindAll()
    {
        IEnumerable<dynamic> result = QueryFactory.Query(TableName).Get();
        List<UgcMarketSale> listings = new();

        foreach (dynamic data in result)
        {
            listings.Add(ReadUgcMarketSale(data));
        }
        return listings;
    }

    public UgcMarketSale FindById(long id)
    {
        return ReadUgcMarketSale(QueryFactory.Query(TableName).Where("id", id).Get().FirstOrDefault());
    }

    public bool Delete(long uid)
    {
        return QueryFactory.Query(TableName).Where("id", uid).Delete() == 1;
    }

    private static UgcMarketSale ReadUgcMarketSale(dynamic data)
    {
        return new UgcMarketSale(
            data.id,
            data.price,
            data.profit,
            data.item_name,
            data.sold_time,
            data.seller_character_id
            );
    }
}
