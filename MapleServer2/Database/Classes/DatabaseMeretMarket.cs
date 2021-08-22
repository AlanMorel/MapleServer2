using Maple2Storage.Types;
using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseMeretMarket
    {
        public static List<MeretMarketItem> FindAllByCategoryId(MeretMarketCategory category)
        {
            List<MeretMarketItem> items = DatabaseManager.QueryFactory.Query("meretmarketitems").Where("Category", (int) category).Get<MeretMarketItem>().ToList();
            foreach (MeretMarketItem item in items.Where(x => x.BannerId != 0))
            {
                item.Banner = DatabaseManager.QueryFactory.Query("banners").Where("Id", item.BannerId).Get<Banner>().FirstOrDefault();
            }
            return items;
        }

        public static MeretMarketItem FindById(int id) => DatabaseManager.QueryFactory.Query("meretmarketitems").Where("MarketId", id).Get<MeretMarketItem>().FirstOrDefault();

        public static List<Banner> FindAllBanners() => DatabaseManager.QueryFactory.Query("banners").Get<Banner>().ToList();
    }
}
