using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ItemGemstoneUpgradeMetadataStorage
{
    private static readonly Dictionary<int, ItemGemstoneUpgradeMetadata> ItemGemstoneUpgradeMetadatas = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.ItemGemstoneUpgrade);
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
