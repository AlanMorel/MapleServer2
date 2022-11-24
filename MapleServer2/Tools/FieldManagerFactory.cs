using System.Collections.Concurrent;
using MapleServer2.Managers;
using MapleServer2.Types;

namespace MapleServer2.Tools;

public static class FieldManagerFactory
{
    private static readonly ConcurrentDictionary<int, List<FieldManager>> Managers;

    static FieldManagerFactory()
    {
        Managers = new();
    }

    public static FieldManager GetManager(Player player)
    {
        if (!Managers.TryGetValue(player.MapId, out List<FieldManager>? list))
        {
            list = new()
            {
                new(player)
            };
            Managers[player.MapId] = list;
        }

        FieldManager? manager = list.FirstOrDefault(x => x.InstanceId == player.InstanceId);
        if (manager is null)
        {
            manager = new(player);
            Managers[player.MapId].Add(manager);
        }

        return manager;
    }

    public static void ReleaseManagerById(int mapId, int instanceId)
    {
        if (!Managers.TryGetValue(mapId, out List<FieldManager>? list))
        {
            return;
        }

        if (list.Count == 0)
        {
            return;
        }

        FieldManager matchedFieldManager = list.FirstOrDefault(fieldManager => fieldManager.InstanceId == instanceId);

        if (matchedFieldManager != null)
        {
            list.Remove(matchedFieldManager);
        }
    }

    public static void ReleaseManager(FieldManager manager)
    {
        if (!Managers.TryGetValue(manager.MapId, out List<FieldManager>? list))
        {
            return;
        }

        list.Remove(manager);
    }
}
