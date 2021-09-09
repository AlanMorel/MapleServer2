using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseShop : DatabaseTable
    {
        public DatabaseShop() : base("Shops") { }

        public Shop FindById(long id)
        {
            Shop shop = QueryFactory.Query(TableName).Where("Id", id).Get<Shop>().FirstOrDefault();
            if (shop == default)
            {
                return null;
            }
            shop.Items = DatabaseManager.ShopItems.FindAllByShopUid(shop.Uid);
            return shop;
        }
    }
}
