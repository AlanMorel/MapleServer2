﻿using System.Collections.Generic;
using System.Linq;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class GuildHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.GUILD;

        public GuildHandler(ILogger<GuildHandler> logger) : base(logger) { }

        private enum GuildMode : byte
        {
            Create = 0x1,
            Disband = 0x2,
            Invite = 0x3,
            InviteResponse = 0x5,
            Leave = 0x7,
            Kick = 0x8,
            RankChange = 0xA,
            PlayerMessage = 0xD,
            CheckIn = 0xF,
            TransferLeader = 0x3D,
            GuildNotice = 0x3E,
            ListGuild = 0x42,
            SubmitApplication = 0x51,
            ApplicationResponse = 0x53,
            GuildWindowRequest = 0x54,
            GuildListRequest = 0x55,
            UseBuff = 0x59,
            EnterHouse = 0x64,
            GuildDonate = 0x6E,
            Services = 0x6F,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            GuildMode mode = (GuildMode) packet.ReadByte();
            switch (mode)
            {
                case GuildMode.Create:
                    HandleCreate(session, packet);
                    break;
                case GuildMode.Disband:
                    HandleDisband(session);
                    break;
                case GuildMode.Invite:
                    HandleInvite(session, packet);
                    break;
                case GuildMode.InviteResponse:
                    HandleInviteResponse(session, packet);
                    break;
                case GuildMode.Leave:
                    HandleLeave(session);
                    break;
                case GuildMode.Kick:
                    HandleKick(session, packet);
                    break;
                case GuildMode.RankChange:
                    HandleRankChange(session, packet);
                    break;
                case GuildMode.PlayerMessage:
                    HandlePlayerMessage(session, packet);
                    break;
                case GuildMode.CheckIn:
                    HandleCheckIn(session);
                    break;
                case GuildMode.TransferLeader:
                    HandleTransferLeader(session, packet);
                    break;
                case GuildMode.GuildNotice:
                    HandleGuildNotice(session, packet);
                    break;
                case GuildMode.ListGuild:
                    HandleListGuild(session, packet);
                    break;
                case GuildMode.SubmitApplication:
                    HandleSubmitApplication(session, packet);
                    break;
                case GuildMode.ApplicationResponse:
                    HandleApplicationResponse(session, packet);
                    break;
                case GuildMode.GuildWindowRequest:
                    HandleGuildWindowRequest(session);
                    break;
                case GuildMode.GuildListRequest:
                    HandleGuildListRequest(session);
                    break;
                case GuildMode.UseBuff:
                    HandleUseBuff(session, packet);
                    break;
                case GuildMode.EnterHouse:
                    HandleEnterHouse(/*session, packet*/);
                    break;
                case GuildMode.GuildDonate:
                    HandleGuildDonate(session, packet);
                    break;
                case GuildMode.Services:
                    HandleServices(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleCreate(GameSession session, PacketReader packet)
        {
            string guildName = packet.ReadUnicodeString();

            if (!session.Player.Wallet.Meso.Modify(-2000))
            {
                short noticeCode = 5121;
                session.Send(GuildPacket.ErrorNotice(noticeCode));
                return;
            }

            session.FieldManager.BroadcastPacket(GuildPacket.UpdateGuildTag(session.Player, guildName));
            session.Send(GuildPacket.Create(guildName));

            string inviter = ""; // nobody because nobody invited the guild leader
            byte response = 0; // 0 to not display invite notification
            byte rank = 0; // set to leader rank

            Guild newGuild = new(guildName, new List<Player> { session.Player });
            GameServer.GuildManager.AddGuild(newGuild);
            session.Player.GuildId = newGuild.Id;

            session.Send(GuildPacket.UpdateGuild(newGuild));
            session.Send(GuildPacket.MemberBroadcastJoinNotice(session.Player, inviter, response, rank));
            session.Send(GuildPacket.MemberJoin(session.Player));
        }

        private static void HandleDisband(GameSession session)
        {
            Guild guild = GameServer.GuildManager.GetGuildByLeader(session.Player);
            if (guild == null)
            {
                return;
            }

            session.Send(GuildPacket.DisbandConfirm());
            guild.BroadcastPacketGuild(GuildPacket.MemberNotice(session.Player));
            GameServer.GuildManager.RemoveGuild(guild);
            session.Player.GuildId = 0;
        }

        private static void HandleInvite(GameSession session, PacketReader packet)
        {
            string targetPlayer = packet.ReadUnicodeString();

            Guild guild = GameServer.GuildManager.GetGuildByLeader(session.Player);
            if (guild == null)
            {
                return;
            }

            Player playerInvited = GameServer.Storage.GetPlayerByName(targetPlayer);
            if (playerInvited == null)
            {
                short noticeCode = 2563;
                session.Send(GuildPacket.ErrorNotice(noticeCode));
            }

            if (playerInvited.GuildId != 0)
            {
                short noticeCode = 1027;
                session.Send(GuildPacket.ErrorNotice(noticeCode));
                return;
            }

            if (guild.Members.Count >= guild.MaxMembers)
            {
                //TODO Plug in 'full guild' error packets
                return;
            }
            else
            {
                session.Send(GuildPacket.InviteConfirm(playerInvited));
                playerInvited.Session.Send(GuildPacket.SendInvite(session.Player, playerInvited, guild));
            }
        }

        private static void HandleInviteResponse(GameSession session, PacketReader packet)
        {
            long guildId = packet.ReadLong();
            string guildName = packet.ReadUnicodeString();
            packet.ReadShort();
            string inviterName = packet.ReadUnicodeString();
            string inviteeName = packet.ReadUnicodeString();
            byte response = packet.ReadByte(); // 01 accept 

            Guild guild = GameServer.GuildManager.GetGuildById(guildId);
            if (guild == null)
            {
                return;
            }

            Player inviter = GameServer.Storage.GetPlayerByName(inviterName);
            if (inviter == null)
            {
                return;
            }

            if (response == 00)
            {
                inviter.Session.Send(GuildPacket.InviteNotification(inviteeName, 256));
                session.Send(GuildPacket.InviteResponseConfirm(inviter, session.Player, guild, response));
                return;
            }
            else
            {
                // TODO: Reject packets
            }

            byte rank = 3;

            inviter.Session.Send(GuildPacket.InviteNotification(inviteeName, response));
            session.Send(GuildPacket.InviteResponseConfirm(inviter, session.Player, guild, response));
            session.FieldManager.BroadcastPacket(GuildPacket.UpdateGuildTag(session.Player, guildName));
            guild.AddMember(session.Player);
            session.Player.GuildId = guild.Id;
            guild.BroadcastPacketGuild(GuildPacket.MemberBroadcastJoinNotice(session.Player, inviterName, response, rank));
            guild.BroadcastPacketGuild(GuildPacket.MemberJoin(session.Player));
            session.Send(GuildPacket.UpdateGuild(guild));
        }

        private static void HandleLeave(GameSession session)
        {
            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.GuildId);
            if (guild == null)
            {
                return;
            }

            session.Send(GuildPacket.LeaveConfirm());
            session.Send(GuildPacket.MemberNotice(session.Player));
            guild.BroadcastPacketGuild(GuildPacket.MemberLeaveNotice(session.Player));
            guild.RemoveMember(session.Player);
        }

        private static void HandleKick(GameSession session, PacketReader packet)
        {
            string target = packet.ReadUnicodeString();

            Player member = GameServer.Storage.GetPlayerByName(target);
            if (member == null)
            {
                return;
            }

            Guild guild = GameServer.GuildManager.GetGuildByLeader(session.Player);
            if (guild == null)
            {
                return;
            }

            if (member.CharacterId == guild.Leader.CharacterId)
            {
                //TODO: Error packets
                return;
            }

            session.Send(GuildPacket.KickConfirm(member));
            member.Session.Send(GuildPacket.KickNotification(session.Player));
            member.Session.Send(GuildPacket.MemberNotice(member));
            guild.RemoveMember(member);
            guild.BroadcastPacketGuild(GuildPacket.KickMember(member, session.Player));

        }

        private static void HandleRankChange(GameSession session, PacketReader packet)
        {
            string memberName = packet.ReadUnicodeString();
            byte rank = packet.ReadByte();

            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.GuildId);
            if (guild == null)
            {
                return;
            }

            session.Send(GuildPacket.RankChangeConfirm(memberName, rank));
            guild.BroadcastPacketGuild(GuildPacket.RankChangeNotice(session.Player.Name, memberName, rank));
            // TODO Change guildmember ranks
        }

        private static void HandlePlayerMessage(GameSession session, PacketReader packet)
        {
            string message = packet.ReadUnicodeString();

            session.Send(GuildPacket.UpdatePlayerMessage(session.Player, message));
        }

        private static void HandleCheckIn(GameSession session)
        {
            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.GuildId);
            if (guild == null)
            {
                return;
            }

            GuildContribution contribution = GuildMetadataStorage.GetContribution("attend");

            guild.Funds += 10000;
            guild.Exp += 80;
            session.Player.GuildContribution += contribution.Value;
            session.Send(GuildPacket.CheckInConfirm());
            // TODO: Send Guild Coins
            // TODO: Can only check in once a day
            session.Send(GuildPacket.UpdateGuildFunds(guild.Funds));
            session.Send(GuildPacket.UpdateGuildExp(guild.Exp));
            session.Send(GuildPacket.UpdateGuildStatsNotice(80, 0));
            session.Send(GuildPacket.UpdateGuildStatsNotice(0, 10000));
            session.Send(GuildPacket.UpdatePlayerContribution(session.Player, 10));
        }

        private static void HandleTransferLeader(GameSession session, PacketReader packet)
        {
            string target = packet.ReadUnicodeString();

            Player newLeader = GameServer.Storage.GetPlayerByName(target);
            if (newLeader == null)
            {
                return;
            }

            Player oldLeader = session.Player;

            Guild guild = GameServer.GuildManager.GetGuildByLeader(oldLeader);
            if (guild == null)
            {
                return;
            }

            session.Send(GuildPacket.TransferLeaderConfirm(newLeader));
            guild.BroadcastPacketGuild(GuildPacket.AssignNewLeader(newLeader, oldLeader));
            guild.Members.Insert(0, newLeader);
            guild.Members.Remove(oldLeader);
            guild.Members.Add(oldLeader);
        }

        private static void HandleGuildNotice(GameSession session, PacketReader packet)
        {
            packet.ReadByte();
            string notice = packet.ReadUnicodeString();

            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.GuildId);
            if (guild == null)
            {
                return;
            }

            session.Send(GuildPacket.GuildNoticeConfirm(notice));
            guild.BroadcastPacketGuild(GuildPacket.GuildNoticeChange(session.Player, notice));
        }

        private static void HandleListGuild(GameSession session, PacketReader packet)
        {
            byte toggle = packet.ReadByte(); // 00 = unlist, 01 = list

            Guild guild = GameServer.GuildManager.GetGuildByLeader(session.Player);
            if (guild == null)
            {
                return;
            }

            session.Send(GuildPacket.ListGuildConfirm(toggle));
            session.Send(GuildPacket.ListGuildUpdate(session.Player));
            // TODO: update guild listing
        }

        private static void HandleSubmitApplication(GameSession session, PacketReader packet)
        {
            long guildId = packet.ReadLong();

            Guild guild = GameServer.GuildManager.GetGuildById(guildId);
            if (guild == null)
            {
                return;
            }

            long guildApplicationId = GuidGenerator.Long();

            session.Send(GuildPacket.SubmitApplicationConfirm(guildApplicationId, guild));
            guild.BroadcastPacketGuild(GuildPacket.SendApplication(guildApplicationId, session.Player)); // maybe only sent to guild leader/jrs?
            // TODO: Create a way to store their applicationId with their player information
        }

        private static void HandleApplicationResponse(GameSession session, PacketReader packet)
        {
            long guildApplicationId = packet.ReadLong();
            byte response = packet.ReadByte();

            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.GuildId);
            if (guild == null)
            {
                return;
            }

            if (response == 0)
            {
                //TODO: reject packets
                return;
            }

            session.Send(GuildPacket.ApplicationResponseSend(guildApplicationId, response));
            // TODO: Send 0x06, 0x2F, 0x12, 0x20 to guild/guildLeader
            // TODO: Send 0x30, 0x5, 0x4B, 0x0, 0x20 to new member
        }

        private static void HandleGuildWindowRequest(GameSession session)
        {
            session.Send(GuildPacket.GuildWindowConfirm());
        }

        private static void HandleGuildListRequest(GameSession session)
        {
            List<Guild> guildList = GameServer.GuildManager.GetGuildList();
            guildList = guildList.OrderBy(g => g.Members.Count).ToList();
            session.Send(GuildPacket.DisplayGuildList(guildList));
        }

        private static void HandleUseBuff(GameSession session, PacketReader packet)
        {
            int buffId = packet.ReadInt();

            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.GuildId);
            if (guild == null)
            {
                return;
            }

            GuildBuff buff = GuildMetadataStorage.GetBuff(buffId);

            if (buff.Id > 1000)
            {
                if (!session.Player.Wallet.Meso.Modify(-buff.Cost))
                {
                    return;
                }
            }
            else
            {
                if (buff.Cost > guild.Funds)
                {
                    return;
                }
                guild.Funds -= buff.Cost;
            }

            session.Send(GuildPacket.ActivateBuff(buffId));
            // TODO: Send buff packet
        }

        private static void HandleEnterHouse(/*GameSession session, PacketReader packet*/)
        {
            // TODO
        }

        private static void HandleGuildDonate(GameSession session, PacketReader packet)
        {
            int donateQuantity = packet.ReadInt();
            int donationAmount = donateQuantity * 10000;

            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.GuildId);
            if (guild == null)
            {
                return;
            }

            if (!session.Player.Wallet.Meso.Modify(-donationAmount))
            {
                short noticeCode = 5121;
                session.Send(GuildPacket.ErrorNotice(noticeCode));
                return;
            }
            GuildContribution contribution = GuildMetadataStorage.GetContribution("donation");

            session.Player.GuildContribution += contribution.Value * donateQuantity;
            int guildFunds = guild.Funds + donationAmount;
            guild.Funds = guildFunds;

            session.Send(GuildPacket.UpdateGuildFunds(guild.Funds));
            session.Send(GuildPacket.UpdatePlayerDonation());
            session.Send(GuildPacket.UpdateGuildStatsNotice(0, donationAmount));
            session.Send(GuildPacket.UpdatePlayerContribution(session.Player, donateQuantity));
        }

        private static void HandleServices(GameSession session, PacketReader packet)
        {
            int service = packet.ReadInt();

            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.GuildId);
            if (guild == null)
            {
                return;
            }
            // TODO: Get Guild Service costs
            guild.BroadcastPacketGuild(GuildPacket.UpgradeService(session.Player, service));
            guild.BroadcastPacketGuild(GuildPacket.UpdateGuildFunds(0));
        }
    }
}
