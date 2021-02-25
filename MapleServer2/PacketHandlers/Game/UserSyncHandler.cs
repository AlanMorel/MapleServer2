using System;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Packets.Helpers;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    // ClientTicks/Time here are probably used for animation
    // Currently I am just updating animation instantly.
    public class UserSyncHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.USER_SYNC;

        public UserSyncHandler(ILogger<UserSyncHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte function = packet.ReadByte(); // Unknown what this is for
            session.ClientTick = packet.ReadInt(); //ClientTicks
            packet.ReadInt(); // ServerTicks

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

            CoordF coord = syncStates[0].Coord.ToFloat();
            CoordF closestBlock = Block.ClosestBlock(coord);
            closestBlock.Z -= Block.BLOCK_SIZE; // Get block under player

            if (IsCoordSafe(session, syncStates[0].Coord, closestBlock))
            {
                session.Player.SafeBlock = closestBlock;
            }

            session.FieldPlayer.Coord = syncStates[0].Coord.ToFloat();

            if (IsOutOfBounds(session.FieldPlayer.Coord, session.FieldManager.BoundingBox))
            {
                int fallDamage = 100; // TODO: create a formula to determine HP loss
                CoordF safeBlock = session.Player.SafeBlock;
                safeBlock.Z += Block.BLOCK_SIZE + 1; // Without this player will spawn inside the block
                // for some reason if coord is negative player is teleported one block over, which can result player being stuck inside a block
                if (session.FieldPlayer.Coord.X < 0)
                {
                    safeBlock.X -= Block.BLOCK_SIZE;
                }
                if (session.FieldPlayer.Coord.Y < 0)
                {
                    safeBlock.Y -= Block.BLOCK_SIZE;
                }
                session.Player.ConsumeHp(fallDamage);

                session.Send(UserMoveByPortalPacket.Move(session, safeBlock));
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
