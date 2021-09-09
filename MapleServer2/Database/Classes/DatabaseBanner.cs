using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseBanner : DatabaseTable
    {
        public DatabaseBanner() : base("Banners") { }

        public List<Banner> FindAllBanners() => QueryFactory.Query(TableName).Get<Banner>().ToList();

        public Banner FindById(long id) => QueryFactory.Query(TableName).Where("Id", id).Get<Banner>().FirstOrDefault();
    }
}
