using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseUGCBanner : DatabaseTable
{
    public DatabaseUGCBanner() : base("ad_banners") { }

    public void Insert(UGCBanner ugcBanner)
    {
        QueryFactory.Query(TableName).Insert(new
        {
            id = ugcBanner.Id,
            map_id = ugcBanner.MapId
        });
    }

    public List<UGCBanner> GetAll()
    {
        List<UGCBanner> ugcBanners = new();
        IEnumerable<dynamic> results = QueryFactory.Query(TableName).Get();
        foreach (dynamic result in results)
        {
            ugcBanners.Add(ReadUGCBanner(result));
        }
        return ugcBanners;
    }

    private static UGCBanner ReadUGCBanner(dynamic result) => new(result.id, result.map_id);
}
