using System.Collections.Concurrent;
using Maple2.Trigger._03009023_in;
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
            Console.WriteLine($"no list of managers found for: {mapId}");
            Console.WriteLine($"The manager has values:");
            foreach (int mId in Managers.Keys)
            {
                Console.WriteLine($"{mId}\n");
            }
            return;
        }

        if (list.Count == 0)
        {
            Console.WriteLine($"the list of managers was empty for: {mapId} instance: {instanceId}");
            return;
        }

        FieldManager matchedFieldManager = list.FirstOrDefault(fieldManager => fieldManager.InstanceId == instanceId);

        if (matchedFieldManager != null)
        {
            list.Remove(matchedFieldManager);
            Console.WriteLine($"removed field: {matchedFieldManager.MapId} instance: {matchedFieldManager.InstanceId}");
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
