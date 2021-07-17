using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
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

            session.Send(HomeCommandPacket.LoadHome(session.Player));
            session.Send(ResponseCubePacket.DecorationScore(session.FieldPlayer.Value.Account.Home));
            session.Send(ResponseCubePacket.LoadHome(session.FieldPlayer));
            session.Send(ResponseCubePacket.ReturnMap(session.Player.ReturnMapId));

            session.Send(KeyTablePacket.SendHotbars(session.Player.GameOptions));

            List<GameEvent> gameEvents = DatabaseManager.GetGameEvents();
            session.Send(GameEventPacket.Load(gameEvents));
        }
    }
}
