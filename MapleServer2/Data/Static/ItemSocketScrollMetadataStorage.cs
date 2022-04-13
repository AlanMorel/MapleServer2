using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ItemSocketScrollMetadataStorage
{
    private static readonly Dictionary<int, ItemSocketScrollMetadata> ItemSocketScroll = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.ItemSocketScroll);
        List<ItemSocketScrollMetadata> items = Serializer.Deserialize<List<ItemSocketScrollMetadata>>(stream);
        foreach (ItemSocketScrollMetadata item in items)
        {
            ItemSocketScroll[item.Id] = item;
        }
    }

    public static ItemSocketScrollMetadata GetMetadata(int scrollId)
    {
        return ItemSocketScroll.GetValueOrDefault(scrollId);
    }
}
