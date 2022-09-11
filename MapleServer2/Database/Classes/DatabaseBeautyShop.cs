using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseBeautyShop : DatabaseTable
{
    public DatabaseBeautyShop() : base("beauty_shops") { }

    public BeautyShop FindById(int id)
    {
        dynamic data = QueryFactory.Query(TableName).Where("id", id).Get().FirstOrDefault();
        if (data == null)
        {
            return null;
        }

        BeautyShop shop = ReadShop(data);
        return shop;
    }

    private static BeautyShop ReadShop(dynamic data)
    {
        List<BeautyShopItem> items = DatabaseManager.BeautyShopItems.FindAllByShopId(data.id);
        return new BeautyShop(data, items);
    }

    public int GetSpecialVoucher()
    {
        dynamic data = QueryFactory.Query(TableName).Where("id", 509).Get().FirstOrDefault();
        return data?.required_item_id;
    }
}
