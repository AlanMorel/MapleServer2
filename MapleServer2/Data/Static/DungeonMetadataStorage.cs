using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class DungeonStorage
{
    private static readonly Dictionary<int, DungeonMetadata> Dungeons = new();
    static List<int> DungeonMaps = new();
    static List<int> LobbyMaps = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.Dungeon);
        List<DungeonMetadata> dungeons = Serializer.Deserialize<List<DungeonMetadata>>(stream);
        foreach (DungeonMetadata dungeon in dungeons)
        {
            Dungeons.Add(dungeon.DungeonRoomId, dungeon);

            if (!LobbyMaps.Contains(dungeon.LobbyFieldId) && dungeon.LobbyFieldId != 0) //if dungeon has no lobby field id is 0, which should not be added
            {
                LobbyMaps.Add(dungeon.LobbyFieldId);
            }

            foreach (int fieldId in dungeon.FieldIds)
            {
                if (DungeonMaps.Contains(fieldId))
                {
                    continue;
                }
                DungeonMaps.Add(fieldId);
            }
        }
    }

    public static DungeonMetadata? GetDungeonById(int dungeonId)
    {
        return Dungeons.GetValueOrDefault(dungeonId);
    }

    /// <summary>
    ///either dungeon map or dungeon lobby map
    /// </summary>
    public static bool IsDungeonMap(int mapId)
    {
        return (DungeonMaps.Contains(mapId) || LobbyMaps.Contains(mapId));
    }
}
