using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Packets.Helpers;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game {
    // ClientTicks/Time here are probably used for animation
    // Currently I am just updating animation instantly.
    public class UserSyncHandler : GamePacketHandler {
        public override ushort OpCode => RecvOp.USER_SYNC;

        public UserSyncHandler(ILogger<UserSyncHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet) {
            byte function = packet.ReadByte(); // Unknown what this is for
            packet.ReadInt(); // ServerTicks
            packet.ReadInt(); // ClientTicks
            byte segments = packet.ReadByte();
            if (segments < 1) {
                return;
            }

            SyncState[] syncStates = new SyncState[segments];
            for (int i = 0; i < segments; i++) {
                syncStates[i] = packet.ReadSyncState();

                packet.ReadInt(); // ClientTicks
                packet.ReadInt(); // ServerTicks
            }

            Packet syncPacket = SyncStatePacket.UserSync(session.FieldPlayer, syncStates);
            session.FieldManager.BroadcastPacket(syncPacket, session);
            session.FieldPlayer.Coord = syncStates[0].Coord.ToFloat();
            // not sure if this needs to be synced here
            session.Player.Animation = syncStates[0].Animation1;
        }
    }
}