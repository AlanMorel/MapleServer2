using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Managers.Actors;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Serilog.Core;

namespace MapleServer2.PacketHandlers.Game.Helpers;

public static class NpcFunctionHelper
{
    public static void Function(GameSession session, int npcId, int functionId)
    {
        if (functionId == 0)
        {   
            return;
        }
        
        switch (npcId)
        {
            case 11000255: // Rosetta
                MapPortal portal = MapEntityMetadataStorage.GetPortals(session.Player.MapId).FirstOrDefault(portal => portal.Id == 99); // unsure how the portalId is determined
                if (portal is null)
                {
                    return;
                }
                session.Send(NpcTalkPacket.Action(ActionType.Portal, "", "", portal.Id));
                session.Send(NpcTalkPacket.Action(ActionType.OpenWindow, "BeautyShopDialog", "hair,style"));
                session.Send(UserMoveByPortalPacket.Move(session.Player.FieldPlayer, portal.Coord.ToFloat(), portal.Rotation.ToFloat()));
                break;
            case 11000351: // Mirror
                MapPortal portal2 = MapEntityMetadataStorage.GetPortals(session.Player.MapId).FirstOrDefault(portal => portal.Id == 99); // unsure how the portalId is determined
                if (portal2 is null)
                {
                    return;
                }
                session.Send(NpcTalkPacket.Action(ActionType.Portal, "", "", portal2.Id));
                session.Send(NpcTalkPacket.Action(ActionType.OpenWindow, "BeautyShopDialog", "mirror"));
                session.Send(UserMoveByPortalPacket.Move(session.Player.FieldPlayer, portal2.Coord.ToFloat(), portal2.Rotation.ToFloat()));
                break;
            case 11000045: // Ikas
                session.Send(NpcTalkPacket.Action(ActionType.OpenWindow, "CharacterAbilityDialog", "50"));
                break;
            case 11001953: // Kay's Wheel
                session.Send(NpcTalkPacket.Action(ActionType.OpenWindow, "RouletteDialog", "13"));
                break;
            default:
                Console.WriteLine($"Unhandled NPC function: {npcId}");
                break;
        }
    }
}
