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
            if (IsNewSafeCoordNeeded(session, syncStates[0].Coord.ToFloat()))
            {
                session.Player.SafeCoord = session.FieldPlayer.Coord.ClosestBlock();
            }

            session.FieldPlayer.Coord = syncStates[0].Coord.ToFloat();


            if (IsOutOfBounds(session.FieldPlayer.Coord, session.FieldManager.BoundingBox))
            {
                session.Send(UserMoveByPortalPacket.Move(session, session.Player.SafeCoord));
                session.Send(FallDamagePacket.FallDamage(session, 150)); // TODO: create a formula to determine HP loss
            }
            // not sure if this needs to be synced here
            session.Player.Animation = syncStates[0].Animation1;
        }

        private static bool IsOutOfBounds(CoordF coord, CoordS[] boundingBox)
        {
            CoordS higherBoundingBox = boundingBox[0].Z > boundingBox[1].Z ? boundingBox[0] : boundingBox[1];
            CoordS lowerBoundingBox = boundingBox[0].Z > boundingBox[1].Z ? boundingBox[1] : boundingBox[0];
            if (coord.X > higherBoundingBox.X || coord.Y < higherBoundingBox.Y || coord.Z > higherBoundingBox.Z || (coord.X < lowerBoundingBox.X || coord.Y > lowerBoundingBox.Y || coord.Z < lowerBoundingBox.Z))
            {
                return true;
            }
            return false;
        }

        private static bool IsNewSafeCoordNeeded(GameSession session, CoordF lastestCoord)
        {
            return (session.Player.SafeCoord - lastestCoord).Length() > 200 && session.FieldPlayer.Coord.Z == lastestCoord.Z && !session.Player.OnAirMount; // Save last coord if player is not falling and not in a air mount
        }
    }
}
