using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class UGCMapMetadataStorage
{
    private static readonly Dictionary<int, UGCMapMetadata> map = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-ugc-map-metadata");
        List<UGCMapMetadata> items = Serializer.Deserialize<List<UGCMapMetadata>>(stream);
        foreach (UGCMapMetadata item in items)
        {
            map[item.MapId] = item;
        }
    }

    public static bool IsValid(int mapId)
    {
        return map.ContainsKey(mapId);
    }

    public static UGCMapGroup GetGroupMetadata(int mapId, byte groupId)
    {
        return GetMetadata(mapId).Groups.FirstOrDefault(x => x.Id == groupId);
    }

    public static UGCMapMetadata GetMetadata(int mapId)
    {
        return map.GetValueOrDefault(mapId);
    }

    public static int GetId(int exchangeId)
    {
        return map.GetValueOrDefault(exchangeId).MapId;
    }
}
