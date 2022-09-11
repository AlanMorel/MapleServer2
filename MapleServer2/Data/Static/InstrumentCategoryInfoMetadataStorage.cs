using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class InstrumentCategoryInfoMetadataStorage
{
    private static readonly Dictionary<int, InstrumentCategoryInfoMetadata> Instruments = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.InstrumentCategoryInfo);
        List<InstrumentCategoryInfoMetadata> items = Serializer.Deserialize<List<InstrumentCategoryInfoMetadata>>(stream);
        foreach (InstrumentCategoryInfoMetadata item in items)
        {
            Instruments[item.CategoryId] = item;
        }
    }

    public static bool IsValid(int categoryId)
    {
        return Instruments.ContainsKey(categoryId);
    }

    public static InstrumentCategoryInfoMetadata? GetMetadata(int categoryId)
    {
        return Instruments.GetValueOrDefault(categoryId);
    }
}
