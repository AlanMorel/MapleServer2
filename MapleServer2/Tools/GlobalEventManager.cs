using System.Collections.Generic;
using MapleServer2.Types;

namespace MapleServer2.Tools
{
    public class GlobalEventManager
    {
        private readonly Dictionary<int, GlobalEvent> GlobalEventList;

        public GlobalEventManager()
        {
            GlobalEventList = new Dictionary<int, GlobalEvent>();
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
}
