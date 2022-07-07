using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game;

public class NewsNotificationHandler : GamePacketHandler<NewsNotificationHandler>
{
    public override RecvOp OpCode => RecvOp.NewsNotification;

    private enum Mode : byte
    {
        OpenBrowser = 0x0,
        OpenSidebar = 0x2
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        short unk = packet.ReadShort();
        Mode mode = (Mode) packet.ReadByte();

        switch (mode)
        {
            case Mode.OpenBrowser:
                HandleOpenBrowser(session);
                break;
            case Mode.OpenSidebar:
                HandleOpenSidebar(session);
                break;
            default:
                LogUnknownMode(mode);
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
