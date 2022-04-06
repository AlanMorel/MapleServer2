using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class DefaultItemsMetadataStorage
{
    private static readonly Dictionary<int, DefaultItemsMetadata> Jobs = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.DefaultItems}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<DefaultItemsMetadata> items = Serializer.Deserialize<List<DefaultItemsMetadata>>(stream);
        foreach (DefaultItemsMetadata item in items)
        {
            Jobs[item.JobCode] = item;
        }
    }

    public static bool IsValid(int job, int itemId)
    {
        DefaultItemsMetadata metadata = Jobs.GetValueOrDefault(job);
        if (!metadata.DefaultItems.Any(x => x.ItemId == itemId))
        {
            return Jobs.GetValueOrDefault(0).DefaultItems.Any(x => x.ItemId == itemId);
        }
        return metadata.DefaultItems.Any(x => x.ItemId == itemId);
    }
}
