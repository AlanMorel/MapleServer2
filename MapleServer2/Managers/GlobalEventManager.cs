using MapleServer2.Types;

namespace MapleServer2.Managers
{
    public class GlobalEventManager
    {
        private readonly Dictionary<int, GlobalEvent> GlobalEventList;

        public GlobalEventManager()
        {
            GlobalEventList = new Dictionary<int, GlobalEvent>();
        }

        public void AddEvent(GlobalEvent globalEvent) => GlobalEventList.Add(globalEvent.Id, globalEvent);

        public void RemoveEvent(GlobalEvent globalEvent) => GlobalEventList.Remove(globalEvent.Id);

        public int GetCount() => GlobalEventList.Count;

        public GlobalEvent GetEventById(int id) => GlobalEventList.GetValueOrDefault(id);
    }
}
