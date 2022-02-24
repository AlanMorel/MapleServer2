using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game;

public class EnterEventFieldHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.ENTER_EVENTFIELD;

    public override void Handle(GameSession session, PacketReader packet)
    {
        EventFieldPopup fieldPopupEvent = DatabaseManager.Events.FindFieldPopupEvent();
        if (fieldPopupEvent == null)
        {
            return;
        }

        session.Player.Warp(fieldPopupEvent.MapId);
    }
}
