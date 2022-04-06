using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class GuildContributionMetadataStorage
{
    private static readonly Dictionary<string, GuildContributionMetadata> Contributions = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.GuildContribution}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<GuildContributionMetadata> items = Serializer.Deserialize<List<GuildContributionMetadata>>(stream);
        foreach (GuildContributionMetadata item in items)
        {
            Contributions[item.Type] = item;
        }
    }

    public static bool IsValid(string type)
    {
        return Contributions.ContainsKey(type);
    }

    public static int GetContributionAmount(string type)
    {
        return Contributions[type].Value;
    }
}
