using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ItemOptionPickMetadataStorage
{
    private static readonly Dictionary<int, ItemOptionPickMetadata> ItemOptionPick = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.ItemOptionPick}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<ItemOptionPickMetadata> items = Serializer.Deserialize<List<ItemOptionPickMetadata>>(stream);
        foreach (ItemOptionPickMetadata item in items)
        {
            ItemOptionPick[item.Id] = item;
        }
    }

    public static ItemOptionPick GetMetadata(int id, int rarity)
    {
        ItemOptionPickMetadata metadata = ItemOptionPick.Values.FirstOrDefault(x => x.Id == id);
        return metadata?.ItemOptions.FirstOrDefault(x => x.Rarity == rarity);
    }
}
