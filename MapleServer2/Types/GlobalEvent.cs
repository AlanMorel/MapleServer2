using System.Collections.Generic;
using System.Threading.Tasks;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class GlobalEvent
    {
        public readonly int Id;
        public List<GlobalEventType> Events = new List<GlobalEventType>();

        public GlobalEvent()
        {
            Id = GuidGenerator.Int();
        }

        public async Task Start()
        {
            MapleServer.BroadcastPacketAll(GlobalPortalPacket.Notice(this));

            await Task.Delay(60000);

            MapleServer.BroadcastPacketAll(GlobalPortalPacket.Clear(this));
            GameServer.GlobalEventManager.RemoveEvent(this);
        }
    }

    public enum GlobalEventType : byte
    {
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
        christmas_dancedance_stop = 13,
    }
}
