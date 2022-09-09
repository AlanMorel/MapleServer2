using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class FishingRodMetadataStorage
{
    private static readonly Dictionary<int, FishingRodMetadata> FishingRod = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.FishingRod);
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

    public static FishingRodMetadata? GetMetadata(int rodId)
    {
        return FishingRod.GetValueOrDefault(rodId);
    }
}
