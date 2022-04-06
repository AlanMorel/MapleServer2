using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class TitleMetadataStorage
{
    private static readonly Dictionary<int, TitleMetadata> Titles = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.Title}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<TitleMetadata> items = Serializer.Deserialize<List<TitleMetadata>>(stream);
        foreach (TitleMetadata item in items)
        {
            Titles[item.Id] = item;
        }
    }

    public static List<TitleMetadata> GetAll() => Titles.Values.ToList();
}
