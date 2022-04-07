using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class MeretMarketCategoryMetadataStorage
{
    private static readonly Dictionary<int, MeretMarketCategoryMetadata> MeretMarketCategoryMetadatas = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.MeretMarketCategory);
        List<MeretMarketCategoryMetadata> items = Serializer.Deserialize<List<MeretMarketCategoryMetadata>>(stream);
        foreach (MeretMarketCategoryMetadata item in items)
        {
            MeretMarketCategoryMetadatas[(int) item.Section] = item;
        }
    }

    public static MeretMarketTab GetTabMetadata(MeretMarketSection section, int categoryId)
    {
        MeretMarketCategoryMetadata metadata = MeretMarketCategoryMetadatas.GetValueOrDefault((int) section);
        return metadata?.Tabs.FirstOrDefault(x => x.Id == categoryId);
    }
}
