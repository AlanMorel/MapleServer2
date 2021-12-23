namespace MapleServer2.Database.Types;

public class Banner
{
    public int Id { get; set; }
    public string Name { get; set; } // name must start with "homeproduct_" for Meret Market banners
    public BannerType Type { get; set; }
    public BannerSubType SubType { get; set; }
    public string ImageUrl { get; set; } // Meret Market banner resolution: 538x301
    public BannerLanguage Language { get; set; }
    public long BeginTime { get; set; }
    public long EndTime { get; set; }


    public Banner() { }

    public Banner(int id, string name, string type, string sub_type, string image_url, int language, long begin_time, long end_time)
    {
        Id = id;
        Name = name;
        _ = Enum.TryParse(type, out BannerType bannerType);
        Type = bannerType;
        _ = Enum.TryParse(sub_type, out BannerSubType bannerSubType);
        SubType = bannerSubType;
        ImageUrl = image_url;
        Language = (BannerLanguage) language;
        BeginTime = begin_time;
        EndTime = end_time;
    }
}
public enum BannerType
{
    merat,
    playgift,
    pcbang,
    right, // used for cash
    left // used for cash
}
public enum BannerSubType
{
    cash
}
public enum BannerLanguage
{
    All = -1,
    Korean = 2
}
