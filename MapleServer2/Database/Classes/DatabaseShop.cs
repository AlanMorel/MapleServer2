using MapleServer2.Database.Types;
using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseShop : DatabaseTable
{
    public DatabaseShop() : base("shops") { }

    public Shop FindById(int id)
    {
        dynamic? data = QueryFactory.Query(TableName).Where("id", id).Get().FirstOrDefault();
        if (data == null)
        {
            return null;
        }
        Shop shop = ReadShop(data);
        shop.Items = DatabaseManager.ShopItems.FindAllByShopId(shop.Id);
        return shop;
    }

    public List<Shop> FindAll()
    {
        IEnumerable<dynamic> result = QueryFactory.Query(TableName).Get();
        List<Shop> shops = new();
        foreach (dynamic data in result)
        {
            shops.Add(ReadShop(data));
        }

        return shops;
    }

    public void Update(Shop shop)
    {
        QueryFactory.Query(TableName).Where("id", shop.Id).Update(new
        {
            next_restock_timestamp = shop.RestockTime
        });
    }

    private static Shop ReadShop(dynamic data)
    {
        return new(data);
    }
}
