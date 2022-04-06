using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class GuildPropertyMetadataStorage
{
    private static readonly Dictionary<int, GuildPropertyMetadata> Properties = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.GuildProperty}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<GuildPropertyMetadata> items = Serializer.Deserialize<List<GuildPropertyMetadata>>(stream);
        foreach (GuildPropertyMetadata item in items)
        {
            Properties[item.Level] = item;
        }
    }

    public static GuildPropertyMetadata GetMetadata(int guildExp)
    {
        foreach (GuildPropertyMetadata property in Properties.Values)
        {
            if (guildExp < property.AccumExp)
            {
                return property;
            }
        }
        // otherwise guild is max level
        return Properties.Values.Last();
    }
}
