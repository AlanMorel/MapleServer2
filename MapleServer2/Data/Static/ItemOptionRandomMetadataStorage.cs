using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ItemOptionRandomMetadataStorage
{
    private static readonly Dictionary<int, ItemOptionRandomMetadata> ItemOptionRandom = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.ItemOptionRandom}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<ItemOptionRandomMetadata> items = Serializer.Deserialize<List<ItemOptionRandomMetadata>>(stream);
        foreach (ItemOptionRandomMetadata item in items)
        {
            ItemOptionRandom[item.Id] = item;
        }
    }

    public static ItemOptionRandom GetMetadata(int id, int rarity)
    {
        ItemOptionRandomMetadata metadata = ItemOptionRandom.Values.FirstOrDefault(x => x.Id == id);
        return metadata?.ItemOptions.FirstOrDefault(x => x.Rarity == rarity);
    }
}
