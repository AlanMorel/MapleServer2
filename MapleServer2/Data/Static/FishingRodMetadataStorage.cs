using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class FishingRodMetadataStorage
{
    private static readonly Dictionary<int, FishingRodMetadata> FishingRod = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.FishingRod}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<FishingRodMetadata> items = Serializer.Deserialize<List<FishingRodMetadata>>(stream);
        foreach (FishingRodMetadata item in items)
        {
            FishingRod[item.RodId] = item;
        }
    }

    public static bool IsValid(int rodId)
    {
        return FishingRod.ContainsKey(rodId);
    }

    public static FishingRodMetadata GetMetadata(int rodId)
    {
        return FishingRod.GetValueOrDefault(rodId);
    }
}
