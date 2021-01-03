using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Packets.Helpers;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class RideSyncHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.RIDE_SYNC;

        public RideSyncHandler(ILogger<RideSyncHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte function = packet.ReadByte(); // Unknown what this is for
            packet.ReadInt(); // ServerTicks
            packet.ReadInt(); // ClientTicks
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

            Packet syncPacket = SyncStatePacket.RideSync(session.FieldPlayer, syncStates);
            session.FieldManager.BroadcastPacket(syncPacket, session);
        }
    }
}
