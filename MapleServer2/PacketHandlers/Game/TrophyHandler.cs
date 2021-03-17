using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class TrophyHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.ACHIEVE;

        private enum TrophyHandlerMode : byte
        {

        }

        public TrophyHandler(ILogger<BreakableHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            TrophyHandlerMode mode = (TrophyHandlerMode) packet.ReadByte();
        }
    }
}
