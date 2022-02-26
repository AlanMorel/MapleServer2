using System.Xml;
using MaplePacketLib2.Tools;
using MapleServer2.Commands.Core;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Enums;
using MapleServer2.Managers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class UserChatHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.USER_CHAT;

    public override void Handle(GameSession session, PacketReader packet)
    {
        ChatType type = (ChatType) packet.ReadInt();
        string message = packet.ReadUnicodeString();
        string recipient = packet.ReadUnicodeString();
        long clubId = packet.ReadLong();

        if (message.Length > 0 && message[..1].Equals("/"))
        {
            string[] args = message[1..].Split(" ");
            if (!GameServer.CommandManager.HandleCommand(new GameCommandTrigger(args, session)))
            {
                session.SendNotice($"No command were found with alias: {args[0]}");
            }
            return;
        }

        PacketWriter itemLinkPacket = GetItemLink(message);

        switch (type)
        {
            case ChatType.Channel:
                HandleChannelChat(session, message, type, itemLinkPacket);
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
                HandleClubChat(session, message, type, clubId, itemLinkPacket);
                break;
            case ChatType.All:
            case ChatType.ChatBubble:
                HandleChat(session, message, type, itemLinkPacket);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(type);
                break;
        }
    }

    private static void HandleChannelChat(GameSession session, string message, ChatType type, PacketWriter itemLinkPacket)
    {
        Player player = session.Player;
        
        int meretCost = int.Parse(ConstantsMetadataStorage.GetConstant("MeratConsumeChannelChat"));
        
        // check if event is in progress
        SaleChat saleEvent = DatabaseManager.Events.FindSaleChatEvent();
        if (saleEvent is not null)
        {
            meretCost  = (int) (meretCost - (meretCost * Convert.ToSingle(saleEvent.ChannelChatDiscountAmount) / 100 / 100));
        }
        
        Item voucher = player.Inventory.Items.Values.FirstOrDefault(x => x.Tag == "FreeChannelChatCoupon");
        if (voucher is not null)
        {
            session.Send(NoticePacket.Notice(SystemNotice.UsedChannelChatVoucher, NoticeType.ChatAndFastText));
            player.Inventory.ConsumeItem(session, voucher.Uid, 1);
        }
        else if (!player.Account.RemoveMerets(meretCost))
        {
            session.Send(ChatPacket.Error(player, SystemNotice.InsufficientMerets, ChatType.NoticeAlert));
            return;
        }

        List<Player> allPlayers = GameServer.PlayerManager.GetAllPlayers();
        foreach (Player i in allPlayers.Where(x => x.ChannelId == player.ChannelId))
        {
            if (itemLinkPacket is not null)
            {
                i.Session.Send(itemLinkPacket);
            }
            i.Session.Send(ChatPacket.Send(i, message, type));
        }
    }

    private static void HandleSuperChat(GameSession session, string message, ChatType type, PacketWriter itemLinkPacket)
    {
        if (session.Player.SuperChat == 0)
        {
            return;
        }

        Item superChatItem = session.Player.Inventory.Items.Values.FirstOrDefault(x => x.Function.Id == session.Player.SuperChat);
        if (superChatItem is null)
        {
            session.Player.SuperChat = 0;
            session.Send(SuperChatPacket.Deselect(session.Player.FieldPlayer));
            session.Send(ChatPacket.Error(session.Player, SystemNotice.InsufficientSuperChatThemes, ChatType.NoticeAlert));
            return;
        }

        if (itemLinkPacket is not null)
        {
            MapleServer.BroadcastPacketAll(itemLinkPacket);
        }
        MapleServer.BroadcastPacketAll(ChatPacket.Send(session.Player, message, type));
        session.Player.Inventory.ConsumeItem(session, superChatItem.Uid, 1);
        session.Send(SuperChatPacket.Deselect(session.Player.FieldPlayer));
        session.Player.SuperChat = 0;
    }

    private static void HandleWorldChat(GameSession session, string message, ChatType type, PacketWriter itemLinkPacket)
    {
        int meretCost = int.Parse(ConstantsMetadataStorage.GetConstant("MeratConsumeWorldChat"));
        
        // check if event is in progress
        SaleChat saleEvent = DatabaseManager.Events.FindSaleChatEvent();
        if (saleEvent is not null)
        {
            meretCost  = (int) (meretCost - (meretCost * Convert.ToSingle(saleEvent.WorldChatDiscountAmount) / 100 / 100));
        }
        
        Item voucher = session.Player.Inventory.Items.Values.FirstOrDefault(x => x.Tag == "FreeWorldChatCoupon");
        if (voucher is not null)
        {
            session.Send(NoticePacket.Notice(SystemNotice.UsedWorldChatVoucher, NoticeType.ChatAndFastText));
            session.Player.Inventory.ConsumeItem(session, voucher.Uid, 1);
        }
        else if (!session.Player.Account.RemoveMerets(meretCost))
        {
            session.Send(ChatPacket.Error(session.Player, SystemNotice.InsufficientMerets, ChatType.NoticeAlert));
            return;
        }

        if (itemLinkPacket is not null)
        {
            MapleServer.BroadcastPacketAll(itemLinkPacket);
        }
        MapleServer.BroadcastPacketAll(ChatPacket.Send(session.Player, message, type));
    }

    private static void HandleGuildAlert(GameSession session, string message, ChatType type, PacketWriter itemLinkPacket)
    {
        Guild guild = GameServer.GuildManager.GetGuildById(session.Player.Guild.Id);

        GuildMember member = guild?.Members.FirstOrDefault(x => x.Player == session.Player);
        if (member is null)
        {
            return;
        }

        if (!((GuildRights) guild.Ranks[member.Rank].Rights).HasFlag(GuildRights.CanGuildAlert))
        {
            return;
        }

        if (itemLinkPacket is not null)
        {
            guild.BroadcastPacketGuild(itemLinkPacket);
        }
        guild.BroadcastPacketGuild(ChatPacket.Send(session.Player, message, type));
    }

    private static void HandleGuildChat(GameSession session, string message, ChatType type, PacketWriter itemLinkPacket)
    {
        Guild guild = GameServer.GuildManager.GetGuildById(session.Player.Guild.Id);
        if (guild is null)
        {
            return;
        }

        if (itemLinkPacket is not null)
        {
            guild.BroadcastPacketGuild(itemLinkPacket);
        }
        guild.BroadcastPacketGuild(ChatPacket.Send(session.Player, message, type));
    }

    private static void HandlePartyChat(GameSession session, string message, ChatType type, PacketWriter itemLinkPacket)
    {
        Party party = session.Player.Party;
        if (party is null)
        {
            return;
        }

        if (itemLinkPacket is not null)
        {
            party.BroadcastPacketParty(itemLinkPacket);
        }
        party.BroadcastPacketParty(ChatPacket.Send(session.Player, message, type));
    }

    private static void HandleWhisperChat(GameSession session, string recipient, string message, PacketWriter itemLinkPacket)
    {
        Player recipientPlayer = GameServer.PlayerManager.GetPlayerByName(recipient);
        if (recipientPlayer is null)
        {
            session.Send(ChatPacket.Error(session.Player, SystemNotice.UnableToWhisper, ChatType.WhisperFail));
            return;
        }

        if (BuddyManager.IsBlocked(session.Player, recipientPlayer))
        {
            session.Send(ChatPacket.Error(session.Player, SystemNotice.UnableToWhisper, ChatType.WhisperFail));
            return;
        }

        if (itemLinkPacket is not null)
        {
            recipientPlayer.Session.Send(itemLinkPacket);
            session.Send(itemLinkPacket);
        }
        recipientPlayer.Session.Send(ChatPacket.Send(session.Player, message, ChatType.WhisperFrom));
        session.Send(ChatPacket.Send(recipientPlayer, message, ChatType.WhisperTo));
    }

    private static void HandleClubChat(GameSession session, string message, ChatType type, long clubId, PacketWriter itemLinkPacket)
    {
        Club club = GameServer.ClubManager.GetClubById(clubId);
        if (club is null || !session.Player.Clubs.Contains(club))
        {
            return;
        }

        if (itemLinkPacket is not null)
        {
            club.BroadcastPacketClub(itemLinkPacket);
        }

        club.BroadcastPacketClub(ChatPacket.Send(session.Player, message, type, clubId));
    }

    private static void HandleChat(GameSession session, string message, ChatType type, PacketWriter itemLinkPacket)
    {
        if (itemLinkPacket is not null)
        {
            session.FieldManager.BroadcastPacket(itemLinkPacket);
        }
        session.FieldManager.SendChat(session.Player, message, type);
    }

    private static PacketWriter GetItemLink(string message)
    {
        // '<' signals a message containing an item link
        if (!message.Contains('<'))
        {
            return null;
        }
        PacketWriter itemLinkPacket = null;

        XmlDocument itemLinkMessages = new();
        itemLinkMessages.LoadXml("<xml>" + message + "</xml>");

        List<Item> items = new();

        foreach (XmlNode itemLinkMessage in itemLinkMessages.SelectNodes("//A"))
        {
            string[] itemLinkMessageSplit = itemLinkMessage.Attributes["HREF"].Value.Split(",");
            string itemLinkType = itemLinkMessageSplit[0].Split(":")[1];
            long itemUid = long.Parse(itemLinkMessageSplit[1]);
            Item item = null;

            if (itemLinkType != "itemTooltip")
            {
                continue;
            }

            int itemToolTipType = int.Parse(itemLinkMessageSplit[2]);
            if (itemToolTipType == 2) // quest/navigator items
            {
                if (ItemMetadataStorage.IsValid((int) itemUid))
                {
                    item = new((int) itemUid, false)
                    {
                        Uid = itemUid
                    };
                }
            }
            else if (itemToolTipType == 3) // normal item
            {
                item = DatabaseManager.Items.FindByUid(itemUid);
            }

            if (item is not null)
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
