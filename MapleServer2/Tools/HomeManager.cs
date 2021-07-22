using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapleServer2.Database;
using MapleServer2.Types;

namespace MapleServer2.Tools
{
    public class HomeManager
    {
        private readonly ConcurrentDictionary<long, Home> HomeList;
        private readonly List<int> MapIds; // cache for maps already loaded
        private long InstanceCounter = 0;

        public HomeManager()
        {
            HomeList = new ConcurrentDictionary<long, Home>();
            MapIds = new List<int>();
            SaveLoop();
        }

        public void AddHome(Home home) => HomeList[home.Id] = home;

        public void RemoveHome(Home home) => HomeList.Remove(home.Id, out _);

        public Home GetHome(long id)
        {
            HomeList.TryGetValue(id, out Home home);
            if (home == null)
            {
                home = DatabaseManager.GetHome(id);
                if (home != null)
                {
                    home.InstanceId = InstanceCounter;
                    AddHome(home);
                    IncrementCounter();
                }
            }
            return home;
        }

        public List<Home> GetPlots(int mapId)
        {
            if (!MapIds.Contains(mapId))
            {
                List<Home> homes = DatabaseManager.GetHomesOnMap(mapId);
                foreach (Home home in homes)
                {
                    home.InstanceId = InstanceCounter;
                    AddHome(home);
                    IncrementCounter();
                }
                MapIds.Add(mapId);
            }
            return HomeList.Values.Where(h => h.PlotId == mapId).ToList();
        }

        private void SaveLoop()
        {
            Task.Run(async () =>
              {
                  while (true)
                  {
                      foreach (KeyValuePair<long, Home> kvp in HomeList)
                      {
                          DatabaseManager.UpdateHome(kvp.Value);
                      }
                      await Task.Delay(1000 * 60); // 1 minute
                      //   await Task.Delay(1000 * 60 * 30); // 30 minutes
                  }
              });
        }

        private void IncrementCounter() => InstanceCounter += 2;
    }
}
