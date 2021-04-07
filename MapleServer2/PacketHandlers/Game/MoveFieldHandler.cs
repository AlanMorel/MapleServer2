using System.Linq;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
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
        public override RecvOp OpCode => RecvOp.REQUEST_MOVE_FIELD;

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
                    Logger.Warning($"Unable to find portal:{portalId} in map:{srcMapId}");
                    return;
                }

                MapPortal dstPortal = MapEntityStorage.GetPortals(srcPortal.Target)
                    .FirstOrDefault(portal => portal.Target == srcMapId);
                if (dstPortal == default)
                {
                    Logger.Warning($"Unable to find return portal to map:{srcMapId} in map:{srcPortal.Target}");
                }

                dstPortal = MapEntityStorage.GetPortals(srcPortal.Target)
                .FirstOrDefault(portal => portal.Id == srcPortal.TargetPortalId);
                if (dstPortal == default)
                {
                    Logger.Warning($"Unable to find portal id:{srcPortal.TargetPortalId} in map:{srcPortal.Target}");
                    return;
                }

                // TODO: There needs to be a more centralized way to set coordinates...
                session.Player.MapId = srcPortal.Target;
                session.Player.Rotation = dstPortal.Rotation.ToFloat();
                session.Player.Coord = dstPortal.Coord.ToFloat();

                if (dstPortal.Name == "Portal_cube") // spawn on the next block if portal is a cube
                {
                    if (dstPortal.Rotation.Z == 0) // Facing SE
                    {
                        session.Player.Coord.Y -= Block.BLOCK_SIZE;
                    }
                    else if (dstPortal.Rotation.Z == 90) // Facing NE
                    {
                        session.Player.Coord.X += Block.BLOCK_SIZE;
                    }
                    else if (dstPortal.Rotation.Z == 180) // Facing NW
                    {
                        session.Player.Coord.Y += Block.BLOCK_SIZE;
                    }
                    else if (dstPortal.Rotation.Z == 270) // Facing SW
                    {
                        session.Player.Coord.X -= Block.BLOCK_SIZE;
                    }
                }

                session.Player.SafeBlock = session.Player.Coord;
                session.Send(FieldPacket.RequestEnter(session.FieldPlayer));
            }
        }
    }
}
