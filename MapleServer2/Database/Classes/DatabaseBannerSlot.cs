using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseBannerSlot : DatabaseTable
{
    public DatabaseBannerSlot() : base("ad_banner_slots") { }

    public long Insert(BannerSlot bannerSlot)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            datetime = bannerSlot.ActivateTime.ToUnixTimeSeconds(),
            banner_id = bannerSlot.BannerId,
            active = bannerSlot.Active
        });
    }

    public void UpdateUGCUid(long id, long ugcUid)
    {
        QueryFactory.Query(TableName).Where("id", id).Update(new
        {
            ugc_uid = ugcUid
        });
    }

    public void UpdateActive(long id, bool active)
    {
        QueryFactory.Query(TableName).Where("id", id).Update(new
        {
            active
        });
    }

    public void Delete(long id)
    {
        QueryFactory.Query(TableName).Where("id", id).Delete();
    }

    public List<BannerSlot> FindByBannerId(long bannerId)
    {
        List<BannerSlot> banners = new();
        IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("banner_id", bannerId).Get();

        foreach (dynamic data in results)
        {
            banners.Add(ReadBannerSlot(data));
        }

        return banners;
    }

    private static BannerSlot ReadBannerSlot(dynamic data)
    {
        return new BannerSlot(data.id, data.datetime, data.banner_id, data.active == 1, DatabaseManager.UGC.FindByUid(data.ugc_uid));
    }
}
