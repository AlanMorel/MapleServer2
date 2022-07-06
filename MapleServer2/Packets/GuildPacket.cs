using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class GuildPacket
{
    private enum Mode : byte
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
        UpdateMemberName = 0xC,
        CheckInBegin = 0xF,
        MemberBroadcastJoinNotice = 0x12,
        MemberLeaveNotice = 0x13,
        KickMember = 0x14,
        RankChangeNotice = 0x15,
        UpdatePlayerMessage = 0x16,
        MemberLoggedIn = 0x17,
        MemberLoggedOff = 0x18,
        AssignNewLeader = 0x19,
        GuildNoticeChange = 0x1A,
        GuildNoticeEmblemChange = 0x1B,
        UpdateRankNotice = 0x1D,
        ListGuildUpdate = 0x1E,
        UpdateMemberLocation = 0x1F,
        UpdatePlayer = 0x20,
        GuildNameChange = 0x22,
        TrophyNotice = 0x23,
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
        UpdateBannerUrl = 0x38,
        UpgradeService = 0x39,
        RequestMiniGameWar = 0x3B,
        TransferLeaderConfirm = 0x3D,
        GuildNoticeConfirm = 0x3E,
        ChangeEmblemUrl = 0x3F,
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
        UpdatePlayerDonation = 0x6E
    }

    public static PacketWriter UpdateGuild(Guild guild)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.UpdateGuild);
        pWriter.WriteLong(guild.Id);
        pWriter.WriteUnicodeString(guild.Name);
        pWriter.WriteUnicodeString(guild.Emblem);
        pWriter.WriteByte(guild.Capacity);
        pWriter.WriteUnicodeString();
        pWriter.WriteUnicodeString(guild.Notice);
        pWriter.WriteLong(guild.LeaderAccountId);
        pWriter.WriteLong(guild.LeaderCharacterId);
        pWriter.WriteUnicodeString(guild.LeaderName);
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
        pWriter.WriteByte();
        pWriter.WriteInt();
        pWriter.WriteByte((byte) guild.Members.Count);

        foreach (GuildMember member in guild.Members)
        {
            pWriter.WriteByte(0x3);
            pWriter.WriteByte(member.Rank);
            pWriter.WriteLong(member.Player.CharacterId);
            WriteGuildMember(pWriter, member.Player);
            pWriter.WriteUnicodeString(member.Motto);
            pWriter.WriteLong(member.JoinTimestamp);
            pWriter.WriteLong(member.LastLogTimestamp);
            pWriter.WriteLong(member.AttendanceTimestamp);
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt(member.DailyContribution);
            pWriter.WriteInt(member.ContributionTotal);
            pWriter.WriteInt(member.DailyDonationCount);
            pWriter.WriteLong();
            pWriter.WriteInt();

            pWriter.WriteBool(member.Player.Session is null || !member.Player.Session.Connected()); // 00 = online, 01 = offline
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

        byte events = 4;
        pWriter.WriteByte(events);
        for (int i = 0; i < events; i++)
        {
            pWriter.WriteInt(i + 1);
            pWriter.WriteInt();
        }

        pWriter.WriteInt(guild.HouseRank);
        pWriter.WriteInt(guild.HouseTheme);
        pWriter.WriteInt(guild.Banners.Count);
        foreach (UGC ugcBanner in guild.Banners)
        {
            pWriter.WriteInt(ugcBanner.GuildPosterId);
            pWriter.WriteUnicodeString(ugcBanner.Url);
            pWriter.WriteLong(ugcBanner.CharacterId);
            pWriter.WriteUnicodeString(ugcBanner.CharacterName);
        }

        pWriter.WriteByte((byte) guild.Services.Count);
        foreach (GuildService service in guild.Services)
        {
            pWriter.WriteInt(service.Id);
            pWriter.WriteInt(service.Level);
        }

        bool flag = false;
        pWriter.WriteBool(flag); // GuildNpcShopProducts
        if (flag)
        {
            short count = 0;
            pWriter.WriteShort(count);
            for (int i = 0; i < count; i++)
            {
                bool flag2 = false;
                pWriter.WriteBool(flag2);
                if (flag2)
                {
                    pWriter.WriteShort();
                    pWriter.WriteLong();
                    short count2 = 0;
                    pWriter.WriteShort(count2);
                    for (int j = 0; j < count2; j++)
                    {
                        bool flag3 = false;
                        pWriter.WriteBool(flag3);
                        if (flag3)
                        {
                            pWriter.WriteInt();
                            pWriter.WriteByte();
                            pWriter.WriteInt();
                            pWriter.WriteInt();
                        }
                    }
                }
            }
        }

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

        pWriter.WriteInt();
        pWriter.WriteUnicodeString();
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

    public static PacketWriter Create(string guildName)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.Create);
        pWriter.WriteByte();
        pWriter.WriteUnicodeString(guildName);
        return pWriter;
    }

    public static PacketWriter DisbandConfirm()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.DisbandConfirm);
        pWriter.WriteByte();
        return pWriter;
    }

    public static PacketWriter InviteConfirm(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.InviteConfirm);
        pWriter.WriteUnicodeString(player.Name);
        return pWriter;
    }

    public static PacketWriter SendInvite(Player inviter, Player invitee, Guild guild)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.SendInvite);
        pWriter.WriteLong(guild.Id);
        pWriter.WriteUnicodeString(guild.Name);
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteUnicodeString(inviter.Name);
        pWriter.WriteUnicodeString(invitee.Name);
        return pWriter;
    }

    public static PacketWriter InviteResponseConfirm(Player inviter, Player invitee, Guild guild, short response)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.InviteResponseConfirm);
        pWriter.WriteLong(guild.Id);
        pWriter.WriteUnicodeString(guild.Name);
        pWriter.WriteShort();
        pWriter.WriteUnicodeString(inviter.Name);
        pWriter.WriteUnicodeString(invitee.Name);
        pWriter.WriteShort(response);
        return pWriter;
    }

    public static PacketWriter InviteNotification(string inviteeName, short response)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.InviteNotification);
        pWriter.WriteUnicodeString(inviteeName);
        pWriter.WriteShort(response);
        return pWriter;
    }

    public static PacketWriter LeaveConfirm()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.LeaveConfirm);
        return pWriter;
    }

    public static PacketWriter KickConfirm(Player member)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.KickConfirm);
        pWriter.WriteUnicodeString(member.Name);
        return pWriter;
    }

    public static PacketWriter KickNotification(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.KickNotification);
        pWriter.WriteUnicodeString(player.Name);
        return pWriter;
    }

    public static PacketWriter RankChangeConfirm(string memberName, byte rank)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.RankChangeConfirm);
        pWriter.WriteUnicodeString(memberName);
        pWriter.WriteByte(rank);
        return pWriter;
    }

    public static PacketWriter UpdateMemberName(string oldName, string newName)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.UpdateMemberName);
        pWriter.WriteUnicodeString(oldName);
        pWriter.WriteUnicodeString(newName);
        return pWriter;
    }

    public static PacketWriter CheckInBegin()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.CheckInBegin);
        return pWriter;
    }

    public static PacketWriter MemberBroadcastJoinNotice(GuildMember member, string inviterName, bool displayNotice)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.MemberBroadcastJoinNotice);
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

    public static PacketWriter MemberLeaveNotice(Player member)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.MemberLeaveNotice);
        pWriter.WriteUnicodeString(member.Name);
        return pWriter;
    }

    public static PacketWriter KickMember(Player member, Player leader)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.KickMember);
        pWriter.WriteUnicodeString(leader.Name);
        pWriter.WriteUnicodeString(member.Name);
        return pWriter;
    }

    public static PacketWriter RankChangeNotice(string leaderName, string memberName, byte rank)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.RankChangeNotice);
        pWriter.WriteUnicodeString(leaderName);
        pWriter.WriteUnicodeString(memberName);
        pWriter.WriteByte(rank);
        return pWriter;
    }

    public static PacketWriter UpdatePlayerMessage(Player player, string message)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.UpdatePlayerMessage);
        pWriter.WriteUnicodeString(player.Name);
        pWriter.WriteUnicodeString(message);
        return pWriter;
    }

    public static PacketWriter MemberLoggedIn(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.MemberLoggedIn);
        pWriter.WriteUnicodeString(player.Name);
        return pWriter;
    }

    public static PacketWriter MemberLoggedOff(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.MemberLoggedOff);
        pWriter.WriteUnicodeString(player.Name);
        pWriter.WriteLong(TimeInfo.Now());
        return pWriter;
    }

    public static PacketWriter AssignNewLeader(Player newLeader, Player oldLeader)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.AssignNewLeader);
        pWriter.WriteUnicodeString(oldLeader.Name);
        pWriter.WriteUnicodeString(newLeader.Name);
        return pWriter;
    }

    public static PacketWriter GuildNoticeChange(Player player, string notice)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.GuildNoticeChange);
        pWriter.WriteUnicodeString(player.Name);
        pWriter.WriteByte(0x1);
        pWriter.WriteUnicodeString(notice);
        return pWriter;
    }

    public static PacketWriter GuildNoticeEmblemChange(string playerName, string emblemUrl)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.GuildNoticeEmblemChange);
        pWriter.WriteByte();
        pWriter.WriteInt();
        pWriter.WriteUnicodeString(playerName);
        pWriter.WriteUnicodeString(emblemUrl);
        return pWriter;
    }

    public static PacketWriter UpdateRankNotice(Guild guild, byte rankIndex)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.UpdateRankNotice);
        pWriter.WriteByte();
        pWriter.WriteInt();
        pWriter.WriteUnicodeString(guild.LeaderName);
        pWriter.WriteByte(rankIndex);
        pWriter.WriteByte(rankIndex);
        pWriter.WriteUnicodeString(guild.Ranks[rankIndex].Name);
        pWriter.WriteInt(guild.Ranks[rankIndex].Rights);
        return pWriter;
    }

    public static PacketWriter ListGuildUpdate(Player player, bool toggle)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.ListGuildUpdate);
        pWriter.WriteUnicodeString(player.Name);
        pWriter.WriteBool(toggle);
        pWriter.WriteInt();
        return pWriter;
    }

    public static PacketWriter UpdateMemberLocation(string playerName, int mapId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.UpdateMemberLocation);
        pWriter.WriteUnicodeString(playerName);
        pWriter.WriteInt(mapId);
        return pWriter;
    }

    public static PacketWriter UpdatePlayer(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.UpdatePlayer);
        pWriter.WriteUnicodeString(player.Name);
        WriteGuildMember(pWriter, player);
        return pWriter;
    }

    public static PacketWriter GuildNameChange(string newName)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.GuildNameChange);
        pWriter.WriteUnicodeString(newName);
        return pWriter;
    }

    public static PacketWriter TrophyNotice()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.TrophyNotice);
        pWriter.WriteUnicodeString(); // character name
        pWriter.WriteInt(); // trophy id
        pWriter.WriteInt(); // trophy value
        pWriter.WriteShort(); // mode: 0 = completed the final stage, 1 = gained a trophy, there is more modes but they make the same message?

        return pWriter;
    }

    public static PacketWriter FinishCheckIn(GuildMember member)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.FinishCheckIn);
        pWriter.WriteUnicodeString(member.Player.Name);
        pWriter.WriteLong(member.AttendanceTimestamp);
        return pWriter;
    }

    public static PacketWriter BattleMatchmaking(bool success)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.BattleMatchmaking);
        pWriter.WriteBool(success);
        return pWriter;
    }

    public static PacketWriter BattleApplyNotice(string playerName)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.BattleApplyNotice);
        pWriter.WriteUnicodeString(playerName);
        return pWriter;
    }

    public static PacketWriter BattleCancelApplyNotice(string playerName)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.BattleCancelApplyNotice);
        pWriter.WriteUnicodeString(playerName);
        return pWriter;
    }

    public static PacketWriter SendApplication(GuildApplication application, Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.SendApplication);
        pWriter.WriteLong(application.Id);
        pWriter.WriteLong(application.GuildId);
        pWriter.WriteLong(player.AccountId);
        pWriter.WriteLong(player.CharacterId);
        pWriter.WriteUnicodeString(player.Name);
        pWriter.WriteUnicodeString(player.ProfileUrl);
        pWriter.Write(player.Job);
        pWriter.Write(player.JobCode);
        pWriter.WriteInt(player.Levels.Level);
        foreach (int trophyCategory in player.TrophyCount)
        {
            pWriter.WriteInt(trophyCategory);
        }

        pWriter.WriteLong(TimeInfo.Now() + Environment.TickCount);
        return pWriter;
    }

    public static PacketWriter WithdrawApplicationGuildUpdate(long applicationId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.WithdrawApplicationGuildUpdate);
        pWriter.WriteLong(applicationId);
        return pWriter;
    }

    public static PacketWriter ApplicationResponseBroadcastNotice(string reviewerName, string applierName, byte response, long applicationId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.ApplicationResponseBroadcastNotice);
        pWriter.WriteUnicodeString(reviewerName);
        pWriter.WriteUnicodeString(applierName);
        pWriter.WriteByte(response);
        pWriter.WriteLong(applicationId);
        return pWriter;
    }

    public static PacketWriter ApplicationResponseToApplier(string guildName, long applicationId, byte response)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.ApplicationResponseToApplier);
        pWriter.WriteUnicodeString(guildName);
        pWriter.WriteLong(applicationId);
        pWriter.WriteByte(response);
        return pWriter;
    }

    public static PacketWriter UpdateGuildExp(int guildExp)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.UpdateGuildExp);
        pWriter.WriteInt(guildExp);
        return pWriter;
    }

    public static PacketWriter UpdateGuildFunds(int guildFunds)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.UpdateGuildFunds);
        pWriter.WriteInt(guildFunds);
        return pWriter;
    }

    public static PacketWriter UpdatePlayerContribution(GuildMember member, int contribution)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.UpdatePlayerContribution);
        pWriter.WriteUnicodeString(member.Player.Name);
        pWriter.WriteInt(contribution);
        pWriter.WriteInt(member.DailyContribution);
        pWriter.WriteInt(member.ContributionTotal);
        return pWriter;
    }

    public static PacketWriter UpgradeBuff(int buffId, int buffLevel, string playerName)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.UpgradeBuff);
        pWriter.WriteUnicodeString(playerName);
        pWriter.WriteInt(buffId);
        pWriter.WriteInt(buffLevel);
        pWriter.WriteLong();
        return pWriter;
    }

    public static PacketWriter StartMinigame(string playerName, int minigameId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.StartMiniGame);
        pWriter.WriteUnicodeString(playerName);
        pWriter.WriteInt(minigameId);
        pWriter.WriteInt();
        return pWriter;
    }

    public static PacketWriter ChangeHouse(string playerName, int houseRank, int houseTheme)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.ChangeHouse);
        pWriter.WriteUnicodeString(playerName);
        pWriter.WriteInt(houseRank);
        pWriter.WriteInt(houseTheme);
        return pWriter;
    }

    public static PacketWriter UpgradeService(Player player, int serviceId, int serviceLevel)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.UpgradeService);
        pWriter.WriteUnicodeString(player.Name);
        pWriter.WriteInt(serviceId);
        pWriter.WriteInt(serviceLevel);
        return pWriter;
    }

    public static PacketWriter UpdateBannerUrl(Player player, UGC ugc)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.UpdateBannerUrl);
        pWriter.WriteLong(player.CharacterId);
        pWriter.WriteUnicodeString(player.Name);
        pWriter.WriteInt(ugc.GuildPosterId);
        pWriter.WriteUnicodeString(ugc.Url);
        return pWriter;
    }

    public static PacketWriter RequestMiniGameWar(bool request)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.RequestMiniGameWar);
        pWriter.WriteBool(request);
        pWriter.WriteByte(0x1);
        return pWriter;
    }

    public static PacketWriter TransferLeaderConfirm(Player newLeader)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.TransferLeaderConfirm);
        pWriter.WriteUnicodeString(newLeader.Name);
        return pWriter;
    }

    public static PacketWriter SubmitApplication(GuildApplication application, string guildName)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.SubmitApplication);
        pWriter.WriteLong(application.Id);
        pWriter.WriteUnicodeString(guildName);
        return pWriter;
    }

    public static PacketWriter WithdrawApplicationPlayerUpdate(GuildApplication application, string guildName)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.WithdrawApplicationPlayerUpdate);
        pWriter.WriteLong(application.Id);
        pWriter.WriteUnicodeString(guildName);
        return pWriter;
    }

    public static PacketWriter ApplicationResponse(long guildApplicationId, string applierName, byte response)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.ApplicationResponse);
        pWriter.WriteLong(guildApplicationId);
        pWriter.WriteUnicodeString(applierName);
        pWriter.WriteByte(response);
        return pWriter;
    }

    public static PacketWriter GuildNoticeConfirm(string notice)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.GuildNoticeConfirm);
        pWriter.WriteByte(0x1);
        pWriter.WriteUnicodeString(notice);
        return pWriter;
    }

    public static PacketWriter ChangeEmblemUrl(string emblemUrl)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.ChangeEmblemUrl);
        pWriter.WriteUnicodeString(emblemUrl);
        return pWriter;
    }

    public static PacketWriter UpdateRankConfirm(Guild guild, byte rankIndex)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.UpdateRankConfirm);
        pWriter.WriteByte(rankIndex);
        pWriter.WriteByte(rankIndex);
        pWriter.WriteUnicodeString(guild.Ranks[rankIndex].Name);
        pWriter.WriteInt(guild.Ranks[rankIndex].Rights);
        return pWriter;
    }

    public static PacketWriter ListGuildConfirm(bool toggle)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.ListGuildConfirm);
        pWriter.WriteBool(toggle);
        pWriter.WriteInt();
        return pWriter;
    }

    public static PacketWriter SendMail()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.SendMail);
        return pWriter;
    }

    public static PacketWriter UpdateGuildTag2(Player player, string guildName)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.UpdateGuildTag2);
        pWriter.WriteUnicodeString(player.Name);
        pWriter.WriteUnicodeString(guildName);
        return pWriter;
    }

    public static PacketWriter UpdateGuildTag(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.UpdateGuildTag);
        pWriter.WriteUnicodeString(player.Name);
        return pWriter;
    }

    public static PacketWriter ErrorNotice(byte errorNotice, int param = 0)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.ErrorNotice);
        pWriter.WriteByte(1);
        pWriter.WriteByte(errorNotice);
        pWriter.WriteInt(param);
        return pWriter;
    }

    public static PacketWriter LoadApplications(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.LoadApplications);
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
            pWriter.WriteUnicodeString(guild.LeaderName);
            pWriter.WriteLong(player.AccountId);
            pWriter.WriteLong(player.CharacterId);
            pWriter.WriteLong(application.CreationTimestamp);
        }

        return pWriter;
    }

    public static PacketWriter DisplayGuildList(List<Guild> guilds)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.DisplayGuildList);
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
            pWriter.WriteLong(guild.LeaderAccountId);
            pWriter.WriteLong(guild.LeaderCharacterId);
            pWriter.WriteUnicodeString(guild.LeaderName);
        }

        return pWriter;
    }

    public static PacketWriter UseBuffNotice(int buffID)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.UseBuffNotice);
        pWriter.WriteInt(buffID);
        return pWriter;
    }

    public static PacketWriter ActivateBuff(int buffID)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.ActivateBuff);
        pWriter.WriteInt(buffID);
        return pWriter;
    }

    public static PacketWriter List()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.List);
        pWriter.WriteByte(0x1);
        pWriter.WriteLong();
        pWriter.WriteLong();
        pWriter.WriteLong();
        pWriter.WriteShort();
        pWriter.WriteUnicodeString(); //GuildMark url
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteInt(); //guild Member Total
        pWriter.WriteInt(); //guild Capacity
        return pWriter;
    }

    public static PacketWriter UpdateGuildStatsNotice(int exp, int funds)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.UpdateGuildStatsNotice);
        pWriter.WriteInt(exp);
        pWriter.WriteInt(funds);
        return pWriter;
    }

    public static PacketWriter StartMiniGame(int minigameId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.StartMiniGame);
        pWriter.WriteInt(minigameId);
        return pWriter;
    }

    public static PacketWriter UpdatePlayerDonation( /*int totalDonationAmount*/)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Guild);
        pWriter.Write(Mode.UpdatePlayerDonation);
        pWriter.WriteInt(0x3); // total amount of donations today
        pWriter.WriteLong(TimeInfo.Now() + Environment.TickCount);
        return pWriter;
    }

    public static void WriteGuildMember(PacketWriter pWriter, Player member)
    {
        pWriter.WriteLong(member.AccountId);
        pWriter.WriteLong(member.CharacterId);
        pWriter.WriteUnicodeString(member.Name);
        pWriter.Write(member.Gender);
        pWriter.Write(member.Job);
        pWriter.Write(member.JobCode);
        pWriter.WriteShort(member.Levels.Level);
        pWriter.WriteInt(); // player gearscore
        pWriter.WriteInt(member.MapId);
        pWriter.WriteShort(member.ChannelId);
        pWriter.WriteUnicodeString(member.ProfileUrl);
        pWriter.WriteInt(member.Account.Home?.PlotMapId ?? 0);
        pWriter.WriteInt(member.Account.Home?.PlotNumber ?? 0);
        pWriter.WriteInt(member.Account.Home?.ApartmentNumber ?? 0);
        pWriter.WriteLong(member.Account.Home?.Expiration ?? 0);
        foreach (int trophyCategory in member.TrophyCount)
        {
            pWriter.WriteInt(trophyCategory);
        }
    }
}
