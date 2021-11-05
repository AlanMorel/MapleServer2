using System.Collections.Concurrent;
using MapleServer2.Database;
using MapleServer2.Types;

namespace MapleServer2.Managers;

public class HomeManager
{
    private readonly ConcurrentDictionary<long, Home> HomeList;
    private readonly List<int> MapIds; // cache for maps already loaded
    private long InstanceCounter = 0;

    public HomeManager()
    {
        HomeList = new();
        MapIds = new();
        SaveLoop();
    }

    public void AddHome(Home home)
    {
        home.InstanceId = IncrementCounter();
        HomeList[home.Id] = home;
    }

    public void RemoveHome(Home home)
    {
        HomeList.Remove(home.Id, out _);
    }

    public Home GetHomeById(long id)
    {
        HomeList.TryGetValue(id, out Home home);
        if (home == null)
        {
            home = DatabaseManager.Homes.FindById(id);
            if (home != null)
            {
                home.InstanceId = IncrementCounter();
                AddHome(home);
            }
        }
        return home;
    }

    public Home GetHomeByAccountId(long accountId)
    {
        Home home = HomeList.Values.FirstOrDefault(x => x.AccountId == accountId);
        if (home == null)
        {
            home = DatabaseManager.Homes.FindByAccountId(accountId);
            if (home != null)
            {
                home.InstanceId = IncrementCounter();
                AddHome(home);
            }
        }
        return home;
    }

    public List<Home> GetPlots(int mapId)
    {
        if (!MapIds.Contains(mapId))
        {
            List<Home> homes = DatabaseManager.Homes.FindAllByMapId(mapId);
            foreach (Home home in homes)
            {
                home.InstanceId = IncrementCounter();
                AddHome(home);
            }
            MapIds.Add(mapId);
        }
        return HomeList.Values.Where(h => h.PlotMapId == mapId).ToList();
    }

    private void SaveLoop()
    {
        Task.Run(async () =>
        {
            while (true)
            {
                foreach (KeyValuePair<long, Home> kvp in HomeList)
                {
                    DatabaseManager.Homes.Update(kvp.Value);
                }
                await Task.Delay(1000 * 60); // 1 minute
            }
        });
    }

    // Each home have two instance id's, one for the home and one for the decor planner.
    private long IncrementCounter()
    {
        Interlocked.Increment(ref InstanceCounter);
        Interlocked.Increment(ref InstanceCounter);
        return InstanceCounter;
    }

    public List<Home> GetAllHomes()
    {
        return HomeList.Values.ToList();
    }
}
