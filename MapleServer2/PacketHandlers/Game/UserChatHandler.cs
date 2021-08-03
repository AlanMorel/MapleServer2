using System.Linq;
using MaplePacketLib2.Tools;
using MapleServer2.Commands.Core;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class UserChatHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.USER_CHAT;

        public UserChatHandler(ILogger<GamePacketHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            ChatType type = (ChatType) packet.ReadInt();
            string message = packet.ReadUnicodeString();
            string recipient = packet.ReadUnicodeString();
            long clubId = packet.ReadLong();

            if (message.Length > 0 && message.Substring(0, 1).Equals("/"))
            {
                string[] args = message[1..].ToLower().Split(" ");
                GameServer.CommandManager.HandleCommand(new GameCommandTrigger(args, session));
                return;
            }

            switch (type)
            {
                case ChatType.Channel:
                    HandleChannelChat(/*session, message, type*/);
                    break;
                case ChatType.Super:
                    HandleSuperChat(session, message, type);
                    break;
                case ChatType.World:
                    HandleWorldChat(session, message, type);
                    break;
                case ChatType.GuildAlert:
                    HandleGuildAlert(session, message, type);
                    break;
                case ChatType.Guild:
                    HandleGuildChat(session, message, type);
                    break;
                case ChatType.Party:
                    HandlePartyChat(session, message, type);
                    break;
                case ChatType.WhisperTo:
                    HandleWhisperChat(session, recipient, message);
                    break;
                case ChatType.Club:
                    HandleClubChat(/*session, message, type, clubId*/);
                    break;
                default:
                    HandleChat(session, message, type);
                    break;
            }
        }

        private static void HandleChannelChat(/*GameSession session, string message, ChatType type*/)
        {
            // TODO: Send to all players on current channel
            // session.Send(NoticePacket.Notice(SystemNotice.UsedChannelChatVoucher, NoticeType.ChatAndFastText));
        }

        private static void HandleSuperChat(GameSession session, string message, ChatType type)
        {
            if (session.Player.SuperChat == 0)
            {
                return;
            }

            Item superChatItem = session.Player.Inventory.Items.Values.FirstOrDefault(x => x.Function.Id == session.Player.SuperChat);
            if (superChatItem == null)
            {
                session.Player.SuperChat = 0;
                session.Send(SuperChatPacket.Deselect(session.FieldPlayer));
                session.Send(ChatPacket.Error(session.Player, SystemNotice.InsufficientSuperChatThemes, ChatType.NoticeAlert));
                return;
            }

            MapleServer.BroadcastPacketAll(ChatPacket.Send(session.Player, message, type));
            InventoryController.Consume(session, superChatItem.Uid, 1);
            session.Send(SuperChatPacket.Deselect(session.FieldPlayer));
            session.Player.SuperChat = 0;
        }

        private static void HandleWorldChat(GameSession session, string message, ChatType type)
        {
            Item voucher = session.Player.Inventory.Items.Values.FirstOrDefault(x => x.Tag == "FreeWorldChatCoupon");
            if (voucher != null)
            {
                session.Send(NoticePacket.Notice(SystemNotice.UsedWorldChatVoucher, NoticeType.ChatAndFastText));
                InventoryController.Consume(session, voucher.Uid, 1);
            }
            else if (!session.Player.Account.RemoveMerets(session, 30))
            {
                session.Send(ChatPacket.Error(session.Player, SystemNotice.InsufficientMerets, ChatType.NoticeAlert));
                return;
            }

            MapleServer.BroadcastPacketAll(ChatPacket.Send(session.Player, message, type));
        }

        private static void HandleGuildAlert(GameSession session, string message, ChatType type)
        {
            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.Guild.Id);
            if (guild == null)
            {
                return;
            }

            GuildMember member = guild.Members.FirstOrDefault(x => x.Player == session.Player);
            if (member == null)
            {
                return;
            }

            if (!((GuildRights) guild.Ranks[member.Rank].Rights).HasFlag(GuildRights.CanGuildAlert))
            {
                return;
            }

            guild.BroadcastPacketGuild(ChatPacket.Send(session.Player, message, type));
        }

        private static void HandleGuildChat(GameSession session, string message, ChatType type)
        {
            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.Guild.Id);
            if (guild == null)
            {
                return;
            }

            guild.BroadcastPacketGuild(ChatPacket.Send(session.Player, message, type));
        }

        private static void HandlePartyChat(GameSession session, string message, ChatType type)
        {
            Party party = session.Player.Party;
            if (party == null)
            {
                return;
            }

            party.BroadcastPacketParty(ChatPacket.Send(session.Player, message, type));
        }

        private static void HandleWhisperChat(GameSession session, string recipient, string message)
        {
            Player recipientPlayer = GameServer.Storage.GetPlayerByName(recipient);
            if (recipientPlayer == null)
            {
                session.Send(ChatPacket.Error(session.Player, SystemNotice.UnableToWhisper, ChatType.WhisperFail));
                return;
            }

            if (BuddyManager.IsBlocked(session.Player, recipientPlayer))
            {
                session.Send(ChatPacket.Error(session.Player, SystemNotice.UnableToWhisper, ChatType.WhisperFail));
                return;
            }

            recipientPlayer.Session.Send(ChatPacket.Send(session.Player, message, ChatType.WhisperFrom));
            session.Send(ChatPacket.Send(recipientPlayer, message, ChatType.WhisperTo));
        }

        private static void HandleClubChat(/*GameSession session, string message, ChatType type, long clubId*/)
        {
            // TODO
        }

        private static void HandleChat(GameSession session, string message, ChatType type)
        {
            session.FieldManager.SendChat(session.Player, message, type);
        }
    }
}
