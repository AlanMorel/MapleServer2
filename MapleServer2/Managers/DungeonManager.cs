using MapleServer2.Types;

namespace MapleServer2.Managers;

public class DungeonManager
{
    private readonly Dictionary<int, DungeonSession> DungeonSessionList;
    private readonly List<int> RecyclableSessionIds;
    private readonly List<int> RecyclableMapInstanceIds;

    private int SessionId;
    private int MapInstanceId;

    public DungeonManager()
    {
        DungeonSessionList = new(); //dungeonsessionid, dungeonsession
        RecyclableSessionIds = new();
        RecyclableMapInstanceIds = new(); //mapid
    }

    public int GetMapInstanceId()
    {
        if (RecyclableMapInstanceIds.Count <= 0)
        {
            return MapInstanceId++;
        }

        int mapInstanceId = RecyclableMapInstanceIds.First();

        RecyclableMapInstanceIds.Remove(mapInstanceId);

        return mapInstanceId;
    }

    public DungeonSession CreateDungeonSession(int dungeonId, DungeonType dungeonType)
    {
        int dungeonSessionId = GetUniqueSessionId();
        int dungeonInstanceId = GetMapInstanceId();

        DungeonSession dungeonSession = new(dungeonSessionId, dungeonId, dungeonInstanceId, dungeonType);
        DungeonSessionList.Add(dungeonSessionId, dungeonSession);

        return dungeonSession;
    }

    public void RemoveDungeonSession(int dungeonSessionId)
    {
        if (!DungeonSessionList.ContainsKey(dungeonSessionId))
        {
            return;
        }

        int currentDungeonSessionId = DungeonSessionList[dungeonSessionId].SessionId;
        int currentDungeonInstanceId = DungeonSessionList[dungeonSessionId].DungeonInstanceId;

        RecyclableSessionIds.Add(currentDungeonSessionId);
        RecyclableMapInstanceIds.Add(currentDungeonInstanceId);
        DungeonSessionList.Remove(dungeonSessionId);
    }

    public int GetUniqueSessionId()
    {
        if (RecyclableSessionIds.Count <= 0)
        {
            return SessionId++;
        }

        int sessionId = RecyclableSessionIds.First();

        RecyclableSessionIds.Remove(sessionId);
        return sessionId;
    }

    public DungeonSession? GetDungeonSessionBySessionId(int dungeonSessionId)
    {
        return !DungeonSessionList.ContainsKey(dungeonSessionId) ? null : DungeonSessionList[dungeonSessionId];
    }

    public bool IsDungeonUsingFieldInstance(FieldManager fieldManager, Player player) //alternatively this could be: IsFieldInstanceUsed in FieldManagerFactory
    {
        // fieldManager.MapId: left map that is to be destroyed
        // player.MapId: travel destination of the player
        DungeonSession? currentDungeonSession = GetDungeonSessionBySessionId(player.DungeonSessionId);
        if (currentDungeonSession is null) // is not null after entering dungeon via directory
        {
            return false; // no dungeon session -> the map is unused by dungeon
        }

        // check map that is left: 
        if (!currentDungeonSession.IsDungeonSessionMap(fieldManager.MapId)) // left map is not dungeon map e.g. tria
        {
            return false;
        }

        // travel destination is a dungeon map: lobby to dungeon or dungeon to lobby
        if (!currentDungeonSession.IsDungeonSessionMap(player.MapId))
        {
            return false;
        }

        // if travel destination is a dungeon it has to be the same instance or it does not pertain to the same DungeonSession.
        return player.InstanceId == currentDungeonSession.DungeonInstanceId;
    }

    public void ResetDungeonSession(Player player, DungeonSession dungeonSession)
    {
        RemoveDungeonSession(dungeonSession.SessionId);
        // if last player leaves lobby or dungeon map -> destroy dungeonSession.
        if (dungeonSession.DungeonType is DungeonType.Group && player.Party is not null)
        {
            player.Party.DungeonSessionId = -1;
            return;
        }

        player.DungeonSessionId = -1;
    }
}
