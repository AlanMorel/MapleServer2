using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class FishingSpotMetadataStorage
{
    private static readonly Dictionary<int, FishingSpotMetadata> FishingSpot = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-fishing-spot-metadata");
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

    public static FishingSpotMetadata GetMetadata(int mapId)
    {
        return FishingSpot.GetValueOrDefault(mapId);
    }

    public static bool CanFish(int mapId, long playerExp)
    {
        int minExpRequired = FishingSpot.Values.FirstOrDefault(x => x.Id == mapId).MinMastery;
        if (playerExp < minExpRequired)
        {
            return false;
        }
        return true;
    }
}
