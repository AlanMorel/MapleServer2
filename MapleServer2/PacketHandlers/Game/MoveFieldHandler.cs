using System.Linq;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Extensions;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class MoveFieldHandler : GamePacketHandler
    {
        public override ushort OpCode => RecvOp.REQUEST_MOVE_FIELD;

        public MoveFieldHandler(ILogger<MoveFieldHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte function = packet.ReadByte();
            if (function == 0)
            {
                int srcMapId = packet.ReadInt();
                if (srcMapId != session.FieldManager.MapId)
                {
                    return;
                }

                int portalId = packet.ReadInt();
                MapPortal srcPortal = MapEntityStorage.GetPortals(srcMapId)
                    .FirstOrDefault(portal => portal.Id == portalId);
                if (srcPortal == default)
                {
                    logger.Warning($"Unable to find portal:{portalId} in map:{srcMapId}");
                    return;
                }

                MapPortal dstPortal = MapEntityStorage.GetPortals(srcPortal.Target)
                    .FirstOrDefault(portal => portal.Target == srcMapId);
                if (dstPortal == default)
                {
                    logger.Warning($"Unable to find return portal to map:{srcMapId} in map:{srcPortal.Target}");
                    return;
                }

                // TODO: There needs to be a more centralized way to set coordinates...
                session.Player.MapId = srcPortal.Target;
                session.Player.Coord = dstPortal.Coord.ToFloat();
                session.Send(FieldPacket.RequestEnter(session.FieldPlayer));
            }
        }
    }
}