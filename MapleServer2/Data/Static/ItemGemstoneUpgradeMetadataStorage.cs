using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ItemGemstoneUpgradeMetadataStorage
{
    private static readonly Dictionary<int, ItemGemstoneUpgradeMetadata> ItemGemstoneUpgradeMetadatas = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.ItemGemstoneUpgrade}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<ItemGemstoneUpgradeMetadata> items = Serializer.Deserialize<List<ItemGemstoneUpgradeMetadata>>(stream);
        foreach (ItemGemstoneUpgradeMetadata item in items)
        {
            ItemGemstoneUpgradeMetadatas[item.ItemId] = item;
        }
    }

    public static ItemGemstoneUpgradeMetadata GetMetadata(int itemId)
    {
        return ItemGemstoneUpgradeMetadatas.GetValueOrDefault(itemId);
    }

    public static int GetGemLevel(int itemId)
    {
        return ItemGemstoneUpgradeMetadatas.GetValueOrDefault(itemId).GemLevel;
    }
}
