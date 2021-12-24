using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class FishMetadataStorage
{
    private static readonly Dictionary<int, FishMetadata> FishMetadatas = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-fish-metadata");
        List<FishMetadata> items = Serializer.Deserialize<List<FishMetadata>>(stream);
        foreach (FishMetadata item in items)
        {
            FishMetadatas[item.Id] = item;
        }
    }

    public static bool IsValid(int fishId)
    {
        return FishMetadatas.ContainsKey(fishId);
    }

    public static FishMetadata GetMetadata(int fishId)
    {
        return FishMetadatas.GetValueOrDefault(fishId);
    }

    public static List<FishMetadata> GetValidFishes(int mapId, string habitat)
    {
        if (habitat == "water") // temp fix for maps with seawater. 
        {
            return FishMetadatas.Values.Where(x => x.HabitatMapId.Contains(mapId) &&
                                        (x.Habitat == habitat || x.Habitat == "seawater")).ToList();
        }
        return FishMetadatas.Values.Where(x => x.HabitatMapId.Contains(mapId) && x.Habitat == habitat).ToList();
    }
}
