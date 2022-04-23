using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class AdBannerMetadataStorage
{
    private static readonly Dictionary<long, AdBannerMetadata> AdBanner = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.AdBanner);
        List<AdBannerMetadata> items = Serializer.Deserialize<List<AdBannerMetadata>>(stream);
        foreach (AdBannerMetadata item in items)
        {
            AdBanner[item.Id] = item;
        }
    }

    public static AdBannerMetadata GetMetadata(long bannerId)
    {
        return AdBanner.GetValueOrDefault(bannerId);
    }

    public static IEnumerable<AdBannerMetadata> GetMetadataList() => AdBanner.Values;
}
