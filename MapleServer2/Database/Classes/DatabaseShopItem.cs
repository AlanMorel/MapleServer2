using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseShopItem : DatabaseTable
    {
        public DatabaseShopItem() : base("ShopItems") { }

        public ShopItem FindByUid(long uid) => QueryFactory.Query(TableName).Where("Uid", uid).Get<ShopItem>().FirstOrDefault();

        public List<ShopItem> FindAllByShopUid(long shopUid) => QueryFactory.Query(TableName).Where("ShopUid", shopUid).Get<ShopItem>().ToList();
    }
}
