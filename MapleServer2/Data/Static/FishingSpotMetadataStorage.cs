using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class FishingSpotMetadataStorage
{
    private static readonly Dictionary<int, FishingSpotMetadata> FishingSpot = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.FishingSpot);
        List<FishingSpotMetadata> items = Serializer.Deserialize<List<FishingSpotMetadata>>(stream);
        foreach (FishingSpotMetadata item in items)
        {
            FishingSpot[item.Id] = item;
        }
    }

    public static bool IsValid(int mapId)
    {
        return FishingSpot.ContainsKey(mapId);
    }

    public static FishingSpotMetadata? GetMetadata(int mapId)
    {
        return FishingSpot.GetValueOrDefault(mapId);
    }

    public static bool CanFish(int mapId, long playerExp)
    {
        FishingSpotMetadata? fishingSpotMetadata = FishingSpot.Values.FirstOrDefault(x => x.Id == mapId);

        int? minExpRequired = fishingSpotMetadata?.MinMastery;
        return playerExp >= minExpRequired;
    }
}
