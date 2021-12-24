using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class GuildPropertyMetadataStorage
{
    private static readonly Dictionary<int, GuildPropertyMetadata> Properties = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-guild-property-metadata");
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
