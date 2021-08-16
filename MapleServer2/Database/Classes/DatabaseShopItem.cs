using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseShopItem
    {
        public static ShopItem FindByUid(long uid) => DatabaseManager.QueryFactory.Query("shopitems").Where("Uid", uid).Get<ShopItem>().FirstOrDefault();

        public static List<ShopItem> FindAllByShopUid(long shopUid) => DatabaseManager.QueryFactory.Query("shopitems").Where("ShopUid", shopUid).Get<ShopItem>().ToList();
    }
}
