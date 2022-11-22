using System.Diagnostics;
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
        DungeonSessionList = new();
        RecyclableSessionIds = new();
        RecyclableMapInstanceIds = new();
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

    public void RemoveDungeonSession(int dungeonSessionId, DungeonType dungeonType, Player player = null)
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

        if (dungeonType == DungeonType.Solo)
        {
            player.DungeonSessionId = -1;
        }
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

    public DungeonSession? GetBySessionId(int dungeonSessionId)
    {
        return !DungeonSessionList.ContainsKey(dungeonSessionId) ? null : DungeonSessionList[dungeonSessionId];
    }

    public void ResetDungeonSession(Player player, DungeonSession dungeonSession)
    {
        //TODO: implement dungeon reset button, change dungeon info here
    }


}
