using System;
using System.Collections.Generic;
using System.Linq;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
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
            UpdateRank = 0x41,
            ListGuild = 0x42,
            SubmitApplication = 0x50,
            WithdrawApplication = 0x51,
            ApplicationResponse = 0x52,
            LoadApplications = 0x54,
            LoadGuildList = 0x55,
            SearchGuildByName = 0x56,
            UseBuff = 0x59,
            UpgradeBuff = 0x5A,
            UpgradeHome = 0x62,
            ChangeHomeTheme = 0x63,
            EnterHouse = 0x64,
            GuildDonate = 0x6E,
            Services = 0x6F,
        }

        private enum GuildErrorNotice : byte
        {
            GuildNotFound = 0x3,
            CharacterIsAlreadyInAGuild = 0x4,
            UnableToSendInvite = 0x5,
            InviteFailed = 0x6,
            UserAlreadyJoinedAGuild = 0x7,
            GuildNoLongerValid = 0x8,
            UnableToInvitePlayer = 0xA,
            GuildWithSameNameExists = 0xB,
            NameContainsForbiddenWord = 0xC,
            GuildMemberNotFound = 0xD,
            CannotDisbandWithMembers = 0xE,
            GuildIsAtCapacity = 0xF,
            GuildMemberHasNotJoined = 0x10,
            LeaderCannotLeaveGuild = 0x11,
            CannotKickLeader = 0x12,
            NotEnoughMesos = 0x14,
            InsufficientPermissions = 0x15,
            OnlyLeaderCanDoThis = 0x16,
            RankCannotBeUsed = 0x17,
            CannotChangeMaxCapacityToValue = 0x18,
            IncorrectRank = 0x19,
            RankCannotBeGranted = 0x1B,
            RankSettingFailed = 0x1C,
            CannotDoDuringGuildBattle = 0x21,
            ApplicationNotFound = 0x27,
            TargetIsInAnUninvitableLocation = 0x29,
            GuildLevelNotHighEnough = 0x2A,
            InsufficientGuildFunds = 0x2B,
            CannotUseGuildSkillsRightNow = 0x2C,
            YouAreAlreadyAtGloriousArena = 0x2E,
            ApplicationsAreNotAccepted = 0x2F,
            YouNeedAtLeastXPlayersOnline = 0x30
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
                case GuildMode.UpdateRank:
                    HandleUpdateRank(session, packet);
                    break;
                case GuildMode.ListGuild:
                    HandleListGuild(session, packet);
                    break;
                case GuildMode.SubmitApplication:
                    HandleSubmitApplication(session, packet);
                    break;
                case GuildMode.WithdrawApplication:
                    HandleWithdrawApplication(session, packet);
                    break;
                case GuildMode.ApplicationResponse:
                    HandleApplicationResponse(session, packet);
                    break;
                case GuildMode.LoadApplications:
                    HandleLoadApplications(session);
                    break;
                case GuildMode.LoadGuildList:
                    HandleLoadGuildList(session, packet);
                    break;
                case GuildMode.SearchGuildByName:
                    HandleSearchGuildByName(session, packet);
                    break;
                case GuildMode.UseBuff:
                    HandleUseBuff(session, packet);
                    break;
                case GuildMode.UpgradeBuff:
                    HandleUpgradeBuff(session, packet);
                    break;
                case GuildMode.UpgradeHome:
                    HandleUpgradeHome(session, packet);
                    break;
                case GuildMode.ChangeHomeTheme:
                    HandleChangeHomeTheme(session, packet);
                    break;
                case GuildMode.EnterHouse:
                    HandleEnterHouse(session);
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

            if (session.Player.Guild != null)
            {
                return;
            }

            if (!session.Player.Wallet.Meso.Modify(session, -2000))
            {
                session.Send(GuildPacket.ErrorNotice((byte) GuildErrorNotice.NotEnoughMesos));
                return;
            }

            if (DatabaseManager.GuildExists(guildName))
            {
                session.Send(GuildPacket.ErrorNotice((byte) GuildErrorNotice.GuildWithSameNameExists));
                return;
            }
            Guild newGuild = new Guild(guildName, session.Player);

            GameServer.GuildManager.AddGuild(newGuild);

            session.FieldManager.BroadcastPacket(GuildPacket.UpdateGuildTag2(session.Player, guildName));
            session.Send(GuildPacket.Create(guildName));

            string inviter = ""; // nobody because nobody invited the guild leader

            GuildMember member = newGuild.Members.FirstOrDefault(x => x.Player == session.Player);
            session.Send(GuildPacket.UpdateGuild(newGuild));
            session.Send(GuildPacket.MemberBroadcastJoinNotice(member, inviter, false));
            session.Send(GuildPacket.MemberJoin(session.Player));

            // Remove any applications
            foreach (GuildApplication application in session.Player.GuildApplications)
            {
                Guild guild = GameServer.GuildManager.GetGuildById(application.GuildId);
                application.Remove(session.Player, guild);
            }
            DatabaseManager.UpdateCharacter(session.Player);
        }

        private static void HandleDisband(GameSession session)
        {
            Guild guild = GameServer.GuildManager.GetGuildByLeader(session.Player);
            if (guild == null)
            {
                return;
            }

            // Remove any applications
            if (guild.Applications.Count > 0)
            {
                foreach (GuildApplication application in guild.Applications)
                {
                    Player player = GameServer.Storage.GetPlayerById(application.CharacterId);
                    if (player == null)
                    {
                        continue;
                    }
                    application.Remove(player, guild);
                    // TODO: Send mail to player as rejected auto message
                }
            }
            session.Send(GuildPacket.DisbandConfirm());
            session.FieldManager.BroadcastPacket(GuildPacket.UpdateGuildTag(session.Player));
            guild.RemoveMember(session.Player);
            GameServer.GuildManager.RemoveGuild(guild);
            DatabaseManager.Delete(guild);
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
                session.Send(GuildPacket.ErrorNotice((byte) GuildErrorNotice.UnableToSendInvite));
            }

            if (playerInvited.Guild != null)
            {
                session.Send(GuildPacket.ErrorNotice((byte) GuildErrorNotice.CharacterIsAlreadyInAGuild));
                return;
            }

            if (guild.Members.Count >= guild.Capacity)
            {
                //TODO Plug in 'full guild' error packets
                return;
            }

            session.Send(GuildPacket.InviteConfirm(playerInvited));
            playerInvited.Session.Send(GuildPacket.SendInvite(session.Player, playerInvited, guild));

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

            guild.AddMember(session.Player);
            GuildMember member = guild.Members.FirstOrDefault(x => x.Player == session.Player);
            if (member == null)
            {
                return;
            }

            inviter.Session.Send(GuildPacket.InviteNotification(inviteeName, response));
            session.Send(GuildPacket.InviteResponseConfirm(inviter, session.Player, guild, response));
            session.FieldManager.BroadcastPacket(GuildPacket.UpdateGuildTag2(session.Player, guildName));
            guild.BroadcastPacketGuild(GuildPacket.MemberBroadcastJoinNotice(member, inviterName, true));
            guild.BroadcastPacketGuild(GuildPacket.MemberJoin(session.Player), session);
            session.Send(GuildPacket.UpdateGuild(guild));
        }

        private static void HandleLeave(GameSession session)
        {
            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.Guild.Id);
            if (guild == null)
            {
                return;
            }

            session.Send(GuildPacket.LeaveConfirm());
            session.FieldManager.BroadcastPacket(GuildPacket.UpdateGuildTag(session.Player));
            guild.BroadcastPacketGuild(GuildPacket.MemberLeaveNotice(session.Player));
            guild.RemoveMember(session.Player);
        }

        private static void HandleKick(GameSession session, PacketReader packet)
        {
            string target = packet.ReadUnicodeString();

            Player targetPlayer = GameServer.Storage.GetPlayerByName(target);
            if (targetPlayer == null)
            {
                return;
            }

            Guild guild = GameServer.GuildManager.GetGuildByLeader(session.Player);
            if (guild == null)
            {
                return;
            }

            if (targetPlayer.CharacterId == guild.Leader.CharacterId)
            {
                //TODO: Error packets
                return;
            }

            GuildMember selfPlayer = guild.Members.FirstOrDefault(x => x.Player == session.Player);
            if (selfPlayer == null)
            {
                return;
            }

            if (!((GuildRights) guild.Ranks[selfPlayer.Rank].Rights).HasFlag(GuildRights.CanInvite))
            {
                return;
            }

            session.Send(GuildPacket.KickConfirm(targetPlayer));
            if (targetPlayer.Session != null)
            {
                targetPlayer.Session.Send(GuildPacket.KickNotification(session.Player));
                targetPlayer.Session.FieldManager.BroadcastPacket(GuildPacket.UpdateGuildTag(targetPlayer));
            }
            guild.RemoveMember(targetPlayer);
            guild.BroadcastPacketGuild(GuildPacket.KickMember(targetPlayer, session.Player));
        }

        private static void HandleRankChange(GameSession session, PacketReader packet)
        {
            string memberName = packet.ReadUnicodeString();
            byte rank = packet.ReadByte();

            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.Guild.Id);
            if (guild == null || session.Player != guild.Leader)
            {
                return;
            }

            GuildMember member = guild.Members.First(x => x.Player.Name == memberName);
            if (member == null || member.Rank == rank)
            {
                return;
            }

            member.Rank = rank;
            session.Send(GuildPacket.RankChangeConfirm(memberName, rank));
            guild.BroadcastPacketGuild(GuildPacket.RankChangeNotice(session.Player.Name, memberName, rank));
        }

        private static void HandlePlayerMessage(GameSession session, PacketReader packet)
        {
            string message = packet.ReadUnicodeString();

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

            member.Motto = message;
            guild.BroadcastPacketGuild(GuildPacket.UpdatePlayerMessage(session.Player, message));
        }

        private static void HandleCheckIn(GameSession session)
        {
            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.Guild.Id);
            if (guild == null)
            {
                return;
            }
            GuildMember member = guild.Members.First(x => x.Player == session.Player);

            // Check if attendance timestamp is today
            DateTimeOffset date = DateTimeOffset.FromUnixTimeSeconds(member.AttendanceTimestamp);
            if (date == DateTime.Today)
            {
                return;
            }

            int contributionAmount = GuildContributionMetadataStorage.GetContributionAmount("attend");
            GuildPropertyMetadata property = GuildPropertyMetadataStorage.GetMetadata(guild.Exp);

            member.AddContribution(contributionAmount);
            member.AttendanceTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount;
            session.Send(GuildPacket.CheckInBegin());
            Item guildCoins = new Item(30000861)
            {
                Amount = property.AttendGuildCoin
            };

            InventoryController.Add(session, guildCoins, true);
            guild.AddExp(session, property.AttendExp);
            guild.ModifyFunds(session, property, property.AttendFunds);
            guild.BroadcastPacketGuild(GuildPacket.UpdatePlayerContribution(member, contributionAmount));
            session.Send(GuildPacket.FinishCheckIn(member));
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
            if (guild == null || guild.Leader.CharacterId != oldLeader.CharacterId)
            {
                return;
            }
            GuildMember newLeaderMember = guild.Members.FirstOrDefault(x => x.Player.CharacterId == newLeader.CharacterId);
            GuildMember oldLeaderMember = guild.Members.FirstOrDefault(x => x.Player.CharacterId == oldLeader.CharacterId);
            newLeaderMember.Rank = 0;
            oldLeaderMember.Rank = 1;
            guild.Leader = newLeader;

            session.Send(GuildPacket.TransferLeaderConfirm(newLeader));
            guild.BroadcastPacketGuild(GuildPacket.AssignNewLeader(newLeader, oldLeader));
            guild.AssignNewLeader(oldLeader, newLeader);
        }

        private static void HandleGuildNotice(GameSession session, PacketReader packet)
        {
            packet.ReadByte();
            string notice = packet.ReadUnicodeString();

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

            if (!((GuildRights) guild.Ranks[member.Rank].Rights).HasFlag(GuildRights.CanGuildNotice))
            {
                return;
            }

            session.Send(GuildPacket.GuildNoticeConfirm(notice));
            guild.BroadcastPacketGuild(GuildPacket.GuildNoticeChange(session.Player, notice));
        }

        private static void HandleUpdateRank(GameSession session, PacketReader packet)
        {
            byte rankIndex = packet.ReadByte();
            byte rankIndex2 = packet.ReadByte(); // repeat
            string rankName = packet.ReadUnicodeString();
            int rights = packet.ReadInt();

            Guild guild = GameServer.GuildManager.GetGuildByLeader(session.Player);
            if (guild == null || guild.Leader != session.Player)
            {
                return;
            }

            guild.Ranks[rankIndex].Name = rankName;
            guild.Ranks[rankIndex].Rights = rights;
            session.Send(GuildPacket.UpdateRankConfirm(guild, rankIndex));
            guild.BroadcastPacketGuild(GuildPacket.UpdateRankNotice(guild, rankIndex));
        }

        private static void HandleListGuild(GameSession session, PacketReader packet)
        {
            bool toggle = packet.ReadBool();

            Guild guild = GameServer.GuildManager.GetGuildByLeader(session.Player);
            if (guild == null)
            {
                return;
            }

            guild.Searchable = toggle;
            session.Send(GuildPacket.ListGuildConfirm(toggle));
            session.Send(GuildPacket.ListGuildUpdate(session.Player, toggle));
        }

        private static void HandleSubmitApplication(GameSession session, PacketReader packet)
        {
            long guildId = packet.ReadLong();

            if (session.Player.GuildApplications.Count >= 10)
            {
                return;
            }

            Guild guild = GameServer.GuildManager.GetGuildById(guildId);
            if (guild == null)
            {
                return;
            }

            GuildApplication application = new GuildApplication(session.Player.CharacterId, guild.Id);
            application.Add(session.Player, guild);

            session.Send(GuildPacket.SubmitApplication(application, guild.Name));
            foreach (GuildMember member in guild.Members)
            {
                if (((GuildRights) guild.Ranks[member.Rank].Rights).HasFlag(GuildRights.CanInvite))
                {
                    member.Player.Session.Send(GuildPacket.SendApplication(application, session.Player));
                }
            }
        }

        private static void HandleWithdrawApplication(GameSession session, PacketReader packet)
        {
            long guildApplicationId = packet.ReadLong();

            GuildApplication application = session.Player.GuildApplications.FirstOrDefault(x => x.Id == guildApplicationId);
            if (application == null)
            {
                return;
            }

            Guild guild = GameServer.GuildManager.GetGuildById(application.GuildId);
            if (guild == null)
            {
                return;
            }

            application.Remove(session.Player, guild);

            session.Send(GuildPacket.WithdrawApplicationPlayerUpdate(application, guild.Name));
            foreach (GuildMember member in guild.Members)
            {
                if (((GuildRights) guild.Ranks[member.Rank].Rights).HasFlag(GuildRights.CanInvite))
                {
                    member.Player.Session.Send(GuildPacket.WithdrawApplicationGuildUpdate(application.Id));
                }
            }
        }

        private static void HandleApplicationResponse(GameSession session, PacketReader packet)
        {
            long guildApplicationId = packet.ReadLong();
            byte response = packet.ReadByte();

            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.Guild.Id);
            if (guild == null)
            {
                return;
            }

            GuildApplication application = guild.Applications.FirstOrDefault(x => x.Id == guildApplicationId);
            if (application == null)
            {
                return;
            }

            Player applier = GameServer.Storage.GetPlayerById(application.CharacterId);

            session.Send(GuildPacket.ApplicationResponse(guildApplicationId, applier.Name, response));
            if (response == 1)
            {
                session.Send(GuildPacket.InviteNotification(applier.Name, response));
            }
            guild.BroadcastPacketGuild(GuildPacket.ApplicationResponseBroadcastNotice(session.Player.Name, applier.Name, response, guildApplicationId));
            application.Remove(applier, guild);

            if (applier.Session != null)
            {
                applier.Session.Send(GuildPacket.ApplicationResponseToApplier(guild.Name, guildApplicationId, response));
            }

            if (response == 0)
            {
                if (applier.Session != null)
                {
                    // TODO: Send System mail for rejection
                }
                return;
            }

            guild.AddMember(applier);
            if (applier.Session != null)
            {
                applier.Session.Send(GuildPacket.InviteResponseConfirm(session.Player, applier, guild, response));
                applier.Session.FieldManager.BroadcastPacket(GuildPacket.UpdateGuildTag2(applier, guild.Name));
            }

            GuildMember member = guild.Members.FirstOrDefault(x => x.Player == applier);
            guild.BroadcastPacketGuild(GuildPacket.MemberBroadcastJoinNotice(member, session.Player.Name, false));
            guild.BroadcastPacketGuild(GuildPacket.MemberJoin(applier));
            guild.BroadcastPacketGuild(GuildPacket.UpdateGuild(guild));
        }

        private static void HandleLoadApplications(GameSession session)
        {
            session.Send(GuildPacket.LoadApplications(session.Player));
        }

        private static void HandleLoadGuildList(GameSession session, PacketReader packet)
        {
            int focusAttributes = packet.ReadInt();

            List<Guild> guildList = GameServer.GuildManager.GetGuildList();

            if (guildList.Count == 0)
            {
                return;
            }

            if (focusAttributes == -1)
            {
                session.Send(GuildPacket.DisplayGuildList(guildList));
                return;
            }

            // TODO: Filter guilds with focusAttributes
            session.Send(GuildPacket.DisplayGuildList(guildList));
        }

        private static void HandleSearchGuildByName(GameSession session, PacketReader packet)
        {
            string name = packet.ReadUnicodeString();

            List<Guild> guildList = GameServer.GuildManager.GetGuildListByName(name);
            session.Send(GuildPacket.DisplayGuildList(guildList));
        }

        private static void HandleUseBuff(GameSession session, PacketReader packet)
        {
            int buffId = packet.ReadInt();

            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.Guild.Id);
            if (guild == null)
            {
                return;
            }

            int buffLevel = guild.Buffs.FirstOrDefault(x => x.Id == buffId).Level;

            GuildBuffLevel buff = GuildBuffMetadataStorage.GetGuildBuffLevelData(buffId, buffLevel);
            if (buff == null)
            {
                return;
            }

            if (buffId > 1000)
            {
                if (!session.Player.Wallet.Meso.Modify(session, -buff.Cost))
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
            session.Send(GuildPacket.UseBuffNotice(buffId));
        }

        private static void HandleUpgradeBuff(GameSession session, PacketReader packet)
        {
            int buffId = packet.ReadInt();

            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.Guild.Id);
            if (guild == null)
            {
                return;
            }

            GuildBuff buff = guild.Buffs.First(x => x.Id == buffId);

            // get next level's data
            GuildBuffLevel metadata = GuildBuffMetadataStorage.GetGuildBuffLevelData(buffId, buff.Level + 1);

            GuildPropertyMetadata guildProperty = GuildPropertyMetadataStorage.GetMetadata(guild.Exp);

            if (guildProperty.Level < metadata.LevelRequirement)
            {
                return;
            }

            if (guild.Funds < metadata.UpgradeCost)
            {
                return;
            }

            guild.ModifyFunds(session, guildProperty, -metadata.UpgradeCost);
            buff.Level++;
            guild.BroadcastPacketGuild(GuildPacket.UpgradeBuff(buffId, buff.Level, session.Player.Name));
        }

        private static void HandleUpgradeHome(GameSession session, PacketReader packet)
        {
            int themeId = packet.ReadInt();

            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.Guild.Id);
            if (guild == null || guild.Leader != session.Player)
            {
                return;
            }

            GuildHouseMetadata houseMetadata = GuildHouseMetadataStorage.GetMetadataByThemeId(guild.HouseRank + 1, themeId);
            if (houseMetadata == null)
            {
                return;
            }

            GuildPropertyMetadata guildProperty = GuildPropertyMetadataStorage.GetMetadata(guild.Exp);

            if (guildProperty.Level < houseMetadata.RequiredLevel ||
                guild.Funds < houseMetadata.UpgradeCost)
            {
                return;
            }

            guild.ModifyFunds(session, guildProperty, -houseMetadata.UpgradeCost);
            guild.HouseRank = houseMetadata.Level;
            guild.HouseTheme = houseMetadata.Theme;
            guild.BroadcastPacketGuild(GuildPacket.ChangeHouse(session.Player.Name, guild.HouseRank, guild.HouseTheme)); // need to confirm if this is the packet used when upgrading
        }

        private static void HandleChangeHomeTheme(GameSession session, PacketReader packet)
        {
            int themeId = packet.ReadInt();

            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.Guild.Id);
            if (guild == null || guild.Leader != session.Player)
            {
                return;
            }

            GuildHouseMetadata houseMetadata = GuildHouseMetadataStorage.GetMetadataByThemeId(guild.HouseRank, themeId);
            if (houseMetadata == null)
            {
                return;
            }

            GuildPropertyMetadata guildProperty = GuildPropertyMetadataStorage.GetMetadata(guild.Exp);

            if (guild.Funds < houseMetadata.UpgradeCost)
            {
                return;
            }

            guild.ModifyFunds(session, guildProperty, -houseMetadata.RethemeCost);
            guild.HouseTheme = houseMetadata.Theme;
            guild.BroadcastPacketGuild(GuildPacket.ChangeHouse(session.Player.Name, guild.HouseRank, guild.HouseTheme));
        }

        private static void HandleEnterHouse(GameSession session)
        {
            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.Guild.Id);
            if (guild == null)
            {
                return;
            }

            int mapid = GuildHouseMetadataStorage.GetFieldId(guild.HouseRank, guild.HouseTheme);
            if (mapid == 0)
            {
                return;
            }

            session.Player.ReturnCoord = session.FieldPlayer.Coord;
            session.Player.ReturnMapId = session.Player.MapId;
            session.Player.Warp(mapId: mapid, instanceId: guild.Id);
        }

        private static void HandleGuildDonate(GameSession session, PacketReader packet)
        {
            int donateQuantity = packet.ReadInt();
            int donationAmount = donateQuantity * 10000;

            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.Guild.Id);
            if (guild == null)
            {
                return;
            }

            GuildPropertyMetadata guildProperty = GuildPropertyMetadataStorage.GetMetadata(guild.Exp);

            GuildMember member = guild.Members.First(x => x.Player == session.Player);
            if (member.DailyDonationCount >= guildProperty.DonationMax)
            {
                return;
            }

            if (!session.Player.Wallet.Meso.Modify(session, -donationAmount))
            {
                session.Send(GuildPacket.ErrorNotice((byte) GuildErrorNotice.NotEnoughMesos));
                return;
            }

            Item coins = new Item(30000861)
            {
                Amount = guildProperty.DonateGuildCoin * donateQuantity
            };

            InventoryController.Add(session, coins, true);

            int contribution = GuildContributionMetadataStorage.GetContributionAmount("donation");

            member.DailyDonationCount += (byte) donateQuantity;
            member.AddContribution(contribution * donateQuantity);
            guild.ModifyFunds(session, guildProperty, donationAmount);
            session.Send(GuildPacket.UpdatePlayerDonation());
            guild.BroadcastPacketGuild(GuildPacket.UpdatePlayerContribution(member, donateQuantity));
        }

        private static void HandleServices(GameSession session, PacketReader packet)
        {
            int serviceId = packet.ReadInt();

            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.Guild.Id);
            if (guild == null)
            {
                return;
            }

            int currentLevel = 0;
            GuildService service = guild.Services.FirstOrDefault(x => x.Id == serviceId);
            if (service != null)
            {
                service.Level = currentLevel;
            }

            GuildServiceMetadata serviceMetadata = GuildServiceMetadataStorage.GetMetadata(serviceId, currentLevel);
            if (serviceMetadata == null)
            {
                return;
            }

            GuildPropertyMetadata propertyMetadata = GuildPropertyMetadataStorage.GetMetadata(guild.Exp);

            if (guild.HouseRank < serviceMetadata.HouseLevelRequirement ||
                propertyMetadata.Level < serviceMetadata.LevelRequirement ||
                guild.Funds < serviceMetadata.UpgradeCost)
            {
                return;
            }

            guild.ModifyFunds(session, propertyMetadata, -serviceMetadata.UpgradeCost);
            guild.BroadcastPacketGuild(GuildPacket.UpgradeService(session.Player, serviceId, serviceMetadata.Level));
        }
    }
}
