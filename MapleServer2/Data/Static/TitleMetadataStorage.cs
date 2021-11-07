using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class TitleMetadataStorage
{
    private static readonly Dictionary<int, TitleMetadata> map = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-title-metadata");
        List<TitleMetadata> items = Serializer.Deserialize<List<TitleMetadata>>(stream);
        foreach (TitleMetadata item in items)
        {
            map[item.Id] = item;
        }
    }

    public static List<TitleMetadata> GetAll() => map.Values.ToList();
}
