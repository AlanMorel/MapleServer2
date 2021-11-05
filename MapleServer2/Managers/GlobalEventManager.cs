using MapleServer2.Types;

namespace MapleServer2.Managers;

public class GlobalEventManager
{
    private readonly Dictionary<int, GlobalEvent> GlobalEventList;

    public GlobalEventManager()
    {
        GlobalEventList = new();
    }

    public void AddEvent(GlobalEvent globalEvent)
    {
        GlobalEventList.Add(globalEvent.Id, globalEvent);
    }

    public void RemoveEvent(GlobalEvent globalEvent)
    {
        GlobalEventList.Remove(globalEvent.Id);
    }

    public int GetCount()
    {
        return GlobalEventList.Count;
    }

    public GlobalEvent GetEventById(int id)
    {
        return GlobalEventList.GetValueOrDefault(id);
    }
}
