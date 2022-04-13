using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ItemEnchantTransferMetadataStorage
{
    private static readonly Dictionary<string, ItemEnchantTransferMetadata> ItemEnchantTransfer = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.ItemEnchantTransfer);
        List<ItemEnchantTransferMetadata> items = Serializer.Deserialize<List<ItemEnchantTransferMetadata>>(stream);
        foreach (ItemEnchantTransferMetadata item in items)
        {
            string key = item.InputRarity + item.InputItemLevel.ToString() + item.InputEnchantLevel + string.Join(",", item.InputSlots);
            ItemEnchantTransfer[key] = item;
        }
    }

    public static ItemEnchantTransferMetadata GetMetadata(int rarity, int itemId, int enchantLevel)
    {
        return ItemEnchantTransfer.Values.FirstOrDefault(x => x.InputRarity == rarity &&
                                                              x.InputItemIds.Contains(itemId) && x.InputEnchantLevel == enchantLevel);
    }
}
