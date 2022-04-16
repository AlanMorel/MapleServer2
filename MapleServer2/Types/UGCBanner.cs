using MapleServer2.Database;

namespace MapleServer2.Types;

public class UGCBanner
{
    public long Id;
    public int MapId;
    public List<BannerSlot> Slots;

    public UGCBanner() { }

    public UGCBanner(long id, int mapId)
    {
        Id = id;
        MapId = mapId;
        Slots = DatabaseManager.BannerSlot.FindByBannerId(id);
    }
}

public class BannerSlot
{
    public readonly long Id;
    public readonly int Date;
    public readonly int Hour;
    public readonly long BannerId;
    public bool Active;
    public UGC UGC;

    public readonly DateTimeOffset ActivateTime;

    public BannerSlot(long bannerId, int date, int hour)
    {
        BannerId = bannerId;
        Date = date;
        Hour = hour;

        int year = date / 10000;
        int month = (date % 10000) / 100;
        int day = date % 100;

        ActivateTime = new(year, month, day, hour, 0, 0, TimeSpan.Zero);
        Id = DatabaseManager.BannerSlot.Insert(this);
    }

    public BannerSlot(long id, long dateInUnixSeconds, long bannerId, bool active, UGC ugc)
    {
        Id = id;
        ActivateTime = DateTimeOffset.FromUnixTimeSeconds(dateInUnixSeconds);

        Date = int.Parse(ActivateTime.ToString("yyyyMMdd"));
        Hour = ActivateTime.Hour;

        BannerId = bannerId;
        Active = active;
        UGC = ugc;
    }
}
