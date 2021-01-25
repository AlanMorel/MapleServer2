﻿using System;
using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class GuildPacket
    {
        private enum GuildPacketMode : byte
        {
            UpdateGuild = 0x0,
            Create = 0x1,
            DisbandConfirm = 0x2,
            InviteConfirm = 0x3,
            SendInvite = 0x4,
            InviteResponseConfirm = 0x5,
            InviteNotification = 0x6,
            LeaveConfirm = 0x7,
            KickConfirm = 0x8,
            KickNotification = 0x9,
            RankChangeConfirm = 0xA,
            CheckInConfirm = 0xF,
            MemberBroadcastJoinNotice = 0x12,
            MemberLeaveNotice = 0x13,
            KickMember = 0x14,
            RankChangeNotice = 0x15,
            UpdatePlayerMessage = 0x16,
            AssignNewLeader = 0x19,
            GuildNoticeChange = 0x1A,
            ListGuildUpdate = 0x1E,
            MemberJoin = 0x20,
            SendApplication = 0x2D,
            UpdateGuildExp = 0x31,
            UpdateGuildFunds = 0x32,
            UpdatePlayerContribution = 0x33,
            UpgradeService = 0x39,
            TransferLeaderConfirm = 0x3D,
            GuildNoticeConfirm = 0x3E,
            ListGuildConfirm = 0x42,
            UpdateGuildTag = 0x4B,
            MemberNotice = 0x4C,
            ErrorNotice = 0x4D,
            SubmitApplicationConfirm = 0x51,
            ApplicationResponseSend = 0x53,
            GuildWindowConfirm = 0x54,
            DisplayGuildList = 0x55,
            ActivateBuff = 0x59,
            List = 0x5A,
            UpdateGuildStatsNotice = 0x5F,
            UpdatePlayerDonation = 0x6E,
        }

        public static Packet UpdateGuild(GameSession session, Guild guild)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.UpdateGuild);
            pWriter.WriteLong(guild.Id);
            pWriter.WriteUnicodeString(guild.Name);
            pWriter.WriteUnicodeString(""); // guildMark Url
            pWriter.WriteByte(0x3C); // guild.Capacity
            pWriter.WriteUnicodeString("");
            pWriter.WriteUnicodeString(""); // guild.Notice
            pWriter.WriteLong(guild.Leader.AccountId);
            pWriter.WriteLong(guild.Leader.CharacterId);
            pWriter.WriteUnicodeString(guild.Leader.Name);
            pWriter.WriteLong(guild.CreationTimestamp); // guild creation timestamp
            pWriter.WriteByte(0x0);
            pWriter.WriteInt(1000);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteByte(0x1);
            pWriter.WriteInt(0);
            pWriter.WriteInt(guild.Exp);
            pWriter.WriteInt(guild.Funds);
            pWriter.WriteByte(0x0);
            pWriter.WriteInt(0);
            pWriter.WriteByte((byte) guild.Members.Count);

            foreach (Player member in guild.Members)
            {
                pWriter.WriteByte(0x3);
                pWriter.WriteByte(3); // rank
                pWriter.WriteLong(1); // guildmember UID?
                pWriter.WriteLong(member.AccountId);
                pWriter.WriteLong(member.CharacterId);
                pWriter.WriteUnicodeString(member.Name);
                pWriter.WriteByte();
                pWriter.WriteEnum(member.Job);
                pWriter.WriteEnum(member.JobCode);
                pWriter.WriteShort(member.Levels.Level);
                pWriter.WriteInt(member.MapId);
                pWriter.WriteInt(); // last seen mapID?
                pWriter.WriteShort(); // player.channel
                pWriter.WriteUnicodeString(member.ProfileUrl);
                pWriter.WriteInt(); // member house mapId
                pWriter.WriteInt();
                pWriter.WriteInt();
                pWriter.WriteLong(); // member house plot expiration timestamp
                pWriter.WriteInt(); // combat trophy count
                pWriter.WriteInt(); // adventure trophy count
                pWriter.WriteInt(); // lifestyle trophy count
                pWriter.WriteUnicodeString("");
                pWriter.WriteLong(); // joined timestamp
                pWriter.WriteLong(); // last seen timestamp
                pWriter.WriteLong();
                pWriter.WriteInt();
                pWriter.WriteInt();
                pWriter.WriteInt();
                pWriter.WriteInt();
                pWriter.WriteInt();
                pWriter.WriteLong();
                pWriter.WriteInt();
                pWriter.WriteByte(); // 00 = online, 01 = offline
            }

            pWriter.WriteByte(0x6); // loop for guild ranks ?
            pWriter.WriteByte(0x0);
            pWriter.WriteUnicodeString("Leader");
            pWriter.WriteInt(4095);
            pWriter.WriteByte(0x1);
            pWriter.WriteUnicodeString("Jr.Leader");
            pWriter.WriteInt(1);
            pWriter.WriteByte(0x2);
            pWriter.WriteUnicodeString("Member");
            pWriter.WriteInt(1);
            pWriter.WriteByte(0x3);
            pWriter.WriteUnicodeString("NewMember");
            pWriter.WriteInt(1);
            pWriter.WriteByte(0x4);
            pWriter.WriteUnicodeString("Member2");
            pWriter.WriteInt(1);
            pWriter.WriteByte(0x5);
            pWriter.WriteUnicodeString("NewMember2");
            pWriter.WriteInt(1);
            pWriter.WriteByte(0x9); // loop for all guild skills
            pWriter.WriteInt(1); // guild skill ID
            pWriter.WriteInt(1); // guild skill level
            pWriter.WriteLong(0);
            pWriter.WriteInt(2);
            pWriter.WriteInt(1);
            pWriter.WriteLong(0);
            pWriter.WriteInt(3);
            pWriter.WriteInt(1);
            pWriter.WriteLong(0);
            pWriter.WriteInt(4);
            pWriter.WriteInt(1);
            pWriter.WriteLong(0);
            pWriter.WriteInt(10001);
            pWriter.WriteInt(1);
            pWriter.WriteLong(0);
            pWriter.WriteInt(10002);
            pWriter.WriteInt(1);
            pWriter.WriteLong(0);
            pWriter.WriteInt(10003);
            pWriter.WriteInt(1);
            pWriter.WriteLong(0);
            pWriter.WriteInt(10004);
            pWriter.WriteInt(1);
            pWriter.WriteLong(0);
            pWriter.WriteInt(10005);
            pWriter.WriteInt(1);
            pWriter.WriteLong(0);
            pWriter.WriteByte(0x4); // another loop. unk
            pWriter.WriteInt(1);
            pWriter.WriteInt(0);
            pWriter.WriteInt(2);
            pWriter.WriteInt(0);
            pWriter.WriteInt(3);
            pWriter.WriteInt(0);
            pWriter.WriteInt(4);
            pWriter.WriteInt(0);
            pWriter.WriteInt(1);
            pWriter.WriteInt(1);
            pWriter.WriteInt(0);
            pWriter.WriteByte(0x0);
            pWriter.WriteByte(0x1);
            pWriter.WriteShort(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteUnicodeString("");
            pWriter.WriteLong(0);
            pWriter.WriteLong(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            return pWriter;
        }

        public static Packet Create(string guildName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.Create);
            pWriter.WriteByte(0x0);
            pWriter.WriteUnicodeString(guildName);
            return pWriter;
        }

        public static Packet DisbandConfirm()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.DisbandConfirm);
            pWriter.WriteByte();
            return pWriter;
        }

        public static Packet InviteConfirm(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.InviteConfirm);
            pWriter.WriteUnicodeString(player.Name);
            return pWriter;
        }

        public static Packet SendInvite(Player inviter, Player invitee, Guild guild)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.SendInvite);
            pWriter.WriteLong(guild.Id);
            pWriter.WriteUnicodeString(guild.Name);
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteUnicodeString(inviter.Name);
            pWriter.WriteUnicodeString(invitee.Name);
            return pWriter;
        }

        public static Packet InviteResponseConfirm(Player inviter, Player invitee, Guild guild, short response)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.InviteResponseConfirm);
            pWriter.WriteLong(guild.Id);
            pWriter.WriteUnicodeString(guild.Name);
            pWriter.WriteShort();
            pWriter.WriteUnicodeString(inviter.Name);
            pWriter.WriteUnicodeString(invitee.Name);
            pWriter.WriteShort(response);
            return pWriter;
        }

        public static Packet InviteNotification(string inviteeName, short response)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.InviteNotification);
            pWriter.WriteUnicodeString(inviteeName);
            pWriter.WriteShort(response);
            return pWriter;
        }

        public static Packet LeaveConfirm()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.LeaveConfirm);
            return pWriter;
        }

        public static Packet KickConfirm(Player member)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.KickConfirm);
            pWriter.WriteUnicodeString(member.Name);
            return pWriter;
        }

        public static Packet KickNotification(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.KickNotification);
            pWriter.WriteUnicodeString(player.Name);
            return pWriter;
        }

        public static Packet RankChangeConfirm(string memberName, byte rank)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.RankChangeConfirm);
            pWriter.WriteUnicodeString(memberName);
            pWriter.WriteByte(rank);
            return pWriter;
        }

        public static Packet CheckInConfirm()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.CheckInConfirm);
            return pWriter;
        }

        public static Packet MemberBroadcastJoinNotice(Player member, string inviterName, byte response, byte rank)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.MemberBroadcastJoinNotice);
            pWriter.WriteUnicodeString(inviterName);
            pWriter.WriteUnicodeString(member.Name);
            pWriter.WriteByte(response); // 01 = display notice
            pWriter.WriteByte(0x3);
            pWriter.WriteByte(rank);
            pWriter.WriteLong(GuidGenerator.Long()); // unk guild member UID?
            pWriter.WriteLong(member.AccountId);
            pWriter.WriteLong(member.CharacterId);
            pWriter.WriteUnicodeString(member.Name);
            pWriter.WriteByte(); //unk
            pWriter.WriteEnum(member.Job);
            pWriter.WriteEnum(member.JobCode);
            pWriter.WriteShort(member.Levels.Level);
            pWriter.WriteInt(); //unk
            pWriter.WriteInt(member.MapId);
            pWriter.WriteShort(); // player.Channel
            pWriter.WriteUnicodeString(member.ProfileUrl);
            pWriter.WriteInt(); // player home mapID
            pWriter.WriteInt(); // unk
            pWriter.WriteInt(); // unk
            pWriter.WriteLong(); // player home mapID expiration time
            pWriter.WriteInt(); // combat trophy count
            pWriter.WriteInt(); // adventure trophy count
            pWriter.WriteInt(); // lifestyle trophy count
            pWriter.WriteShort(); // unk, filler?
            pWriter.WriteLong(); // timestamp
            pWriter.WriteLong(); // timestamp
            pWriter.WriteLong(); // timestamp
            pWriter.WriteLong(); // unk
            pWriter.WriteLong(); // unk
            pWriter.WriteLong(); // unk
            pWriter.WriteLong(); // unk
            pWriter.WriteByte(); // unk 
            return pWriter;
        }

        public static Packet MemberLeaveNotice(Player member)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.MemberLeaveNotice);
            pWriter.WriteUnicodeString(member.Name);
            return pWriter;
        }

        public static Packet KickMember(Player member, Player leader)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.KickMember);
            pWriter.WriteUnicodeString(leader.Name);
            pWriter.WriteUnicodeString(member.Name);
            return pWriter;
        }

        public static Packet RankChangeNotice(string leaderName, string memberName, byte rank)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.RankChangeNotice);
            pWriter.WriteUnicodeString(leaderName);
            pWriter.WriteUnicodeString(memberName);
            pWriter.WriteByte(rank);
            return pWriter;
        }

        public static Packet UpdatePlayerMessage(Player player, string message)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.UpdatePlayerMessage);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteUnicodeString(message);
            return pWriter;
        }

        public static Packet AssignNewLeader(Player newLeader, Player oldLeader)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.AssignNewLeader);
            pWriter.WriteUnicodeString(oldLeader.Name);
            pWriter.WriteUnicodeString(newLeader.Name);
            return pWriter;
        }

        public static Packet GuildNoticeChange(Player player, string notice)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.GuildNoticeChange);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteByte(0x1);
            pWriter.WriteUnicodeString(notice);
            return pWriter;
        }

        public static Packet ListGuildUpdate(Player player, byte toggle)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.ListGuildUpdate);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteByte(0x1);
            pWriter.WriteInt();
            return pWriter;
        }

        public static Packet MemberJoin(Player player, string guildName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.MemberJoin);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteLong(player.AccountId);
            pWriter.WriteLong(player.CharacterId);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteByte(); //unk
            pWriter.WriteEnum(player.Job);
            pWriter.WriteEnum(player.JobCode);
            pWriter.WriteShort(player.Levels.Level);
            pWriter.WriteInt(); //unk
            pWriter.WriteInt(player.MapId);
            pWriter.WriteShort(); // player.Channel
            pWriter.WriteUnicodeString(player.ProfileUrl);
            pWriter.WriteInt(); // player home mapID
            pWriter.WriteInt(); // unk
            pWriter.WriteInt(); // unk
            pWriter.WriteLong(); // player home mapID expiration time
            pWriter.WriteInt(); // combat trophy count
            pWriter.WriteInt(); // adventure trophy count
            pWriter.WriteInt(); // lifestyle trophy count
            return pWriter;
        }

        public static Packet SendApplication(long guildApplicationId, Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.SendApplication);
            pWriter.WriteLong(guildApplicationId);
            pWriter.WriteLong(); // guild member UID?
            pWriter.WriteLong(player.AccountId);
            pWriter.WriteLong(player.CharacterId);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteUnicodeString(player.ProfileUrl);
            pWriter.WriteEnum(player.Job);
            pWriter.WriteEnum(player.JobCode);
            pWriter.WriteInt(player.Levels.Level);
            pWriter.WriteInt(); // combat trophy count
            pWriter.WriteInt(); // adventure trophy count
            pWriter.WriteInt(); // lifestyle trophy count
            pWriter.WriteLong(DateTimeOffset.Now.ToUnixTimeSeconds() + Environment.TickCount);
            return pWriter;
        }

        public static Packet UpdateGuildExp(int guildExp)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.UpdateGuildExp);
            pWriter.WriteInt(guildExp);
            return pWriter;
        }

        public static Packet UpdateGuildFunds(int guildFunds)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.UpdateGuildFunds);
            pWriter.WriteInt(guildFunds);
            return pWriter;
        }

        public static Packet UpdatePlayerContribution(Player player, int contribution)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.UpdatePlayerContribution);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteInt(contribution);
            pWriter.WriteInt(contribution);
            pWriter.WriteInt(player.GuildContribution);
            return pWriter;
        }

        public static Packet UpgradeService(Player player, int service)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.UpgradeService);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteInt(service);
            pWriter.WriteInt(1); // level of service
            return pWriter;
        }

        public static Packet TransferLeaderConfirm(Player newLeader)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.TransferLeaderConfirm);
            pWriter.WriteUnicodeString(newLeader.Name);
            return pWriter;
        }

        public static Packet SubmitApplicationConfirm(long guildApplicationId, Guild guild)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.SubmitApplicationConfirm);
            pWriter.WriteLong(guildApplicationId);
            pWriter.WriteUnicodeString(guild.Name);
            return pWriter;
        }

        public static Packet ApplicationResponseSend(long guildApplicationId, byte response)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.ApplicationResponseSend);
            pWriter.WriteLong(guildApplicationId);
            pWriter.WriteUnicodeString(""); // member.Name
            pWriter.WriteByte(response);
            return pWriter;
        }

        public static Packet GuildNoticeConfirm(string notice)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.GuildNoticeConfirm);
            pWriter.WriteByte(0x1);
            pWriter.WriteUnicodeString(notice);
            return pWriter;
        }

        public static Packet ListGuildConfirm(byte toggle)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.ListGuildConfirm);
            pWriter.WriteByte(toggle);
            pWriter.WriteInt();
            return pWriter;
        }

        public static Packet UpdateGuildTag(Player player, string guildName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.UpdateGuildTag);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteUnicodeString(guildName);
            return pWriter;
        }

        public static Packet MemberNotice(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.MemberNotice);
            pWriter.WriteUnicodeString(player.Name);
            return pWriter;
        }

        public static Packet ErrorNotice(short code)
        {
            /*
            1027 = Unable to invite a member in a guild 
            2563 = Unable to invite a member if not online.
            5121 = Deny guild create
             */
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.ErrorNotice);
            pWriter.WriteShort(code);
            pWriter.WriteInt();
            return pWriter;
        }

        public static Packet GuildWindowConfirm()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.GuildWindowConfirm);
            pWriter.WriteInt();
            return pWriter;
        }

        public static Packet DisplayGuildList(List<Guild> guilds)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.DisplayGuildList);
            pWriter.WriteInt(guilds.Count);

            foreach (Guild guild in guilds)
            {
                pWriter.WriteByte(0x1);
                pWriter.WriteLong(guild.Id);
                pWriter.WriteUnicodeString(guild.Name);
                pWriter.WriteUnicodeString(""); // guildMarkUrl
                pWriter.WriteInt(); // combat trophy accummulative total
                pWriter.WriteInt(); // adventure trophy accummulative total
                pWriter.WriteInt(); // lifestyle trophy accummulative total
                pWriter.WriteInt(guild.Members.Count);
                pWriter.WriteInt(guild.MaxMembers);
                pWriter.WriteInt(guild.Exp);
                pWriter.WriteLong(guild.Leader.AccountId);
                pWriter.WriteLong(guild.Leader.CharacterId);
                pWriter.WriteUnicodeString(guild.Leader.Name);
            }
            return pWriter;
        }

        public static Packet ActivateBuff(int buffID)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.ActivateBuff);
            pWriter.WriteInt(buffID);
            return pWriter;
        }

        public static Packet List()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.List);
            pWriter.WriteByte(0x1);
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteShort();
            pWriter.WriteUnicodeString(""); //GuildMark url
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt(); //guild Member Total
            pWriter.WriteInt(); //guild Capacity
            return pWriter;
        }

        public static Packet UpdateGuildStatsNotice(int exp, int funds)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.UpdateGuildStatsNotice);
            pWriter.WriteInt(exp);
            pWriter.WriteInt(funds);
            return pWriter;
        }

        public static Packet UpdatePlayerDonation(int totalDonationAmount)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.UpdatePlayerDonation);
            pWriter.WriteInt(0x3); // total amount of donations today
            pWriter.WriteLong(DateTimeOffset.Now.ToUnixTimeSeconds() + Environment.TickCount);
            return pWriter;
        }
    }
}
