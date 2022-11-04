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
    public int DungeonInstanceId { get; }
    public List<int> DungeonMapIds { get; }
    public int DungeonLobbyId { get; }

    public DungeonType DungeonType { get; }

    public int PartyId { get; }

    public DungeonSession(int sessionId, int dungeonId, int dungeonInstanceId, DungeonType dungeonType, int partyId = -1)
    {
        DungeonType = dungeonType;
        SessionId = sessionId;
        DungeonId = dungeonId;
        DungeonInstanceId = dungeonInstanceId;
        DungeonMetadata dungeon = DungeonStorage.GetDungeonByDungeonId(dungeonId);
        DungeonMapIds = dungeon.FieldIds;
        DungeonLobbyId = dungeon.LobbyFieldId;
        PartyId = partyId;
    }

    public bool ContainsDungeonField(int mapId)
    {
        return DungeonMapIds.Contains(mapId);
    }


    //lobby or dungeon map
    public bool IsDungeonReservedField(int mapId, int instanceId)
    {
        return instanceId == DungeonInstanceId && (DungeonMapIds.Contains(mapId) || DungeonLobbyId == mapId);
    }

    //lobby id or dungeon map id
    //contains an instance id check cannot be used for checking whether a dungeon map 
    public bool IsTravelingBetweenDungeonMaps(FieldManager fieldManager, Player player)
    {
        int originId = fieldManager.MapId;
        int destinationId = player.MapId;
        int originInstance = fieldManager.InstanceId;
        int destinationInstance = player.InstanceId;

        //origin id and destination id are both dungeon maps
        return IsDungeonReservedField(originId, originInstance) && IsDungeonReservedField(destinationId, destinationInstance);
    }

}
