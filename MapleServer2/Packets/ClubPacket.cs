using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class ClubPacket
{
    private enum ClubPacketMode : byte
    {
        UpdateClub = 0x0,
        Establish = 0x1,
        Create = 0x2,
        DeleteUnestablishedClub = 0x5, // maybe only for establishing?
        InviteSentReceipt = 0x6,
        Invite = 0x7,
        InviteResponse = 0x8,
        LeaderInviteResponse = 0x9,
        LeaveClub = 0xA,
        ChangeBuffReceipt = 0xD,
        ClubProposalInviteResponse = 0xF,
        Disband = 0x10,
        ConfirmInvite = 0x11,
        LeaveNotice = 0x12,
        LoginNotice = 0x13,
        LogoutNotice = 0x14,
        AssignNewLeader = 0x15,
        ChangeBuff = 0x16,
        UpdateMemberLocation = 0x17,
        UpdatePlayer = 0x18,
        Rename = 0x1A,
        UpdateMemberName = 0x1B,
        ErrorNotice = 0x1D,
        Join = 0x1E
    }

    public static PacketWriter UpdateClub(Club club)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.UpdateClub);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(club.Name);
        pWriter.WriteLong(club.LeaderAccountId);
        pWriter.WriteLong(club.LeaderCharacterId);
        pWriter.WriteUnicodeString(club.LeaderName);
        pWriter.WriteLong(club.CreationTimestamp);
        pWriter.WriteByte(0x2); // 0x1 create, 0x2 update?
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteLong(club.LastNameChangeTimestamp);
        pWriter.WriteByte((byte) club.Members.Count);

        foreach (ClubMember member in club.Members)
        {
            pWriter.WriteByte(0x2);
            pWriter.WriteLong(club.Id);
            WriteClubMember(pWriter, member);
        }

        pWriter.WriteByte(0x1);
        return pWriter;
    }

    public static PacketWriter Establish(Club club)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.Establish);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(club.Name);
        return pWriter;
    }

    public static PacketWriter Create(Party party, Club club)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.Create);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(club.Name);
        pWriter.WriteLong(party.Leader.AccountId);
        pWriter.WriteLong(party.Leader.CharacterId);
        pWriter.WriteUnicodeString(party.Leader.Name);
        pWriter.WriteLong(club.CreationTimestamp);
        pWriter.WriteByte(0x1); // 0x1 create, 0x2 update?
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteLong(club.LastNameChangeTimestamp);
        pWriter.WriteByte((byte) club.Members.Count);

        foreach (ClubMember member in club.Members) // originally was party members?
        {
            pWriter.WriteByte(0x2);
            pWriter.WriteLong(club.Id);
            WriteClubMember(pWriter, member);
        }

        return pWriter;
    }

    public static PacketWriter DeleteUnestablishedClub(long clubId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.DeleteUnestablishedClub);
        pWriter.WriteLong(clubId);
        pWriter.WriteInt(0x4D); // unk
        return pWriter;
    }

    public static PacketWriter InviteSentReceipt(long clubId, Player other)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.InviteSentReceipt);
        pWriter.WriteLong(clubId);
        pWriter.WriteUnicodeString(other.Name);
        return pWriter;
    }

    public static PacketWriter Invite(Club club, Player other)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.Invite);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(club.Name);
        pWriter.WriteUnicodeString(club.LeaderName);
        pWriter.WriteUnicodeString(other.Name);
        return pWriter;
    }

    public static PacketWriter InviteResponse(Club club, Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.InviteResponse);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(club.Name);
        pWriter.WriteUnicodeString(club.LeaderName);
        pWriter.WriteUnicodeString(player.Name);
        pWriter.WriteInt(); //00 = accept
        return pWriter;
    }

    public static PacketWriter LeaderInviteResponse(Club club, string invitee, bool response)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.LeaderInviteResponse);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(invitee);
        pWriter.WriteBool(response);
        pWriter.WriteByte();
        return pWriter;
    }

    public static PacketWriter LeaveClub(Club club)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.LeaveClub);
        pWriter.WriteLong(club.Id);
        return pWriter;
    }

    public static PacketWriter ChangeBuffReceipt(Club club, int buffId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.ChangeBuffReceipt);
        pWriter.WriteLong(club.Id);
        pWriter.WriteInt(buffId);
        pWriter.WriteInt(0x1);
        return pWriter;
    }

    public static PacketWriter ClubProposalInviteResponse(long clubId, ClubInviteResponse response, string characterName)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.ClubProposalInviteResponse);
        pWriter.WriteLong(clubId);
        pWriter.Write(response);
        pWriter.WriteUnicodeString(characterName);
        return pWriter;
    }

    public static PacketWriter Disband(Club club)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.Disband);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(club.LeaderName);
        pWriter.WriteInt(0xCF); //unk
        return pWriter;
    }

    public static PacketWriter ConfirmInvite(Club club, ClubMember member /*, byte response*/)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.ConfirmInvite);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(club.LeaderName);
        pWriter.WriteByte(0x2); //unk
        pWriter.WriteLong(club.Id);
        WriteClubMember(pWriter, member);

        return pWriter;
    }

    public static PacketWriter LeaveNotice(Club club, Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.LeaveNotice);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(player.Name);
        return pWriter;
    }

    public static PacketWriter LogoutNotice(Player player, Club club)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.LogoutNotice);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(player.Name);
        pWriter.WriteLong(player.LastLogTime);
        return pWriter;
    }

    public static PacketWriter AssignNewLeader(Player oldLeader, Club club)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.AssignNewLeader);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(oldLeader.Name);
        pWriter.WriteUnicodeString(club.LeaderName);
        pWriter.WriteByte(0x1);
        return pWriter;
    }

    public static PacketWriter ChangeBuff(Club club, int buffId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.ChangeBuff);
        pWriter.WriteLong(club.Id);
        pWriter.WriteInt(buffId);
        pWriter.WriteInt(0x1); // buff level
        return pWriter;
    }

    public static PacketWriter UpdateMemberLocation(long clubId, string memberName, int mapId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.UpdateMemberLocation);
        pWriter.WriteLong(clubId);
        pWriter.WriteUnicodeString(memberName);
        pWriter.WriteInt(mapId);
        return pWriter;
    }

    public static PacketWriter UpdatePlayer(ClubMember member, Club club)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.UpdatePlayer);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(member.Player.Name);
        WriteClubMember(pWriter, member);
        return pWriter;
    }

    public static PacketWriter LoginNotice(Player player, Club club)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.LoginNotice);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(player.Name);
        return pWriter;
    }

    public static PacketWriter Rename(Club club)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.Rename);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(club.Name);
        pWriter.WriteLong(); //unk
        return pWriter;
    }

    public static PacketWriter UpdateMemberName(string oldName, string newName, long characterId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.UpdateMemberName);
        pWriter.WriteLong(characterId);
        pWriter.WriteUnicodeString(oldName);
        pWriter.WriteUnicodeString(newName);
        return pWriter;
    }

    public static PacketWriter ErrorNotice(int errorId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.ErrorNotice);
        pWriter.WriteByte(1);
        pWriter.WriteInt(errorId);
        return pWriter;
    }

    public static PacketWriter Join(string memberName, Club club)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Club);
        pWriter.Write(ClubPacketMode.Join);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(memberName);
        pWriter.WriteUnicodeString(club.Name);
        return pWriter;
    }

    public static void WriteClubMember(PacketWriter pWriter, ClubMember member)
    {
        pWriter.WriteLong(member.Player.AccountId);
        pWriter.WriteLong(member.Player.CharacterId);
        pWriter.WriteUnicodeString(member.Player.Name);
        pWriter.WriteByte();
        pWriter.Write(member.Player.Job);
        pWriter.Write(member.Player.JobCode);
        pWriter.WriteShort(member.Player.Levels.Level);
        pWriter.WriteInt(member.Player.MapId);
        pWriter.WriteShort(member.Player.ChannelId);
        pWriter.WriteUnicodeString(member.Player.ProfileUrl);
        pWriter.WriteInt(member.Player.Account.Home?.PlotMapId ?? 0);
        pWriter.WriteInt(member.Player.Account.Home?.PlotNumber ?? 0);
        pWriter.WriteInt(member.Player.Account.Home?.ApartmentNumber ?? 0);
        pWriter.WriteLong(member.Player.Account.Home?.Expiration ?? 0);
        foreach (int trophyCategory in member.Player.TrophyCount)
        {
            pWriter.WriteInt(trophyCategory);
        }

        pWriter.WriteLong(member.JoinTimestamp);
        pWriter.WriteLong(member.Player.LastLogTime);
        pWriter.WriteBool(!member.Player.Session?.Connected() ?? true);
    }
}
