using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ItemOptionConstantMetadataStorage
{
    private static readonly Dictionary<int, ItemOptionConstantMetadata> ItemOptionConstant = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.ItemOptionConstant);
        List<ItemOptionConstantMetadata> items = Serializer.Deserialize<List<ItemOptionConstantMetadata>>(stream);
        foreach (ItemOptionConstantMetadata item in items)
        {
            ItemOptionConstant[item.Id] = item;
        }
    }

    public static bool IsValid(int id)
    {
        return ItemOptionConstant.ContainsKey(id);
    }

    public static ItemOptionsConstant? GetMetadata(int id, int rarity)
    {
        ItemOptionConstantMetadata? metadata = ItemOptionConstant.Values.FirstOrDefault(x => x.Id == id);
        return metadata?.ItemOptions.FirstOrDefault(x => x.Rarity == rarity);
    }
}
