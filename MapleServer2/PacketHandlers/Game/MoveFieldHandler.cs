using System.Linq;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
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

                if (!MapEntityStorage.HasSafePortal(srcMapId)) // map is instance only
                {
                    session.Player.MapId = session.Player.ReturnMapId;
                    session.Player.Rotation = session.FieldPlayer.Rotation;
                    session.Player.Coord = session.Player.ReturnCoord;
                    session.Player.ReturnCoord.Z += Block.BLOCK_SIZE;
                    session.Send(FieldPacket.RequestEnter(session.FieldPlayer));
                    return;
                }

                MapPortal dstPortal = MapEntityStorage.GetPortals(srcPortal.Target)
                    .FirstOrDefault(portal => portal.Target == srcMapId);
                if (dstPortal == default)
                {
                    session.Player.ReturnCoord = session.FieldPlayer.Coord;
                    session.Player.ReturnMapId = session.Player.MapId;
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
                    if (dstPortal.Rotation.Z == Direction.SOUTH_EAST)
                    {
                        session.Player.Coord.Y -= Block.BLOCK_SIZE;
                    }
                    else if (dstPortal.Rotation.Z == Direction.NORTH_EAST)
                    {
                        session.Player.Coord.X += Block.BLOCK_SIZE;
                    }
                    else if (dstPortal.Rotation.Z == Direction.NORTH_WEST)
                    {
                        session.Player.Coord.Y += Block.BLOCK_SIZE;
                    }
                    else if (dstPortal.Rotation.Z == Direction.SOUTH_WEST)
                    {
                        session.Player.Coord.X -= Block.BLOCK_SIZE;
                    }
                }

                session.Player.SafeBlock = session.Player.Coord;
                DatabaseManager.UpdateCharacter(session.Player);
                session.Send(FieldPacket.RequestEnter(session.FieldPlayer));
            }
        }

        public static void HandleInstanceMove(GameSession session, int mapId)
        {
            // TODO: Revise to include instancing

            if (MapEntityStorage.HasSafePortal(session.Player.MapId))
            {
                session.Player.ReturnCoord = session.FieldPlayer.Coord;
                session.Player.ReturnMapId = session.Player.MapId;
            }

            MapPortal dstPortal = MapEntityStorage.GetPortals(mapId).First(x => x.Id == 1);
            if (dstPortal == null)
            {
                return;
            }

            session.Player.MapId = mapId;
            session.Player.Rotation = dstPortal.Rotation.ToFloat();
            session.Player.Coord = dstPortal.Coord.ToFloat();
            DatabaseManager.UpdateCharacter(session.Player);
            session.Send(FieldPacket.RequestEnter(session.FieldPlayer));
        }
    }
}
