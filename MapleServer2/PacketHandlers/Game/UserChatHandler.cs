using System.Text.RegularExpressions;
using System.Xml;
using MaplePacketLib2.Tools;
using MapleServer2.Commands.Core;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Managers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game
{
    public class UserChatHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.USER_CHAT;

        public UserChatHandler() : base() { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            ChatType type = (ChatType) packet.ReadInt();
            string message = packet.ReadUnicodeString();
            string recipient = packet.ReadUnicodeString();
            long clubId = packet.ReadLong();

            if (message.Length > 0 && message.Substring(0, 1).Equals("/"))
            {
                string[] args = message[1..].Split(" ");
                if (!GameServer.CommandManager.HandleCommand(new GameCommandTrigger(args, session)))
                {
                    session.SendNotice($"No command were found with alias: {args[0]}");
                }
                return;
            }

            Packet itemLinkPacket = GetItemLink(message);

            switch (type)
            {
                case ChatType.Channel:
                    HandleChannelChat(/*session, message, type*/);
                    break;
                case ChatType.Super:
                    HandleSuperChat(session, message, type, itemLinkPacket);
                    break;
                case ChatType.World:
                    HandleWorldChat(session, message, type, itemLinkPacket);
                    break;
                case ChatType.GuildAlert:
                    HandleGuildAlert(session, message, type, itemLinkPacket);
                    break;
                case ChatType.Guild:
                    HandleGuildChat(session, message, type, itemLinkPacket);
                    break;
                case ChatType.Party:
                    HandlePartyChat(session, message, type, itemLinkPacket);
                    break;
                case ChatType.WhisperTo:
                    HandleWhisperChat(session, recipient, message, itemLinkPacket);
                    break;
                case ChatType.Club:
                    HandleClubChat(/*session, message, type, clubId, itemLinkPacket*/);
                    break;
                default:
                    HandleChat(session, message, type, itemLinkPacket);
                    break;
            }
        }

        private static void HandleChannelChat(/*GameSession session, string message, ChatType type*/)
        {
            // TODO: Send to all players on current channel
            // session.Send(NoticePacket.Notice(SystemNotice.UsedChannelChatVoucher, NoticeType.ChatAndFastText));
        }

        private static void HandleSuperChat(GameSession session, string message, ChatType type, Packet itemLinkPacket)
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

            if (itemLinkPacket != null)
            {
                MapleServer.BroadcastPacketAll(itemLinkPacket);
            }
            MapleServer.BroadcastPacketAll(ChatPacket.Send(session.Player, message, type));
            InventoryController.Consume(session, superChatItem.Uid, 1);
            session.Send(SuperChatPacket.Deselect(session.FieldPlayer));
            session.Player.SuperChat = 0;
        }

        private static void HandleWorldChat(GameSession session, string message, ChatType type, Packet itemLinkPacket)
        {
            Item voucher = session.Player.Inventory.Items.Values.FirstOrDefault(x => x.Tag == "FreeWorldChatCoupon");
            if (voucher != null)
            {
                session.Send(NoticePacket.Notice(SystemNotice.UsedWorldChatVoucher, NoticeType.ChatAndFastText));
                InventoryController.Consume(session, voucher.Uid, 1);
            }
            else if (!session.Player.Account.RemoveMerets(30))
            {
                session.Send(ChatPacket.Error(session.Player, SystemNotice.InsufficientMerets, ChatType.NoticeAlert));
                return;
            }

            if (itemLinkPacket != null)
            {
                MapleServer.BroadcastPacketAll(itemLinkPacket);
            }
            MapleServer.BroadcastPacketAll(ChatPacket.Send(session.Player, message, type));
        }

        private static void HandleGuildAlert(GameSession session, string message, ChatType type, Packet itemLinkPacket)
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

            if (itemLinkPacket != null)
            {
                guild.BroadcastPacketGuild(itemLinkPacket);
            }
            guild.BroadcastPacketGuild(ChatPacket.Send(session.Player, message, type));
        }

        private static void HandleGuildChat(GameSession session, string message, ChatType type, Packet itemLinkPacket)
        {
            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.Guild.Id);
            if (guild == null)
            {
                return;
            }

            if (itemLinkPacket != null)
            {
                guild.BroadcastPacketGuild(itemLinkPacket);
            }
            guild.BroadcastPacketGuild(ChatPacket.Send(session.Player, message, type));
        }

        private static void HandlePartyChat(GameSession session, string message, ChatType type, Packet itemLinkPacket)
        {
            Party party = session.Player.Party;
            if (party == null)
            {
                return;
            }

            if (itemLinkPacket != null)
            {
                party.BroadcastPacketParty(itemLinkPacket);
            }
            party.BroadcastPacketParty(ChatPacket.Send(session.Player, message, type));
        }

        private static void HandleWhisperChat(GameSession session, string recipient, string message, Packet itemLinkPacket)
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

            if (itemLinkPacket != null)
            {
                recipientPlayer.Session.Send(itemLinkPacket);
                session.Send(itemLinkPacket);
            }
            recipientPlayer.Session.Send(ChatPacket.Send(session.Player, message, ChatType.WhisperFrom));
            session.Send(ChatPacket.Send(recipientPlayer, message, ChatType.WhisperTo));
        }

        private static void HandleClubChat(/*GameSession session, string message, ChatType type, long clubId, Packet itemLinkPacket*/)
        {
            // TODO
        }

        private static void HandleChat(GameSession session, string message, ChatType type, Packet itemLinkPacket)
        {
            if (itemLinkPacket != null)
            {
                session.FieldManager.BroadcastPacket(itemLinkPacket);
            }
            session.FieldManager.SendChat(session.Player, message, type);
        }

        private static Packet GetItemLink(string message)
        {
            // '<' signals a message containing an item link
            if (!message.Contains('<'))
            {
                return null;
            }
            Packet itemLinkPacket = null;

            XmlDocument itemLinkMessages = new XmlDocument();
            itemLinkMessages.LoadXml("<xml>" + message + "</xml>");

            List<Item> items = new();

            foreach (XmlNode itemLinkMessage in itemLinkMessages.SelectNodes("//A"))
            {
                string[] itemLinkMessageSplit = itemLinkMessage.Attributes["HREF"].Value.Split(",");
                string itemLinkType = itemLinkMessageSplit[0].Split(":")[1];
                long itemUid = long.Parse(itemLinkMessageSplit[1]);
                Item item = null;

                if (itemLinkType == "itemTooltip")
                {
                    int itemToolTipType = int.Parse(itemLinkMessageSplit[2]);
                    if (itemToolTipType == 2) // quest/navigator items
                    {
                        if (ItemMetadataStorage.IsValid((int) itemUid))
                        {
                            item = new Item((int) itemUid, false)
                            {
                                Uid = itemUid
                            };
                        }
                    }
                    else if (itemToolTipType == 3) // normal item
                    {
                        item = DatabaseManager.Items.FindByUid(itemUid);
                    }
                    break;
                }
                if (item != null)
                {
                    items.Add(item);
                }
            }
            if (items.Count > 0)
            {
                itemLinkPacket = ItemLinkPacket.SendLinkItem(items);
            }
            return itemLinkPacket;
        }
    }
}
