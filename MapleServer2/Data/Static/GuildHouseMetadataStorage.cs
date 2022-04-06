using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class GuildHouseMetadataStorage
{
    private static readonly Dictionary<int, GuildHouseMetadata> Houses = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.GuildHouse}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<GuildHouseMetadata> items = Serializer.Deserialize<List<GuildHouseMetadata>>(stream);
        foreach (GuildHouseMetadata item in items)
        {
            Houses[item.FieldId] = item;
        }
    }

    public static GuildHouseMetadata GetMetadataByThemeId(int level, int themeId)
    {
        return Houses.Values.FirstOrDefault(x => x.Level == level && x.Theme == themeId);
    }

    public static int GetFieldId(int level, int themeId)
    {
        return Houses.Values.FirstOrDefault(x => x.Level == level && x.Theme == themeId).FieldId;
    }
}
