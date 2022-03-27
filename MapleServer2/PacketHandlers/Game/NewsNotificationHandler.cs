using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game;

public class NewsNotificationHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.NewsNotif;

    private enum NewsNotificationMode : byte
    {
        OpenBrowser = 0x0,
        OpenSidebar = 0x2
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        short unk = packet.ReadShort();
        NewsNotificationMode mode = (NewsNotificationMode) packet.ReadByte();

        switch (mode)
        {
            case NewsNotificationMode.OpenBrowser:
                HandleOpenBrowser(session);
                break;
            case NewsNotificationMode.OpenSidebar:
                HandleOpenSidebar(session);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleOpenBrowser(GameSession session)
    {
        session.Send(NewsNotificationPacket.OpenBrowser());
    }

    private static void HandleOpenSidebar(GameSession session)
    {
        session.Send(NewsNotificationPacket.OpenSidebar());
    }
}
