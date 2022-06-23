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
}
