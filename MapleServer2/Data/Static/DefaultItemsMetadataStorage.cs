using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class DefaultItemsMetadataStorage
{
    private static readonly Dictionary<int, DefaultItemsMetadata> jobs = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-default-items-metadata");
        List<DefaultItemsMetadata> items = Serializer.Deserialize<List<DefaultItemsMetadata>>(stream);
        foreach (DefaultItemsMetadata item in items)
        {
            jobs[item.JobCode] = item;
        }
    }

    public static bool IsValid(int job, int itemId)
    {
        DefaultItemsMetadata metadata = jobs.GetValueOrDefault(job);
        if (!metadata.DefaultItems.Any(x => x.ItemId == itemId))
        {
            return jobs.GetValueOrDefault(0).DefaultItems.Any(x => x.ItemId == itemId);
        }
        return metadata.DefaultItems.Any(x => x.ItemId == itemId);
    }
}
