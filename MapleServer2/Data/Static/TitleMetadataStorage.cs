using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class TitleMetadataStorage
{
    private static readonly Dictionary<int, TitleMetadata> Titles = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.Title);
        List<TitleMetadata> items = Serializer.Deserialize<List<TitleMetadata>>(stream);
        foreach (TitleMetadata item in items)
        {
            Titles[item.Id] = item;
        }
    }

    public static List<TitleMetadata> GetAll() => Titles.Values.ToList();
}
