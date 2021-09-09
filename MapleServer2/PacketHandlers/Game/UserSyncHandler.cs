using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Packets.Helpers;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game
{
    // ClientTicks/Time here are probably used for animation
    // Currently I am just updating animation instantly.
    public class UserSyncHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.USER_SYNC;

        public UserSyncHandler() : base() { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte function = packet.ReadByte(); // Unknown what this is for
            session.ServerTick = packet.ReadInt();
            session.ClientTick = packet.ReadInt();

            byte segments = packet.ReadByte();
            if (segments < 1)
            {
                return;
            }

            SyncState[] syncStates = new SyncState[segments];
            for (int i = 0; i < segments; i++)
            {
                syncStates[i] = packet.ReadSyncState();

                packet.ReadInt(); // ClientTicks
                packet.ReadInt(); // ServerTicks
            }

            Packet syncPacket = SyncStatePacket.UserSync(session.FieldPlayer, syncStates);
            session.FieldManager.BroadcastPacket(syncPacket, session);
            UpdatePlayer(session, syncStates);
        }

        public static void UpdatePlayer(GameSession session, SyncState[] syncStates)
        {
            CoordF coord = syncStates[0].Coord.ToFloat();
            CoordF closestBlock = Block.ClosestBlock(coord);
            closestBlock.Z -= Block.BLOCK_SIZE; // Get block under player

            if (IsCoordSafe(session, syncStates[0].Coord, closestBlock))
            {
                session.Player.SafeBlock = closestBlock;
            }

            session.FieldPlayer.Coord = syncStates[0].Coord.ToFloat();
            CoordF rotation = new CoordF();
            rotation.Z = syncStates[0].Rotation / 10;
            session.FieldPlayer.Rotation = rotation;

            if (IsOutOfBounds(session.FieldPlayer.Coord, session.FieldManager.BoundingBox))
            {
                int currentHp = session.Player.Stats[PlayerStatId.Hp].Current;
                int fallDamage = currentHp * Math.Clamp(currentHp * 4 / 100 - 1, 0, 25) / 100; // TODO: Create accurate damage model
                CoordF safeBlock = session.Player.SafeBlock;
                safeBlock.Z += Block.BLOCK_SIZE + 1; // Without this player will spawn inside the block
                session.Player.ConsumeHp(fallDamage);

                session.Send(UserMoveByPortalPacket.Move(session.FieldPlayer, safeBlock, session.Player.Rotation));
                session.Send(StatPacket.UpdateStats(session.FieldPlayer, PlayerStatId.Hp));
                session.Send(FallDamagePacket.FallDamage(session, fallDamage));
            }
            // not sure if this needs to be synced here
            session.Player.Animation = syncStates[0].Animation1;
        }

        private static bool IsOutOfBounds(CoordF coord, CoordS[] boundingBox)
        {
            short higherBoundZ = Math.Max(boundingBox[0].Z, boundingBox[1].Z);
            short lowerBoundZ = Math.Min(boundingBox[1].Z, boundingBox[0].Z);
            short higherBoundY = Math.Max(boundingBox[0].Y, boundingBox[1].Y);
            short lowerBoundY = Math.Min(boundingBox[1].Y, boundingBox[0].Y);
            short higherBoundX = Math.Max(boundingBox[0].X, boundingBox[1].X);
            short lowerBoundX = Math.Min(boundingBox[1].X, boundingBox[0].X);

            if (coord.Z > higherBoundZ || coord.Z < lowerBoundZ)
            {
                return true;
            }
            else if (coord.Y > higherBoundY || coord.Y < lowerBoundY)
            {
                return true;
            }
            else if (coord.X > higherBoundX || coord.X < lowerBoundX)
            {
                return true;
            }

            return false;
        }

        private static bool IsCoordSafe(GameSession session, CoordS currentCoord, CoordF closestCoord)
        {
            // Save last coord if player is not falling and not in a air mount
            return MapMetadataStorage.BlockExists(session.Player.MapId, closestCoord.ToShort()) &&
                !session.Player.OnAirMount &&
                (session.Player.SafeBlock - closestCoord).Length() > 350 &&
                session.FieldPlayer.Coord.Z == currentCoord.Z;
        }
    }
}
