using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;

namespace MapleServer2.Types;

public class GlobalEvent
{
    public int Id;
    public int FirstHour;
    public int FirstMinutesOnHour;
    public int MinutesToRunPerDay;
    public List<GlobalEventType> Events = new();

    public GlobalEvent()
    {
        Id = GuidGenerator.Int();
    }

    public void Start()
    {
        _ = BroadcastEvent();
    }

    public async Task BroadcastEvent()
    {
        // Assign new ID
        Id = GuidGenerator.Int();
        GameServer.GlobalEventManager.AddEvent(this);

        List<Player> onlinePlayers = GameServer.PlayerManager.GetAllPlayers().Where(x => !MapMetadataStorage.IsInstancedOnly(x.MapId)).ToList();
        foreach (Player player in onlinePlayers)
        {
            player.Session?.Send(GlobalPortalPacket.Notice(this));
        }

        await Task.Delay(60000);

        MapleServer.BroadcastPacketAll(GlobalPortalPacket.Clear(this));
        GameServer.GlobalEventManager.RemoveEvent(this);
    }

    public static string GetEventStringName(GlobalEventType type) => "s_massive_event_name_" + type;
}

public enum GlobalEventType : byte
{
    none = 0,
    oxquiz = 1,
    trap_master = 2,
    spring_beach = 3,
    crazy_runner = 4,
    final_surviver = 5,
    great_escape = 6,
    dancedance_stop = 7,
    crazy_runner_shanghai = 8,
    hideandseek = 9,
    red_arena = 10,
    blood_mine = 11,
    treasure_island = 12,
    christmas_dancedance_stop = 13
}
