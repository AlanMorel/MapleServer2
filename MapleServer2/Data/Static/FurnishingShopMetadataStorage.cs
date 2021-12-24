using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class FurnishingShopMetadataStorage
{
    private static readonly Dictionary<int, FurnishingShopMetadata> FurnishingShop = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-furnishing-shop-metadata");
        List<FurnishingShopMetadata> items = Serializer.Deserialize<List<FurnishingShopMetadata>>(stream);
        foreach (FurnishingShopMetadata item in items)
        {
            FurnishingShop[item.ItemId] = item;
        }
    }

    public static bool IsValid(int itemId)
    {
        return FurnishingShop.ContainsKey(itemId);
    }

    public static FurnishingShopMetadata GetMetadata(int itemId)
    {
        return FurnishingShop.GetValueOrDefault(itemId);
    }
}
