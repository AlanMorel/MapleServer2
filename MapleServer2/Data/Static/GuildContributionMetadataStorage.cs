using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class GuildContributionMetadataStorage
{
    private static readonly Dictionary<string, GuildContributionMetadata> contributions = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-guild-contribution-metadata");
        List<GuildContributionMetadata> items = Serializer.Deserialize<List<GuildContributionMetadata>>(stream);
        foreach (GuildContributionMetadata item in items)
        {
            contributions[item.Type] = item;
        }
    }

    public static bool IsValid(string type)
    {
        return contributions.ContainsKey(type);
    }

    public static int GetContributionAmount(string type)
    {
        return contributions[type].Value;
    }
}
