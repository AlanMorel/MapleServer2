using Maple2Storage.Enums;
using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseBeautyShopItem : DatabaseTable
{
    public DatabaseBeautyShopItem() : base("beauty_shop_items") { }

    public List<BeautyShopItem> FindAllByShopId(int shopId)
    {
        List<BeautyShopItem> items = new();
        IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("shop_id", shopId).Get();
        foreach (dynamic result in results)
        {
            items.Add(ReadShopItem(result));
        }

        return items;
    }

    public List<BeautyShopItem> FindAllByShopIdAndGender(int shopId, Gender gender)
    {
        return FindAllByShopId(shopId).Where(x => x.Gender == gender).ToList();
    }

    private static BeautyShopItem ReadShopItem(dynamic data)
    {
        return new BeautyShopItem(data);
    }
}
