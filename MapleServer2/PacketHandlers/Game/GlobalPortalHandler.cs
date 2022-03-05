using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class GlobalPortalHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.GLOBAL_PORTAL;

    private enum GlobalPortalMode : byte
    {
        Enter = 0x2
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        GlobalPortalMode mode = (GlobalPortalMode) packet.ReadByte();

        switch (mode)
        {
            case GlobalPortalMode.Enter:
                HandleEnter(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleEnter(GameSession session, PacketReader packet)
    {
        int globalEventId = packet.ReadInt();
        int selectionIndex = packet.ReadInt();

        GlobalEvent globalEvent = GameServer.GlobalEventManager.GetEventById(globalEventId);
        if (globalEvent == null)
        {
            return;
        }

        Map map = Map.Tria;
        switch (globalEvent.Events[selectionIndex])
        {
            case GlobalEventType.s_massive_event_name_oxquiz:
                map = Map.MapleOXQuiz;
                break;
            case GlobalEventType.s_massive_event_name_trap_master:
                map = Map.TrapMaster;
                break;
            case GlobalEventType.s_massive_event_name_spring_beach:
                map = Map.SpringBeach;
                break;
            case GlobalEventType.s_massive_event_name_crazy_runner:
                map = Map.CrazyRunners;
                break;
            case GlobalEventType.s_massive_event_name_final_surviver:
                map = Map.SoleSurvivor;
                break;
            case GlobalEventType.s_massive_event_name_great_escape:
                map = Map.LudibriumEscape;
                break;
            case GlobalEventType.s_massive_event_name_dancedance_stop:
                map = Map.DanceDanceStop;
                break;
            case GlobalEventType.s_massive_event_name_crazy_runner_shanghai:
                map = Map.ShanghaiCrazyRunners;
                break;
            case GlobalEventType.s_massive_event_name_hideandseek:
                map = Map.HideAndSeek;
                break;
            case GlobalEventType.s_massive_event_name_red_arena:
                map = Map.RedArena;
                break;
            case GlobalEventType.s_massive_event_name_blood_mine:
                map = Map.CrimsonTearMine;
                break;
            case GlobalEventType.s_massive_event_name_treasure_island:
                map = Map.TreasureIsland;
                break;
            case GlobalEventType.s_massive_event_name_christmas_dancedance_stop:
                map = Map.HolidayDanceDanceStop;
                break;
            default:
                Logger.Warn($"Unknown Global Event: {globalEvent.Events[selectionIndex]}");
                return;
        }

        session.Player.Mount = null;
        MapPortal portal = MapEntityMetadataStorage.GetPortals((int) map).FirstOrDefault(portal => portal.Id == 1);
        session.Player.Warp((int) map, portal.Coord.ToFloat(), portal.Rotation.ToFloat());
    }
}
