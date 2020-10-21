using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game {
    public class LoadUgcMapHandler : GamePacketHandler {
        public override ushort OpCode => RecvOp.REQUEST_LOAD_UGC_MAP;

        public LoadUgcMapHandler(ILogger<LoadUgcMapHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet) {
            session.Send(PacketWriter.Of(SendOp.LOAD_UGC_MAP).WriteZero(9));
            // SendCubes...?
        }
    }
}