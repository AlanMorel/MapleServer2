using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Packets.Helpers;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game {
    public class StateHandler : GamePacketHandler {
        public override RecvOp OpCode => RecvOp.STATE;

        public StateHandler(ILogger<StateHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet) {
            byte function = packet.ReadByte();
            switch (function)
            {
                case 0:
                    //Jump
                    break;
                case 1:
                    //Land
                    break;
            }
        }
    }
}