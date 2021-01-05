using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game {
    public class ClientTickSyncHandler : GamePacketHandler {
        public override RecvOp OpCode => RecvOp.RESPONSE_CLIENTTICK_SYNC;

        public ClientTickSyncHandler(ILogger<ClientTickSyncHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet) {
            int serverTicks = packet.ReadInt();
            if (serverTicks == session.ServerTick) {
                session.ClientTick = packet.ReadInt();
            }
        }
    }
}