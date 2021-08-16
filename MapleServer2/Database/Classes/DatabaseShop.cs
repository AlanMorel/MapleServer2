using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseShop
    {
        public static Shop FindById(long id)
        {
            Shop shop = DatabaseManager.QueryFactory.Query("shops").Where("Id", id).Get<Shop>().FirstOrDefault();
            shop.Items = DatabaseShopItem.FindAllByShopUid(shop.Uid);
            return shop;
        }
    }
}
