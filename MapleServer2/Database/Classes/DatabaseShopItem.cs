using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseShopItem : DatabaseTable
    {
        public DatabaseShopItem() : base("shopitems") { }

        public ShopItem FindByUid(long uid) => QueryFactory.Query(TableName).Where("uid", uid).Get<ShopItem>().FirstOrDefault();

        public List<ShopItem> FindAllByShopUid(long shopUid) => QueryFactory.Query(TableName).Where("shopuid", shopUid).Get<ShopItem>().ToList();
    }
}
