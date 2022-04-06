using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ItemDropMetadataStorage
{
    private static readonly Dictionary<int, ItemDropMetadata> Drops = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.ItemDrop}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<ItemDropMetadata> items = Serializer.Deserialize<List<ItemDropMetadata>>(stream);
        foreach (ItemDropMetadata item in items)
        {
            Drops[item.Id] = item;
        }
    }

    public static ItemDropMetadata GetItemDropMetadata(int boxId)
    {
        return Drops.GetValueOrDefault(boxId);
    }
}
