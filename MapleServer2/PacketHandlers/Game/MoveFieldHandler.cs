using System.Linq;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class MoveFieldHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_MOVE_FIELD;

        public MoveFieldHandler(ILogger<MoveFieldHandler> logger) : base(logger) { }

        private enum RequestMoveFieldMode : byte
        {
            Move = 0x0,
            LeaveInstance = 0x1
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            RequestMoveFieldMode mode = (RequestMoveFieldMode) packet.ReadByte();

            switch (mode)
            {
                case RequestMoveFieldMode.Move:
                    HandleMove(session, packet);
                    break;
                case RequestMoveFieldMode.LeaveInstance:
                    HandleLeaveInstance(session);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        public static void HandleMove(GameSession session, PacketReader packet)
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
                System.Console.WriteLine($"Unable to find portal:{portalId} in map:{srcMapId}");
                return;
            }

            if (!MapEntityStorage.HasSafePortal(srcMapId)) // map is instance only
            {
                HandleLeaveInstance(session);
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
                System.Console.WriteLine($"Unable to find portal id:{srcPortal.TargetPortalId} in map:{srcPortal.Target}");
                return;
            }

            CoordF coord = dstPortal.Coord.ToFloat();

            if (dstPortal.Name == "Portal_cube") // spawn on the next block if portal is a cube
            {
                if (dstPortal.Rotation.Z == Direction.SOUTH_EAST)
                {
                    coord.Y -= Block.BLOCK_SIZE;
                }
                else if (dstPortal.Rotation.Z == Direction.NORTH_EAST)
                {
                    coord.X += Block.BLOCK_SIZE;
                }
                else if (dstPortal.Rotation.Z == Direction.NORTH_WEST)
                {
                    coord.Y += Block.BLOCK_SIZE;
                }
                else if (dstPortal.Rotation.Z == Direction.SOUTH_WEST)
                {
                    coord.X -= Block.BLOCK_SIZE;
                }
            }

            session.Player.Warp(coord, dstPortal.Rotation.ToFloat(), srcPortal.Target);
        }

        private static void HandleLeaveInstance(GameSession session)
        {
            session.Player.MapId = session.Player.ReturnMapId;
            session.Player.Rotation = session.FieldPlayer.Rotation;
            session.Player.Coord = session.Player.ReturnCoord;
            session.Player.ReturnCoord.Z += Block.BLOCK_SIZE;
            DatabaseManager.UpdateCharacter(session.Player);
            session.Send(FieldPacket.RequestEnter(session.FieldPlayer));
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
