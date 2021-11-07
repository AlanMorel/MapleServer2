using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game;

public class MyInfoHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.MY_INFO;

    public MyInfoHandler() : base() { }

    public override void Handle(GameSession session, PacketReader packet)
    {
        byte mode = packet.ReadByte(); //I don't know any other modes this could have so right now just handle the one.
        switch (mode)
        {
            case 0: //Set Motto
                string newmotto = packet.ReadUnicodeString();
                session.Player.Motto = newmotto;
                session.FieldManager.BroadcastPacket(MyInfoPacket.SetMotto(session.FieldPlayer, newmotto));
                break;
        }
    }
}
