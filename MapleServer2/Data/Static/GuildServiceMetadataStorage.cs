using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class GuildServiceMetadataStorage
{
    private static readonly Dictionary<int, GuildServiceMetadata> Services = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.GuildService}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<GuildServiceMetadata> items = Serializer.Deserialize<List<GuildServiceMetadata>>(stream);
        foreach (GuildServiceMetadata item in items)
        {
            Services[item.Level] = item;
        }
    }

    public static GuildServiceMetadata GetMetadata(int id, int level)
    {
        return Services.Values.First(x => x.Id == id && x.Level == level + 1);
    }
}
