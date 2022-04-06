using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class MeretMarketCategoryMetadataStorage
{
    private static readonly Dictionary<int, MeretMarketCategoryMetadata> MeretMarketCategoryMetadatas = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.MeretMarketCategory}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
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
