using MapleServer2.Types;
using Serilog;

namespace MapleServer2.Managers;

public class MushkingRoyaleQueueManager
{
    private readonly Dictionary<QueueType, MushkingRoyaleQueue> Queues;
    private readonly ILogger Logger = Log.Logger.ForContext<MushkingRoyaleQueueManager>();

    public MushkingRoyaleQueueManager()
    {
        Queues = new()
        {
            [QueueType.Solo] = new(),
            [QueueType.Squad] = new()
        };
    }

    public MushkingRoyaleQueue JoinQueue(Player player, QueueType type)
    {
        Queues[type].AddPlayer(player);
        return Queues[type];
    }

    public void LeaveQueue(Player player, QueueType type)
    {
        Queues[type].RemovePlayer(player);
    }
}

public enum QueueType
{
    Solo,
    Squad
}
