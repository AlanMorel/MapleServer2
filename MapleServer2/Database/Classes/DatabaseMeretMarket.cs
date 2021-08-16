using Maple2Storage.Types;
using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseMeretMarket
    {
        public static List<MeretMarketItem> FindAllByCategoryId(MeretMarketCategory category) => DatabaseManager.QueryFactory.Query("meretmarketitems").Where("Category", (int) category).Get<MeretMarketItem>().ToList();

        public static MeretMarketItem FindById(int id) => DatabaseManager.QueryFactory.Query("meretmarketitems").Where("MarketId", id).Get<MeretMarketItem>().FirstOrDefault();

        public static List<Banner> FindAllBanners() => DatabaseManager.QueryFactory.Query("banners").Get<Banner>().ToList();
    }
}
