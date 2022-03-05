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
        // Assign new ID
        Id = GuidGenerator.Int();
        GameServer.GlobalEventManager.AddEvent(this);
        BroadcastEvent();
    }

    public async Task BroadcastEvent()
    {
        List<Player> onlinePlayers = GameServer.PlayerManager.GetAllPlayers();
        onlinePlayers = onlinePlayers.Where(x => !MapMetadataStorage.MapIsInstancedOnly(x.MapId)).ToList();
        foreach (Player player in onlinePlayers)
        {
            player.Session?.Send(GlobalPortalPacket.Notice(this));
        }

        await Task.Delay(60000);

        MapleServer.BroadcastPacketAll(GlobalPortalPacket.Clear(this));
        GameServer.GlobalEventManager.RemoveEvent(this);
    }
}

public enum GlobalEventType : byte
{
    none = 0,
    s_massive_event_name_oxquiz = 1,
    s_massive_event_name_trap_master = 2,
    s_massive_event_name_spring_beach = 3,
    s_massive_event_name_crazy_runner = 4,
    s_massive_event_name_final_surviver = 5,
    s_massive_event_name_great_escape = 6,
    s_massive_event_name_dancedance_stop = 7,
    s_massive_event_name_crazy_runner_shanghai = 8,
    s_massive_event_name_hideandseek = 9,
    s_massive_event_name_red_arena = 10,
    s_massive_event_name_blood_mine = 11,
    s_massive_event_name_treasure_island = 12,
    s_massive_event_name_christmas_dancedance_stop = 13
}
