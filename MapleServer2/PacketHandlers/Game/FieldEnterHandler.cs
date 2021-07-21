using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class FieldEnterHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.RESPONSE_FIELD_ENTER;

        public FieldEnterHandler(ILogger<FieldEnterHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            packet.ReadInt(); // ?

            // Liftable: 00 00 00 00 00
            // SendBreakable
            // Self
            session.EnterField(session.Player);
            session.Send(StatPacket.SetStats(session.FieldPlayer));
            session.Send(StatPointPacket.WriteTotalStatPoints(session.Player));
            if (session.Player.IsVip())
            {
                session.Send(PremiumClubPacket.ActivatePremium(session.FieldPlayer, session.Player.VIPExpiration));
            }
            session.Send(EmotePacket.LoadEmotes(session.Player));
            session.Send(ChatStickerPacket.LoadChatSticker(session.Player));
            session.Send(ResponseCubePacket.LoadHome(session.FieldPlayer));

            // Normally skill layout would be loaded from a database
            QuickSlot arrowStream = QuickSlot.From(10500001);
            QuickSlot arrowBarrage = QuickSlot.From(10500011);
            QuickSlot eagleGlide = QuickSlot.From(10500151);
            QuickSlot testSkill = QuickSlot.From(10500153);

            if (session.Player.GameOptions.TryGetHotbar(0, out Hotbar mainHotbar))
            {
                /*
                mainHotbar.MoveQuickSlot(4, arrowStream);
                mainHotbar.MoveQuickSlot(5, arrowBarrage);
                mainHotbar.MoveQuickSlot(6, eagleGlide);
                mainHotbar.MoveQuickSlot(7, testSkill);
                */
                session.Send(KeyTablePacket.SendHotbars(session.Player.GameOptions));
            }

            List<GameEvent> gameEvents = DatabaseManager.GetGameEvents();
            session.Send(GameEventPacket.Load(gameEvents));
        }
    }
}
