using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseShopItem
    {
        private readonly string TableName = "shopitems";

        public ShopItem FindByUid(long uid) => DatabaseManager.QueryFactory.Query(TableName).Where("Uid", uid).Get<ShopItem>().FirstOrDefault();

        public List<ShopItem> FindAllByShopUid(long shopUid) => DatabaseManager.QueryFactory.Query(TableName).Where("ShopUid", shopUid).Get<ShopItem>().ToList();
    }
}
