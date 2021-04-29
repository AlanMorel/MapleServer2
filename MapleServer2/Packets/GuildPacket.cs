using System;
using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
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
            CheckInBegin = 0xF,
            MemberBroadcastJoinNotice = 0x12,
            MemberLeaveNotice = 0x13,
            KickMember = 0x14,
            RankChangeNotice = 0x15,
            UpdatePlayerMessage = 0x16,
            AssignNewLeader = 0x19,
            GuildNoticeChange = 0x1A,
            UpdateRankNotice = 0x1D,
            ListGuildUpdate = 0x1E,
            MemberJoin = 0x20,
            GuildNameChange = 0x22,
            FinishCheckIn = 0x24,
            BattleMatchmaking = 0x2A,
            BattleApplyNotice = 0x2B,
            BattleCancelApplyNotice = 0x2C,
            SendApplication = 0x2D,
            WithdrawApplicationGuildUpdate = 0x2E,
            ApplicationResponseBroadcastNotice = 0x2F,
            ApplicationResponseToApplier = 0x30,
            UpdateGuildExp = 0x31,
            UpdateGuildFunds = 0x32,
            UpdatePlayerContribution = 0x33,
            UpgradeBuff = 0x35,
            StartMiniGame = 0x36,
            ChangeHouse = 0x37,
            UpgradeService = 0x39,
            RequestMiniGameWar = 0x3B,
            TransferLeaderConfirm = 0x3D,
            GuildNoticeConfirm = 0x3E,
            UpdateRankConfirm = 0x41,
            ListGuildConfirm = 0x42,
            SendMail = 0x45,
            UpdateGuildTag2 = 0x4B,
            UpdateGuildTag = 0x4C,
            ErrorNotice = 0x4D,
            SubmitApplication = 0x50,
            WithdrawApplicationPlayerUpdate = 0x51,
            ApplicationResponse = 0x52,
            LoadApplications = 0x54,
            DisplayGuildList = 0x55,
            UseBuffNotice = 0x58,
            ActivateBuff = 0x59,
            List = 0x5A,
            UpdateGuildStatsNotice = 0x5F,
            StartMiniGame2 = 0x60,
            UpdatePlayerDonation = 0x6E,
        }

        public static Packet UpdateGuild(Guild guild)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.UpdateGuild);
            pWriter.WriteLong(guild.Id);
            pWriter.WriteUnicodeString(guild.Name);
            pWriter.WriteUnicodeString(guild.Emblem);
            pWriter.WriteByte(guild.Capacity);
            pWriter.WriteUnicodeString("");
            pWriter.WriteUnicodeString(guild.Notice);
            pWriter.WriteLong(guild.Leader.AccountId);
            pWriter.WriteLong(guild.Leader.CharacterId);
            pWriter.WriteUnicodeString(guild.Leader.Name);
            pWriter.WriteLong(guild.CreationTimestamp);
            pWriter.WriteByte(0x1);
            pWriter.WriteInt(1000);
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt(); // total trophies
            pWriter.WriteByte(0x1);
            pWriter.WriteInt(guild.FocusAttributes);
            pWriter.WriteInt(guild.Exp);
            pWriter.WriteInt(guild.Funds);
            pWriter.WriteByte(0x0);
            pWriter.WriteInt(0);
            pWriter.WriteByte((byte) guild.Members.Count);

            foreach (GuildMember member in guild.Members)
            {
                pWriter.WriteByte(0x3);
                pWriter.WriteByte(member.Rank);
                pWriter.WriteLong(member.Player.CharacterId);
                WriteGuildMember(pWriter, member.Player);
                pWriter.WriteUnicodeString(member.Motto);
                pWriter.WriteLong(member.JoinTimestamp);
                pWriter.WriteLong(); // last seen timestamp
                pWriter.WriteLong(member.AttendanceTimestamp);
                pWriter.WriteInt();
                pWriter.WriteInt();
                pWriter.WriteInt(member.DailyContribution);
                pWriter.WriteInt(member.ContributionTotal);
                pWriter.WriteInt(member.DailyDonationCount);
                pWriter.WriteLong();
                pWriter.WriteInt();
                pWriter.WriteByte(); // 00 = online, 01 = offline
            }

            pWriter.WriteByte((byte) guild.Ranks.Length);
            for (int i = 0; i < guild.Ranks.Length; i++)
            {
                pWriter.WriteByte((byte) i);
                pWriter.WriteUnicodeString(guild.Ranks[i].Name);
                pWriter.WriteInt(guild.Ranks[i].Rights);
            }

            pWriter.WriteByte((byte) guild.Buffs.Count);
            foreach (GuildBuff buff in guild.Buffs)
            {
                pWriter.WriteInt(buff.Id);
                pWriter.WriteInt(buff.Level);
                pWriter.WriteLong(buff.StartTimestamp);
            }

            pWriter.WriteByte(0x4); // another loop. unk
            pWriter.WriteInt(1);
            pWriter.WriteInt(0);
            pWriter.WriteInt(2);
            pWriter.WriteInt(0);
            pWriter.WriteInt(3);
            pWriter.WriteInt(0);
            pWriter.WriteInt(4);
            pWriter.WriteInt(0);
            pWriter.WriteInt(guild.HouseRank);
            pWriter.WriteInt(guild.HouseTheme);
            pWriter.WriteInt(0); // for guild posters

            pWriter.WriteByte((byte) guild.Services.Count);
            foreach (GuildService service in guild.Services)
            {
                pWriter.WriteInt(service.Id);
                pWriter.WriteInt(service.Level);
            }
            pWriter.WriteByte();
            pWriter.WriteShort();

            pWriter.WriteInt(guild.GiftBank.Count);
            foreach (Item item in guild.GiftBank)
            {
                pWriter.WriteInt(item.Id);
                pWriter.WriteShort((short) item.Rarity);
                pWriter.WriteInt(item.Amount);
                pWriter.WriteByte();
                pWriter.WriteByte();
                pWriter.WriteByte();
                pWriter.WriteByte();
            }

            pWriter.WriteInt(0);
            pWriter.WriteUnicodeString("");
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
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

        public static Packet CheckInBegin()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.CheckInBegin);
            return pWriter;
        }

        public static Packet MemberBroadcastJoinNotice(GuildMember member, string inviterName, bool displayNotice)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.MemberBroadcastJoinNotice);
            pWriter.WriteUnicodeString(inviterName);
            pWriter.WriteUnicodeString(member.Player.Name);
            pWriter.WriteBool(displayNotice);
            pWriter.WriteByte(0x3);
            pWriter.WriteByte(member.Rank);
            pWriter.WriteLong(member.Player.CharacterId);
            WriteGuildMember(pWriter, member.Player);
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

        public static Packet UpdateRankNotice(Guild guild, byte rankIndex)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.UpdateRankNotice);
            pWriter.WriteByte();
            pWriter.WriteInt();
            pWriter.WriteUnicodeString(guild.Leader.Name);
            pWriter.WriteByte(rankIndex);
            pWriter.WriteByte(rankIndex);
            pWriter.WriteUnicodeString(guild.Ranks[rankIndex].Name);
            pWriter.WriteInt(guild.Ranks[rankIndex].Rights);
            return pWriter;
        }

        public static Packet ListGuildUpdate(Player player, bool toggle)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.ListGuildUpdate);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteBool(toggle);
            pWriter.WriteInt();
            return pWriter;
        }

        public static Packet MemberJoin(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.MemberJoin);
            pWriter.WriteUnicodeString(player.Name);
            WriteGuildMember(pWriter, player);
            return pWriter;
        }

        public static Packet GuildNameChange(string newName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.GuildNameChange);
            pWriter.WriteUnicodeString(newName);
            return pWriter;
        }

        public static Packet FinishCheckIn(GuildMember member)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.FinishCheckIn);
            pWriter.WriteUnicodeString(member.Player.Name);
            pWriter.WriteLong(member.AttendanceTimestamp);
            return pWriter;
        }

        public static Packet BattleMatchmaking(bool success)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.BattleMatchmaking);
            pWriter.WriteBool(success);
            return pWriter;
        }

        public static Packet BattleApplyNotice(string playerName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.BattleApplyNotice);
            pWriter.WriteUnicodeString(playerName);
            return pWriter;
        }

        public static Packet BattleCancelApplyNotice(string playerName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.BattleCancelApplyNotice);
            pWriter.WriteUnicodeString(playerName);
            return pWriter;
        }

        public static Packet SendApplication(GuildApplication application, Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.SendApplication);
            pWriter.WriteLong(application.Id);
            pWriter.WriteLong(application.GuildId);
            pWriter.WriteLong(player.AccountId);
            pWriter.WriteLong(player.CharacterId);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteUnicodeString(player.ProfileUrl);
            pWriter.WriteEnum(player.Job);
            pWriter.WriteEnum(player.JobCode);
            pWriter.WriteInt(player.Levels.Level);
            foreach (int trophyCategory in player.TrophyCount)
            {
                pWriter.WriteInt(trophyCategory);
            }
            pWriter.WriteLong(DateTimeOffset.Now.ToUnixTimeSeconds() + Environment.TickCount);
            return pWriter;
        }
        public static Packet WithdrawApplicationGuildUpdate(long applicationId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.WithdrawApplicationGuildUpdate);
            pWriter.WriteLong(applicationId);
            return pWriter;
        }

        public static Packet ApplicationResponseBroadcastNotice(string reviewerName, string applierName, byte response, long applicationId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.ApplicationResponseBroadcastNotice);
            pWriter.WriteUnicodeString(reviewerName);
            pWriter.WriteUnicodeString(applierName);
            pWriter.WriteByte(response);
            pWriter.WriteLong(applicationId);
            return pWriter;
        }

        public static Packet ApplicationResponseToApplier(string guildName, long applicationId, byte response)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.ApplicationResponseToApplier);
            pWriter.WriteUnicodeString(guildName);
            pWriter.WriteLong(applicationId);
            pWriter.WriteByte(response);
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

        public static Packet UpdatePlayerContribution(GuildMember member, int contribution)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.UpdatePlayerContribution);
            pWriter.WriteUnicodeString(member.Player.Name);
            pWriter.WriteInt(contribution);
            pWriter.WriteInt(member.DailyContribution);
            pWriter.WriteInt(member.ContributionTotal);
            return pWriter;
        }

        public static Packet UpgradeBuff(int buffId, int buffLevel, string playerName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.UpgradeBuff);
            pWriter.WriteUnicodeString(playerName);
            pWriter.WriteInt(buffId);
            pWriter.WriteInt(buffLevel);
            pWriter.WriteLong();
            return pWriter;
        }

        public static Packet StartMinigame(string playerName, int minigameId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.StartMiniGame);
            pWriter.WriteUnicodeString(playerName);
            pWriter.WriteInt(minigameId);
            pWriter.WriteInt();
            return pWriter;
        }

        public static Packet ChangeHouse(string playerName, int houseRank, int houseTheme)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.ChangeHouse);
            pWriter.WriteUnicodeString(playerName);
            pWriter.WriteInt(houseRank);
            pWriter.WriteInt(houseTheme);
            return pWriter;
        }

        public static Packet UpgradeService(Player player, int serviceId, int serviceLevel)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.UpgradeService);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteInt(serviceId);
            pWriter.WriteInt(serviceLevel);
            return pWriter;
        }

        public static Packet RequestMiniGameWar(bool request)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.RequestMiniGameWar);
            pWriter.WriteBool(request);
            pWriter.WriteByte(0x1);
            return pWriter;
        }

        public static Packet TransferLeaderConfirm(Player newLeader)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.TransferLeaderConfirm);
            pWriter.WriteUnicodeString(newLeader.Name);
            return pWriter;
        }

        public static Packet SubmitApplication(GuildApplication application, string guildName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.SubmitApplication);
            pWriter.WriteLong(application.Id);
            pWriter.WriteUnicodeString(guildName);
            return pWriter;
        }

        public static Packet WithdrawApplicationPlayerUpdate(GuildApplication application, string guildName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.WithdrawApplicationPlayerUpdate);
            pWriter.WriteLong(application.Id);
            pWriter.WriteUnicodeString(guildName);
            return pWriter;
        }

        public static Packet ApplicationResponse(long guildApplicationId, string applierName, byte response)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.ApplicationResponse);
            pWriter.WriteLong(guildApplicationId);
            pWriter.WriteUnicodeString(applierName);
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

        public static Packet UpdateRankConfirm(Guild guild, byte rankIndex)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.UpdateRankConfirm);
            pWriter.WriteByte(rankIndex);
            pWriter.WriteByte(rankIndex);
            pWriter.WriteUnicodeString(guild.Ranks[rankIndex].Name);
            pWriter.WriteInt(guild.Ranks[rankIndex].Rights);
            return pWriter;
        }

        public static Packet ListGuildConfirm(bool toggle)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.ListGuildConfirm);
            pWriter.WriteBool(toggle);
            pWriter.WriteInt();
            return pWriter;
        }

        public static Packet SendMail()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.SendMail);
            return pWriter;
        }

        public static Packet UpdateGuildTag2(Player player, string guildName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.UpdateGuildTag2);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteUnicodeString(guildName);
            return pWriter;
        }

        public static Packet UpdateGuildTag(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.UpdateGuildTag);
            pWriter.WriteUnicodeString(player.Name);
            return pWriter;
        }

        public static Packet ErrorNotice(byte errorNotice, int param = 0)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.ErrorNotice);
            pWriter.WriteByte(1);
            pWriter.WriteByte(errorNotice);
            pWriter.WriteInt(param);
            return pWriter;
        }

        public static Packet LoadApplications(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.LoadApplications);
            pWriter.WriteInt(player.GuildApplications.Count);
            foreach (GuildApplication application in player.GuildApplications)
            {
                Guild guild = GameServer.GuildManager.GetGuildById(application.GuildId);
                pWriter.WriteByte(0x1);
                pWriter.WriteLong(application.Id);
                pWriter.WriteLong(guild.Id);
                pWriter.WriteUnicodeString(guild.Name);
                pWriter.WriteInt(guild.Exp);
                pWriter.WriteUnicodeString(guild.Emblem);
                pWriter.WriteInt(); // combat trophy accummulative total
                pWriter.WriteInt(); // adventure trophy accummulative total
                pWriter.WriteInt(); // lifestyle trophy accummulative total
                pWriter.WriteInt(guild.Members.Count);
                pWriter.WriteInt(guild.Capacity);
                pWriter.WriteUnicodeString(guild.Leader.Name);
                pWriter.WriteLong(player.AccountId);
                pWriter.WriteLong(player.CharacterId);
                pWriter.WriteLong(application.CreationTimestamp);
            }
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
                pWriter.WriteUnicodeString(guild.Emblem);
                pWriter.WriteInt(); // combat trophy accummulative total
                pWriter.WriteInt(); // adventure trophy accummulative total
                pWriter.WriteInt(); // lifestyle trophy accummulative total
                pWriter.WriteInt(guild.Members.Count);
                pWriter.WriteInt(guild.Capacity);
                pWriter.WriteInt(guild.Exp);
                pWriter.WriteLong(guild.Leader.AccountId);
                pWriter.WriteLong(guild.Leader.CharacterId);
                pWriter.WriteUnicodeString(guild.Leader.Name);
            }
            return pWriter;
        }

        public static Packet UseBuffNotice(int buffID)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.UseBuffNotice);
            pWriter.WriteInt(buffID);
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

        public static Packet StartMiniGame(int minigameId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.StartMiniGame);
            pWriter.WriteInt(minigameId);
            return pWriter;
        }

        public static Packet UpdatePlayerDonation(/*int totalDonationAmount*/)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteEnum(GuildPacketMode.UpdatePlayerDonation);
            pWriter.WriteInt(0x3); // total amount of donations today
            pWriter.WriteLong(DateTimeOffset.Now.ToUnixTimeSeconds() + Environment.TickCount);
            return pWriter;
        }

        public static void WriteGuildMember(PacketWriter pWriter, Player member)
        {
            pWriter.WriteLong(member.AccountId);
            pWriter.WriteLong(member.CharacterId);
            pWriter.WriteUnicodeString(member.Name);
            pWriter.WriteByte();
            pWriter.WriteEnum(member.Job);
            pWriter.WriteEnum(member.JobCode);
            pWriter.WriteShort(member.Levels.Level);
            pWriter.WriteInt(); // player gearscore
            pWriter.WriteInt(member.MapId);
            pWriter.WriteShort(); // player.channel
            pWriter.WriteUnicodeString(member.ProfileUrl);
            pWriter.WriteInt(member.PlotMapId);
            pWriter.WriteInt(member.HomePlotNumber);
            pWriter.WriteInt(member.ApartmentNumber);
            pWriter.WriteLong(member.HomeExpiration);
            foreach (int trophyCategory in member.TrophyCount)
            {
                pWriter.WriteInt(trophyCategory);
            }
        }
    }
}
