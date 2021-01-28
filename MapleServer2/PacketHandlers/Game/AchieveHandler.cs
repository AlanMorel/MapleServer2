using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class AchieveHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.ACHIEVE;
        private enum AchieveHandlerMode : byte
        {
        
        }

        public AchieveHandler(ILogger<BreakableHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            AchieveHandlerMode mode = (AchieveHandlerMode) packet.ReadByte();
            switch (mode)
            {
                
            }
        }
    }
}
