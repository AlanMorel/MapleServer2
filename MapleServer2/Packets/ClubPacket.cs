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
        InviteSentReceipt = 0x6,
        Invite = 0x7,
        InviteResponse = 0x8,
        LeaderInviteResponse = 0x9,
        LeaveClub = 0xA,
        ChangeBuffReceipt = 0xD,
        ConfirmCreate = 0xF,
        Disband = 0x10,
        ConfirmInvite = 0x11,
        LeaveNotice = 0x12,
        LogoutNotice = 0x14,
        AssignNewLeader = 0x15,
        ChangeBuff = 0x16,
        UpdatePlayerClubList = 0x18,
        LoginNotice = 0x19,
        Rename = 0x1A,
        Join = 0x1E
    }

    public static PacketWriter UpdateClub(Club club)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
        pWriter.Write(ClubPacketMode.UpdateClub);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(club.Name);
        pWriter.WriteLong(club.Leader.AccountId);
        pWriter.WriteLong(club.Leader.CharacterId);
        pWriter.WriteUnicodeString(club.Leader.Name);
        pWriter.WriteLong(); // timestamp
        pWriter.WriteByte(0x2); // 0x1 create, 0x2 update?
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteLong();
        pWriter.WriteByte((byte) club.Members.Count);

        foreach (Player member in club.Members)
        {
            // TODO convert this to a method to share with Create
            pWriter.WriteByte(0x2);
            pWriter.WriteLong(club.Id);
            pWriter.WriteLong(member.AccountId);
            pWriter.WriteLong(member.CharacterId);
            pWriter.WriteUnicodeString(member.Name);
            pWriter.WriteByte();
            pWriter.Write(member.Job);
            pWriter.Write(member.JobCode);
            pWriter.WriteShort(member.Levels.Level);
            pWriter.WriteInt(member.MapId);
            pWriter.WriteShort(); // member.Channel
            pWriter.WriteUnicodeString(member.ProfileUrl);
            pWriter.WriteInt(); // member house mapId
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteLong(); // member house plot expiration timestamp
            pWriter.WriteInt(); // combat trophy count
            pWriter.WriteInt(); // adventure trophy count
            pWriter.WriteInt(); // lifestyle trophy count
            pWriter.WriteLong(); // joined timestamp
            pWriter.WriteLong(); // current timestamp
            pWriter.WriteByte();
        }
        pWriter.WriteByte(0x1);
        return pWriter;
    }

    public static PacketWriter Establish(Club club)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
        pWriter.Write(ClubPacketMode.Establish);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(club.Name);
        return pWriter;
    }

    public static PacketWriter Create(Party party, Club club)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
        pWriter.Write(ClubPacketMode.Create);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(club.Name);
        pWriter.WriteLong(party.Leader.AccountId);
        pWriter.WriteLong(party.Leader.CharacterId);
        pWriter.WriteUnicodeString(party.Leader.Name);
        pWriter.WriteLong(); // timestamp
        pWriter.WriteByte(0x1); // 0x1 create, 0x2 update?
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteLong();
        pWriter.WriteByte((byte) party.Members.Count);

        foreach (Player member in party.Members)
        {
            // TODO convert this to a method to share with UpdateClub
            pWriter.WriteByte(0x2);
            pWriter.WriteLong(club.Id);
            pWriter.WriteLong(member.AccountId);
            pWriter.WriteLong(member.CharacterId);
            pWriter.WriteUnicodeString(member.Name);
            pWriter.WriteByte();
            pWriter.Write(member.Job);
            pWriter.Write(member.JobCode);
            pWriter.WriteShort(member.Levels.Level);
            pWriter.WriteInt(member.MapId);
            pWriter.WriteShort(); // member.Channel
            pWriter.WriteUnicodeString(member.ProfileUrl);
            pWriter.WriteInt(); // player house mapId
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteLong(); // player house plot expiration timestamp
            pWriter.WriteInt(); // combat trophy count
            pWriter.WriteInt(); // adventure trophy count
            pWriter.WriteInt(); // lifestyle trophy count
            pWriter.WriteLong(); // joined timestamp
            pWriter.WriteLong(); // current timestamp
            pWriter.WriteByte();
        }
        return pWriter;
    }

    public static PacketWriter InviteSentReceipt(long clubId, Player other)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
        pWriter.Write(ClubPacketMode.InviteSentReceipt);
        pWriter.WriteLong(clubId);
        pWriter.WriteUnicodeString(other.Name);
        return pWriter;
    }

    public static PacketWriter Invite(Club club, Player other)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
        pWriter.Write(ClubPacketMode.Invite);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(club.Name);
        pWriter.WriteUnicodeString(club.Leader.Name);
        pWriter.WriteUnicodeString(other.Name);
        return pWriter;
    }

    public static PacketWriter InviteResponse(Club club, Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
        pWriter.Write(ClubPacketMode.InviteResponse);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(club.Name);
        pWriter.WriteUnicodeString(club.Leader.Name);
        pWriter.WriteUnicodeString(player.Name);
        pWriter.WriteInt(); //00 = accept
        return pWriter;
    }

    public static PacketWriter LeaderInviteResponse(Club club, string invitee, byte response)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
        pWriter.Write(ClubPacketMode.LeaderInviteResponse);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(invitee);
        pWriter.WriteShort(response);
        return pWriter;
    }

    public static PacketWriter LeaveClub(Club club)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
        pWriter.Write(ClubPacketMode.LeaveClub);
        pWriter.WriteLong(club.Id);
        return pWriter;
    }

    public static PacketWriter ChangeBuffReceipt(Club club, int buffId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
        pWriter.Write(ClubPacketMode.ChangeBuffReceipt);
        pWriter.WriteLong(club.Id);
        pWriter.WriteInt(buffId);
        pWriter.WriteInt(0x1);
        return pWriter;
    }

    public static PacketWriter ConfirmCreate(long clubId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
        pWriter.Write(ClubPacketMode.ConfirmCreate);
        pWriter.WriteLong(clubId);
        pWriter.WriteInt();
        pWriter.WriteShort();
        return pWriter;
    }

    public static PacketWriter Disband(Club club)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
        pWriter.Write(ClubPacketMode.Disband);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(club.Leader.Name);
        pWriter.WriteInt(0xCF); //unk
        return pWriter;
    }

    public static PacketWriter ConfirmInvite(Club club, Player other /*, byte response*/)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
        pWriter.Write(ClubPacketMode.ConfirmInvite);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(club.Leader.Name);
        // TODO use method used in the loop inside Create and UpdateClub
        pWriter.WriteByte(0x2); //unk
        pWriter.WriteLong(); //unk
        pWriter.WriteLong(other.AccountId);
        pWriter.WriteLong(other.CharacterId);
        pWriter.WriteUnicodeString(other.Name);
        pWriter.WriteByte();
        pWriter.Write(other.Job);
        pWriter.Write(other.JobCode);
        pWriter.WriteShort(other.Levels.Level);
        pWriter.WriteInt(other.MapId);
        pWriter.WriteShort(); // member.Channel
        pWriter.WriteUnicodeString(other.ProfileUrl);
        pWriter.WriteInt(); // player house mapId
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteLong(); // player house plot expiration timestamp
        pWriter.WriteInt(); // combat trophy count
        pWriter.WriteInt(); // adventure trophy count
        pWriter.WriteInt(); // lifestyle trophy count
        pWriter.WriteLong(); // joined timestamp
        pWriter.WriteLong(); // current timestamp
        pWriter.WriteByte();
        return pWriter;
    }

    public static PacketWriter LeaveNotice(Club club, Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
        pWriter.Write(ClubPacketMode.LeaveNotice);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(player.Name);
        return pWriter;
    }

    public static PacketWriter LogoutNotice(Player player, Club club)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
        pWriter.Write(ClubPacketMode.LogoutNotice);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(player.Name);
        pWriter.WriteLong(); // current timestamp
        return pWriter;
    }

    public static PacketWriter AssignNewLeader(Player player, Club club)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
        pWriter.Write(ClubPacketMode.AssignNewLeader);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(player.Name);
        pWriter.WriteUnicodeString(); // new leader
        pWriter.WriteByte(0x1);
        return pWriter;
    }

    public static PacketWriter ChangeBuff(Club club, int buffId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
        pWriter.Write(ClubPacketMode.ChangeBuff);
        pWriter.WriteLong(club.Id);
        pWriter.WriteInt(buffId);
        pWriter.WriteInt(0x1);
        return pWriter;
    }

    public static PacketWriter UpdatePlayerClubList(Player player, Club club)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
        pWriter.Write(ClubPacketMode.UpdatePlayerClubList);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(player.Name);
        pWriter.WriteLong(player.AccountId);
        pWriter.WriteLong(player.CharacterId);
        pWriter.WriteUnicodeString(player.Name);
        pWriter.WriteByte();
        pWriter.Write(player.Job);
        pWriter.Write(player.JobCode);
        pWriter.WriteShort(player.Levels.Level);
        pWriter.WriteInt(player.MapId);
        pWriter.WriteShort(); // player.Channel
        pWriter.WriteUnicodeString(player.ProfileUrl);
        pWriter.WriteInt(); // player house mapId
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteLong(); // player house plot expiration timestamp
        pWriter.WriteInt(); // combat trophy count
        pWriter.WriteInt(); // adventure trophy count
        pWriter.WriteInt(); // lifestyle trophy count
        return pWriter;
    }

    public static PacketWriter LoginNotice(Player player, Club club)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
        pWriter.Write(ClubPacketMode.LoginNotice);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(player.Name);
        return pWriter;
    }

    public static PacketWriter Rename(Club club, string clubNewName)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
        pWriter.Write(ClubPacketMode.Rename);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(clubNewName);
        pWriter.WriteLong(); //unk
        return pWriter;
    }

    public static PacketWriter Join(Player player, Club club)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
        pWriter.Write(ClubPacketMode.Join);
        pWriter.WriteLong(club.Id);
        pWriter.WriteUnicodeString(player.Name);
        pWriter.WriteUnicodeString(club.Name);
        return pWriter;
    }
}
