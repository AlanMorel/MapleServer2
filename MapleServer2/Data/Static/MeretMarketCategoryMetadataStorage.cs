using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class MeretMarketCategoryMetadataStorage
{
    private static readonly Dictionary<int, MeretMarketCategoryMetadata> map = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-meret-market-category-metadata");
        List<MeretMarketCategoryMetadata> items = Serializer.Deserialize<List<MeretMarketCategoryMetadata>>(stream);
        foreach (MeretMarketCategoryMetadata item in items)
        {
            map[item.CategoryId] = item;
        }
    }

    public static MeretMarketCategoryMetadata GetMetadata(int categoryId)
    {
        return map.GetValueOrDefault(categoryId);
    }
}
