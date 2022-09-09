using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class DefaultItemsMetadataStorage
{
    private static readonly Dictionary<int, DefaultItemsMetadata> Jobs = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.DefaultItems);
        List<DefaultItemsMetadata> items = Serializer.Deserialize<List<DefaultItemsMetadata>>(stream);
        foreach (DefaultItemsMetadata item in items)
        {
            Jobs[item.JobCode] = item;
        }
    }

    public static bool IsValid(int job, int itemId)
    {
        DefaultItemsMetadata? metadata = Jobs.GetValueOrDefault(job);
        if (metadata is null)
        {
            return false;
        }

        if (metadata.DefaultItems.Any(x => x.ItemId == itemId))
        {
            return metadata.DefaultItems.Any(x => x.ItemId == itemId);
        }

        DefaultItemsMetadata? job0 = Jobs.GetValueOrDefault(0);
        return job0 is not null && job0.DefaultItems.Any(x => x.ItemId == itemId);
    }
}
