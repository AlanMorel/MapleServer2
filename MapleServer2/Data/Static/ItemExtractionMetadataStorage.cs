using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ItemExtractionMetadataStorage
{
    private static readonly Dictionary<int, ItemExtractionMetadata> ItemExtractionMetadatas = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.ItemExtraction}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<ItemExtractionMetadata> items = Serializer.Deserialize<List<ItemExtractionMetadata>>(stream);
        foreach (ItemExtractionMetadata item in items)
        {
            ItemExtractionMetadatas[item.SourceItemId] = item;
        }
    }

    public static bool IsValid(int itemId)
    {
        return ItemExtractionMetadatas.ContainsKey(itemId);
    }

    public static ItemExtractionMetadata GetMetadata(int itemId)
    {
        return ItemExtractionMetadatas.GetValueOrDefault(itemId);
    }

    public static byte GetExtractionCount(int itemId)
    {
        ItemExtractionMetadata metadata = ItemExtractionMetadatas.GetValueOrDefault(itemId);
        if (metadata == null)
        {
            return 0;
        }
        return ItemExtractionMetadatas.GetValueOrDefault(itemId).TryCount;
    }
}
