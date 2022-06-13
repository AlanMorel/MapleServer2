using System.Collections.Concurrent;
using MapleServer2.Managers;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Tools;

public class FieldManagerFactory
{
    private readonly ConcurrentDictionary<int, List<FieldManager>> Managers;

    public FieldManagerFactory()
    {
        Managers = new();
    }

    public FieldManager GetManager(Player player)
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
        if (manager is null)
        {
            manager = new(player);
            Managers[player.MapId].Add(manager);
        }

        return manager;
    }
}
