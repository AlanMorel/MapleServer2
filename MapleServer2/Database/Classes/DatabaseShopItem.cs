using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseShopItem : DatabaseTable
{
    public DatabaseShopItem() : base("shop_items") { }

    public ShopItem FindByUid(long uid)
    {
        return ReadShopItem(QueryFactory.Query(TableName).Where("uid", uid).Get().FirstOrDefault());
    }

    public List<ShopItem> FindAllByShopId(int shopId)
    {
        List<ShopItem> items = new();
        IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("shop_id", shopId).Get();
        foreach (dynamic result in results)
        {
            items.Add(ReadShopItem(result));
        }

        return items;
    }

    public ShopItem? FindByItemId(int itemId)
    {
        dynamic result = QueryFactory.Query(TableName).Where("item_id", itemId).FirstOrDefault();

        if (result == null)
        {
            return null;
        }

        return ReadShopItem(result);
    }

    private static ShopItem ReadShopItem(dynamic data)
    {
        return new ShopItem(data);
    }
}
