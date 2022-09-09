using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ItemOptionRandomMetadataStorage
{
    private static readonly Dictionary<int, ItemOptionRandomMetadata> ItemOptionRandom = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.ItemOptionRandom);
        List<ItemOptionRandomMetadata> items = Serializer.Deserialize<List<ItemOptionRandomMetadata>>(stream);
        foreach (ItemOptionRandomMetadata item in items)
        {
            ItemOptionRandom[item.Id] = item;
        }
    }

    public static ItemOptionRandom? GetMetadata(int id, int rarity)
    {
        ItemOptionRandomMetadata? metadata = ItemOptionRandom.Values.FirstOrDefault(x => x.Id == id);
        return metadata?.ItemOptions.FirstOrDefault(x => x.Rarity == rarity);
    }
}
