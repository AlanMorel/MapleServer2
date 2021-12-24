using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class BlackMarketTableMetadataStorage
{
    private static readonly Dictionary<int, BlackMarketTableMetadata> BlackMarketTable = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-black-market-table-metadata");
        List<BlackMarketTableMetadata> items = Serializer.Deserialize<List<BlackMarketTableMetadata>>(stream);
        foreach (BlackMarketTableMetadata item in items)
        {
            BlackMarketTable[item.CategoryId] = item;
        }
    }

    public static List<string> GetItemCategories(int minCategoryId, int maxCategoryId)
    {
        List<string> itemCategories = new();
        foreach (BlackMarketTableMetadata metadata in BlackMarketTable.Values)
        {
            if (metadata.CategoryId >= minCategoryId && metadata.CategoryId <= maxCategoryId)
            {
                itemCategories.AddRange(metadata.ItemCategories);
            }
        }
        return itemCategories;
    }
}
