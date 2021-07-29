using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class EnterEventFieldHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.ENTER_EVENTFIELD;

        public EnterEventFieldHandler(ILogger<EnterEventFieldHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            GameEvent gameEvent = DatabaseManager.GetSingleGameEvent(GameEventType.EventFieldPopup);
            if (gameEvent == null)
            {
                return;
            }

            session.Player.ReturnCoord = session.FieldPlayer.Coord;
            session.Player.ReturnMapId = session.Player.MapId;
            session.Player.Warp(gameEvent.FieldPopupEvent.MapId);
        }
    }
}
