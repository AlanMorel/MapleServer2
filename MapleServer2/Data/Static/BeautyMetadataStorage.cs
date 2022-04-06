using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class BeautyMetadataStorage
{
    private static readonly Dictionary<int, BeautyMetadata> Shops = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.Beauty}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        ;
        List<BeautyMetadata> shopList = Serializer.Deserialize<List<BeautyMetadata>>(stream);
        foreach (BeautyMetadata shop in shopList)
        {
            Shops[shop.ShopId] = shop;
        }
    }

    public static List<int> GetShopIds()
    {
        return new(Shops.Keys);
    }

    public static BeautyMetadata GetShopById(int shopId)
    {
        return Shops.GetValueOrDefault(shopId);
    }

    public static List<BeautyItem> GetItems(int shopId)
    {
        return Shops.GetValueOrDefault(shopId)?.Items;
    }

    public static List<BeautyItem> GetGenderItems(int shopId, Gender gender)
    {
        BeautyMetadata targetShop = Shops.GetValueOrDefault(shopId);
        return targetShop.Items.Where(x => x.Gender == gender || x.Gender == Gender.Neutral).OrderByDescending(x => x.Flag).ToList();
    }
}
