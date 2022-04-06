using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class UgcMapMetadataStorage
{
    private static readonly Dictionary<int, UgcMapMetadata> UgcMap = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.UGCMap}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<UgcMapMetadata> items = Serializer.Deserialize<List<UgcMapMetadata>>(stream);
        foreach (UgcMapMetadata item in items)
        {
            UgcMap[item.MapId] = item;
        }
    }

    public static bool IsValid(int mapId)
    {
        return UgcMap.ContainsKey(mapId);
    }

    public static UgcMapGroup GetGroupMetadata(int mapId, byte groupId)
    {
        return GetMetadata(mapId).Groups.FirstOrDefault(x => x.Id == groupId);
    }

    public static UgcMapMetadata GetMetadata(int mapId)
    {
        return UgcMap.GetValueOrDefault(mapId);
    }

    public static int GetId(int exchangeId)
    {
        return UgcMap.GetValueOrDefault(exchangeId).MapId;
    }
}
