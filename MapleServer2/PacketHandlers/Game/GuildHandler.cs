using System;
using System.Collections.Generic;
using System.Linq;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
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
                    HandleDisband(session, packet);
                    break;
                case GuildMode.Invite:
                    HandleInvite(session, packet);
                    break;
                case GuildMode.InviteResponse:
                    HandleInviteResponse(session, packet);
                    break;
                case GuildMode.Leave:
                    HandleLeave(session, packet);
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
                    HandleCheckIn(session, packet);
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
                    HandleGuildWindowRequest(session, packet);
                    break;
                case GuildMode.GuildListRequest:
                    HandleGuildListRequest(session, packet);
                    break;
                case GuildMode.UseBuff:
                    HandleUseBuff(session, packet);
                    break;
                case GuildMode.EnterHouse:
                    HandleEnterHouse(session, packet);
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

        private void HandleCreate(GameSession session, PacketReader packet)
        {
            string guildName = packet.ReadUnicodeString();

            if (!session.Player.Wallet.Meso.Modify(-2000))
            {
                short NoticeCode = 5121;
                session.Send(GuildPacket.ErrorNotice(NoticeCode));
                return;
            }

            session.FieldManager.BroadcastPacket(GuildPacket.UpdateGuildTag(session.Player, guildName));
            session.Send(GuildPacket.Create(guildName));

            string inviter = ""; // nobody because nobody invited the guild leader
            byte response = 0; // 0 to not display invite notification
            byte rank = 0; // set to leader rank

            Guild newGuild = new(guildName, new List<Player> { session.Player });
            GameServer.GuildManager.AddGuild(newGuild);

            session.Send(GuildPacket.UpdateGuild(session, newGuild));
            session.Send(GuildPacket.MemberBroadcastJoinNotice(session.Player, inviter, response, rank));
            session.Send(GuildPacket.MemberJoin(session.Player, guildName));
        }

        private void HandleDisband(GameSession session, PacketReader packet)
        {
            Guild guild = GameServer.GuildManager.GetGuildByLeader(session.Player);
            if (guild == null)
            {
                return;
            }

            session.Send(GuildPacket.DisbandConfirm());
            guild.BroadcastPacketGuild(GuildPacket.MemberNotice(session.Player));
            GameServer.GuildManager.RemoveGuild(guild);
        }

        private void HandleInvite(GameSession session, PacketReader packet)
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
                short NoticeCode = 2563;
                session.Send(GuildPacket.ErrorNotice(NoticeCode));
            }

            if (playerInvited.GuildId != 0)
            {
                short NoticeCode = 1027;
                session.Send(GuildPacket.ErrorNotice(NoticeCode));
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

        private void HandleInviteResponse(GameSession session, PacketReader packet)
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
            guild.BroadcastPacketGuild(GuildPacket.MemberBroadcastJoinNotice(session.Player, inviterName, response, rank));
            guild.BroadcastPacketGuild(GuildPacket.MemberJoin(session.Player, guildName));
            session.Send(GuildPacket.UpdateGuild(session, guild));
        }

        private void HandleLeave(GameSession session, PacketReader packet)
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

        private void HandleKick(GameSession session, PacketReader packet)
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

        private void HandleRankChange(GameSession session, PacketReader packet)
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

        private void HandlePlayerMessage(GameSession session, PacketReader packet)
        {
            string message = packet.ReadUnicodeString();

            session.Send(GuildPacket.UpdatePlayerMessage(session.Player, message));
        }

        private void HandleCheckIn(GameSession session, PacketReader packet)
        {
            session.Send(GuildPacket.CheckInConfirm());
            // TODO: Send Guild Coins
            // TODO: Can only check in once a day
            session.Send(GuildPacket.UpdateGuildFunds());
            session.Send(GuildPacket.UpdateGuildExp());
            session.Send(GuildPacket.UpdateGuildFunds2());
            session.Send(GuildPacket.UpdatePlayerContribution(session.Player));
            session.Send(GuildPacket.UpdatePlayerContribution(session.Player));
        }

        private void HandleTransferLeader(GameSession session, PacketReader packet)
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

        private void HandleGuildNotice(GameSession session, PacketReader packet)
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

        private void HandleListGuild(GameSession session, PacketReader packet)
        {
            byte toggle = packet.ReadByte(); // 00 = unlist, 01 = list

            Guild guild = GameServer.GuildManager.GetGuildByLeader(session.Player);
            if (guild == null)
            {
                return;
            }

            session.Send(GuildPacket.ListGuildConfirm(toggle));
            session.Send(GuildPacket.ListGuildUpdate(session.Player, toggle));
            // TODO: update guild listing
        }

        private void HandleSubmitApplication(GameSession session, PacketReader packet)
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

        private void HandleApplicationResponse(GameSession session, PacketReader packet)
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

        private void HandleGuildWindowRequest(GameSession session, PacketReader packet)
        {
            session.Send(GuildPacket.GuildWindowConfirm());
        }

        private void HandleGuildListRequest(GameSession session, PacketReader packet)
        {
            List<Guild> guildList = GameServer.GuildManager.GetGuildList();
            guildList = guildList.OrderBy(g => g.Members.Count).ToList();
            session.Send(GuildPacket.DisplayGuildList(guildList));
        }

        private void HandleUseBuff(GameSession session, PacketReader packet)
        {
            int buffID = packet.ReadInt();

            if (!session.Player.Wallet.Meso.Modify(-100000))
            {
                return;
            }

            session.Send(GuildPacket.ActivateBuff(buffID));
            // TODO: Send buff packet
        }

        private void HandleEnterHouse(GameSession session, PacketReader packet)
        {
            // TODO
        }

        private void HandleGuildDonate(GameSession session, PacketReader packet)
        {
            int donateQuantity = packet.ReadInt();

            session.Send(GuildPacket.UpdateGuildFunds());
            session.Send(GuildPacket.UpdatePlayerDonation());
            session.Send(GuildPacket.UpdateGuildFunds2());
            session.Send(GuildPacket.UpdatePlayerContribution(session.Player));
        }

        private void HandleServices(GameSession session, PacketReader packet)
        {
            int service = packet.ReadInt();

            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.GuildId);
            if (guild == null)
            {
                return;
            }

            guild.BroadcastPacketGuild(GuildPacket.UpgradeService(session.Player, service));
            guild.BroadcastPacketGuild(GuildPacket.UpdateGuildFunds());
        }
    }
}
