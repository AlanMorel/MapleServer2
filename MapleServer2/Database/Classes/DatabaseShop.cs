using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseShop
    {
        private readonly string TableName = "shops";

        public Shop FindById(long id)
        {
            Shop shop = DatabaseManager.QueryFactory.Query(TableName).Where("Id", id).Get<Shop>().FirstOrDefault();
            shop.Items = DatabaseManager.ShopItems.FindAllByShopUid(shop.Uid);
            return shop;
        }
    }
}
