using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Managers;

namespace MapleServer2.Types;

public enum DungeonType
{
    Solo,
    Group
}
public class DungeonSession
{
    public int SessionId { get; }
    public int DungeonId { get; }
    // TODO: in the server instance ids are long, casting from long to int is a narrowing conversion
    // TODO: change all dungeon instance ids to long and where they are used.
    public int DungeonInstanceId { get; }
    public List<int> DungeonMapIds { get; }
    public int DungeonLobbyId { get; }

    public DungeonType DungeonType { get; }

    public bool IsReset { get; set; }
    public bool IsCompleted { get; set; }

    public DungeonSession(int sessionId, int dungeonId, int dungeonInstanceId, DungeonType dungeonType, int partyId = -1)
    {
        DungeonType = dungeonType;
        SessionId = sessionId;
        DungeonId = dungeonId;
        DungeonInstanceId = dungeonInstanceId;
        DungeonMetadata dungeon = DungeonStorage.GetDungeonById(dungeonId);
        DungeonMapIds = dungeon.FieldIds;
        DungeonLobbyId = dungeon.LobbyFieldId;
        IsReset = false;
        IsCompleted = false;
    }

    //lobby or dungeon map
    public bool IsDungeonReservedField(int mapId, int instanceId)
    {
        return instanceId == DungeonInstanceId && (DungeonMapIds.Contains(mapId) || DungeonLobbyId == mapId);
    }

    /// <returns>true if the fieldManager is either a dungeon lobby or map</returns>
    public bool IsTravelingBetweenDungeonMaps(FieldManager fieldManager, Player player)
    {
        int originId = fieldManager.MapId;
        int destinationId = player.MapId;
        int originInstance = (int) fieldManager.InstanceId;
        int destinationInstance = (int) player.InstanceId;

        //origin id and destination id are both dungeon maps
        return IsDungeonReservedField(originId, originInstance) && IsDungeonReservedField(destinationId, destinationInstance);
    }
}
