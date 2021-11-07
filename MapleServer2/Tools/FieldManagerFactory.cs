using MapleServer2.Managers;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Tools;

public class FieldManagerFactory
{
    private readonly Dictionary<int, List<FieldManager>> Managers;

    public FieldManagerFactory()
    {
        Managers = new();
    }

    public FieldManager GetManager(Player player)
    {
        lock (Managers)
        {
            if (!Managers.TryGetValue(player.MapId, out List<FieldManager> list))
            {
                list = new()
                {
                    new(player)
                };
                Managers[player.MapId] = list;
            }

            FieldManager manager = list.FirstOrDefault(x => x.InstanceId == player.InstanceId);
            if (manager == default)
            {
                manager = new(player);
                Managers[player.MapId].Add(manager);
            }

            manager.Increment();
            return manager;
        }
    }

    public bool Release(int key, long instanceId, Player player)
    {
        lock (Managers)
        {
            if (!Managers.TryGetValue(key, out List<FieldManager> managerList))
            {
                return false;
            }

            FieldManager fieldManager = managerList.FirstOrDefault(x => x.InstanceId == instanceId);
            if (fieldManager == default || fieldManager.Decrement() > 0)
            {
                return false;
            }

            //is only called if the leaving player is the last player on the map
            //get the dungeonsession that corresponds with the about to be released instance, in case that the player is in a party (group session) and solo session
            if (GameServer.DungeonManager.IsDungeonUsingFieldInstance(fieldManager, player))
            {
                return false;
            }

            return managerList.Remove(fieldManager);
        }
    }
}
