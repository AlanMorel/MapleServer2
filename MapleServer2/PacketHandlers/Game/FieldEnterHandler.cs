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
            Player player = session.Player;
            Account account = player.Account;
            session.EnterField(player);
            session.Send(StatPacket.SetStats(session.FieldPlayer));
            session.Send(StatPointPacket.WriteTotalStatPoints(player));
            if (account.IsVip())
            {
                session.Send(BuffPacket.SendBuff(0, new Status(100000014, session.FieldPlayer.ObjectId, session.FieldPlayer.ObjectId, 1, (int) account.VIPExpiration, 1)));
                session.Send(PremiumClubPacket.ActivatePremium(session.FieldPlayer, account.VIPExpiration));
            }
            session.Send(EmotePacket.LoadEmotes(player));
            session.Send(ChatStickerPacket.LoadChatSticker(player));

            session.Send(HomeCommandPacket.LoadHome(player));
            session.Send(ResponseCubePacket.DecorationScore(account.Home));
            session.Send(ResponseCubePacket.LoadHome(session.FieldPlayer));
            session.Send(ResponseCubePacket.ReturnMap(player.ReturnMapId));
            if (player.Party != null)
            {
                session.Send(PartyPacket.UpdatePlayer(player));
            }

            session.Send(KeyTablePacket.SendHotbars(player.GameOptions));

            List<GameEvent> gameEvents = DatabaseManager.Events.FindAll();
            session.Send(GameEventPacket.Load(gameEvents));
        }
    }
}
