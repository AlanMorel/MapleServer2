using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class EnchantLimitMetadataStorage
{
    private static readonly Dictionary<int, EnchantLimitMetadata> EnchantLimit = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.ChatSticker);
        List<EnchantLimitMetadata> items = Serializer.Deserialize<List<EnchantLimitMetadata>>(stream);
        foreach (EnchantLimitMetadata item in items)
        {
            EnchantLimit[(int) item.ItemType] = item;
        }
    }

    public static bool IsEnchantable(ItemType type, int itemLevel, int enchantLevel)
    {
        EnchantLimitMetadata metadata = EnchantLimit.GetValueOrDefault((int) type);
        if (metadata is null)
        {
            return false;
        }

        return itemLevel >= metadata.MinLevel && itemLevel <= metadata.MaxLevel && enchantLevel < metadata.MaxEnchantLevel;
    }
}
