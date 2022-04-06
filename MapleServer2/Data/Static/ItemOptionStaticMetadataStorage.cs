using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ItemOptionStaticMetadataStorage
{
    private static readonly Dictionary<int, ItemOptionStaticMetadata> ItemOptionStatic = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.ItemOptionStatic}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<ItemOptionStaticMetadata> items = Serializer.Deserialize<List<ItemOptionStaticMetadata>>(stream);
        foreach (ItemOptionStaticMetadata item in items)
        {
            ItemOptionStatic[item.Id] = item;
        }
    }

    public static bool IsValid(int id)
    {
        return ItemOptionStatic.ContainsKey(id);
    }

    public static ItemOptionsStatic GetMetadata(int id, int rarity)
    {
        ItemOptionStaticMetadata metadata = ItemOptionStatic.Values.FirstOrDefault(x => x.Id == id);
        if (metadata == null)
        {
            return null;
        }
        return metadata.ItemOptions.FirstOrDefault(x => x.Rarity == rarity);
    }
}
