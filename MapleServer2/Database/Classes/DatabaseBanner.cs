using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseBanner
    {
        private readonly string TableName = "banners";

        public List<Banner> FindAllBanners() => DatabaseManager.QueryFactory.Query(TableName).Get<Banner>().ToList();

        public Banner FindById(long id) => DatabaseManager.QueryFactory.Query(TableName).Where("BannerId", id).Get<Banner>().FirstOrDefault();
    }
}
