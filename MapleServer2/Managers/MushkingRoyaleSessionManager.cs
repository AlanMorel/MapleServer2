using MapleServer2.Types;

namespace MapleServer2.Managers;

public class MushkingRoyaleSessionManager
{
    private readonly Dictionary<int, MushkingRoyaleSession> SessionList;

    private int SessionIdCounter = 1000;
    private int MapInstanceId = 100;

    public MushkingRoyaleSessionManager()
    {
        SessionList = new();
    }

    public int GetSessionId()
    {
        SessionIdCounter++;
        return SessionIdCounter;
    }

    public void AddSession(MushkingRoyaleSession session)
    {
        SessionList.Add(session.SessionId, session);
    }

    public void RemoveSession(MushkingRoyaleSession session)
    {
        SessionList.Remove(session.SessionId);
    }

    public MushkingRoyaleSession GetSession(int sessionId)
    {
        return !SessionList.ContainsKey(sessionId) ? null : SessionList[sessionId];
    }

    /*public int GetMapInstanceId()
    {
        if (RecyclableMapInstanceIds.Count <= 0)
        {
            return MapInstanceId++;
        }

        int mapInstanceId = RecyclableMapInstanceIds.First();

        RecyclableMapInstanceIds.Remove(mapInstanceId);

        return mapInstanceId;
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

    public DungeonSession GetDungeonSessionBySessionId(int dungeonSessionId)
    {
        return !DungeonSessionList.ContainsKey(dungeonSessionId) ? null : DungeonSessionList[dungeonSessionId];
    }*/
}
