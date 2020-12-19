using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game {
    public class MyInfoHandler : GamePacketHandler {
        public override RecvOp OpCode => RecvOp.MY_INFO;

        public MyInfoHandler(ILogger<MyInfoHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet) {
            byte mode = packet.ReadByte();
            switch (mode)
            {
                case 0: //Set Motto
                    string newmotto = packet.ReadUnicodeString();
                    session.Send(MyInfoPacket.SetMotto(session.Player, newmotto));
                    break;
            }
                    
            //session.Send(PacketWriter.Of(SendOp.LOAD_UGC_MAP).WriteZero(9));
            // SendCubes...?
        }
    }
}