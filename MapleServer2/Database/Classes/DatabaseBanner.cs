using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseBanner : DatabaseTable
    {
        public DatabaseBanner() : base("banners") { }

        public List<Banner> FindAllBanners()
        {
            List<Banner> banners = new List<Banner>();
            IEnumerable<dynamic> results = QueryFactory.Query(TableName).Get();

            foreach (dynamic data in results)
            {
                banners.Add(ReadBanner(data));
            }
            return banners;
        }

        public Banner FindById(long id) => ReadBanner(QueryFactory.Query(TableName).Where("id", id).Get().FirstOrDefault());

        private static Banner ReadBanner(dynamic data) => new Banner(data.id, data.name, data.type, data.sub_type, data.image_url, data.language, data.begin_time, data.end_time);
    }
}
