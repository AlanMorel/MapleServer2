using MapleServer2.Data.Static;
using MapleServer2.Types;
using MapleServer2.Tools;
using TaskScheduler = MapleServer2.Tools.TaskScheduler;

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

    public GlobalEvent GetCurrentEvent()
    {
        return GlobalEventList.Values.FirstOrDefault();
    }

    public static void ScheduleEvents()
    {
        List<GlobalEvent> events = GlobalEventsMetadataStorage.GetAllAutoGlobalEvents();

        foreach (GlobalEvent globalEvent in events)
        {
            TaskScheduler.Instance.ScheduleTask(globalEvent.FirstHour, globalEvent.FirstMinutesOnHour, 1, globalEvent.MinutesToRunPerDay, globalEvent.Start);
        }
    }
}
