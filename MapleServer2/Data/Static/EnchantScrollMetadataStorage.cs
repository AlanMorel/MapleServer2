using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class EnchantScrollMetadataStorage
{
    private static readonly Dictionary<int, EnchantScrollMetadata> EnchantScroll = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.EnchantScroll);
        List<EnchantScrollMetadata> items = Serializer.Deserialize<List<EnchantScrollMetadata>>(stream);
        foreach (EnchantScrollMetadata item in items)
        {
            EnchantScroll[item.Id] = item;
        }
    }

    public static EnchantScrollMetadata? GetMetadata(int scrollId)
    {
        return EnchantScroll.GetValueOrDefault(scrollId);
    }
}
