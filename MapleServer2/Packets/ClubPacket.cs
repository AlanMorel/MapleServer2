using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
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
            Join = 0x1E,
        }

        public static Packet UpdateClub(Club club, string clubName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.UpdateClub);
            pWriter.WriteLong(club.Id);
            pWriter.WriteUnicodeString(club.Name);
            pWriter.WriteLong(club.Leader.AccountId);
            pWriter.WriteLong(club.Leader.CharacterId);
            pWriter.WriteUnicodeString(club.Leader.Name);
            pWriter.WriteLong(); //unk
            pWriter.WriteByte(0x2); //unk
            pWriter.WriteLong(); //unk
            pWriter.WriteLong(); //unk
            pWriter.WriteByte((byte)club.Members.Count); //loop amount

            foreach (Player member in club.Members)
            {
                pWriter.WriteByte(0x2);
                pWriter.WriteLong(club.Id);
                pWriter.WriteLong(member.AccountId);
                pWriter.WriteLong(member.CharacterId);
                pWriter.WriteUnicodeString(member.Name);
                pWriter.WriteByte(); //unk
                pWriter.WriteInt(member.JobGroupId);
                pWriter.WriteInt(member.JobId);
                pWriter.WriteShort(member.Level);
                pWriter.WriteInt(member.MapId);
                pWriter.WriteShort(); // member.Channel
                pWriter.WriteUnicodeString(member.ProfileUrl);
                pWriter.WriteInt(); // member house mapId
                pWriter.WriteInt(); // unk
                pWriter.WriteInt(); // unk
                pWriter.WriteLong(); // member house plot expiration timestamp
                pWriter.WriteInt(); // combat trophy count
                pWriter.WriteInt(); // adventure trophy count
                pWriter.WriteInt(); // lifestyle trophy count
                pWriter.WriteLong(); // joined timestamp
                pWriter.WriteLong(); // current timestamp
                pWriter.WriteByte(); // filler
            }
            pWriter.WriteByte(0x1); //unk
            return pWriter;
        }

        public static Packet Establish(Club club)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.Establish);
            pWriter.WriteLong(club.Id);
            pWriter.WriteUnicodeString(club.Name);
            return pWriter;
        }

        public static Packet Create(Party party, Club club)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.Create);
            pWriter.WriteLong(club.Id);
            pWriter.WriteUnicodeString(club.Name);
            pWriter.WriteLong(party.Leader.AccountId);
            pWriter.WriteLong(party.Leader.CharacterId);
            pWriter.WriteUnicodeString(party.Leader.Name);
            pWriter.WriteLong(); // timestamp
            pWriter.WriteByte(0x1); // all hail the magic boolean ;)
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteLong();
            pWriter.WriteByte((byte)party.Members.Count); //loop

            foreach (Player member in party.Members)
            {
                pWriter.WriteByte(0x2);
                pWriter.WriteLong(club.Id);
                pWriter.WriteLong(member.AccountId);
                pWriter.WriteLong(member.CharacterId);
                pWriter.WriteUnicodeString(member.Name);
                pWriter.WriteByte();
                pWriter.WriteInt(member.JobGroupId);
                pWriter.WriteInt(member.JobId);
                pWriter.WriteShort(member.Level);
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

        public static Packet InviteSentReceipt(long clubId, Player other)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.InviteSentReceipt);
            pWriter.WriteLong(clubId);
            pWriter.WriteUnicodeString(other.Name);
            return pWriter;
        }

        public static Packet Invite(Club club, Player other)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.Invite);
            pWriter.WriteLong(club.Id);
            pWriter.WriteUnicodeString(club.Name);
            pWriter.WriteUnicodeString(club.Leader.Name); //clubLeader
            pWriter.WriteUnicodeString(other.Name);
            return pWriter;
        }

        public static Packet InviteResponse(long clubId, string clubName, string clubLeader, Player player, byte response)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.InviteResponse);
            pWriter.WriteLong(clubId);
            pWriter.WriteUnicodeString(clubName);
            pWriter.WriteUnicodeString(clubLeader);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteInt(00); //00 = accept
            return pWriter;
        }

        public static Packet LeaderInviteResponse(long clubId, string invitee, byte response)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.LeaderInviteResponse);
            pWriter.WriteLong(clubId);
            pWriter.WriteUnicodeString(invitee);
            pWriter.WriteShort(response);
            return pWriter;
        }

        public static Packet LeaveClub(Club club)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.LeaveClub);
            pWriter.WriteLong(club.Id);
            return pWriter;
        }

        public static Packet ChangeBuffReceipt(Club club, int buffId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.ChangeBuffReceipt);
            pWriter.WriteLong(club.Id);
            pWriter.WriteInt(buffId);
            pWriter.WriteInt(0x1);
            return pWriter;
        }

        public static Packet ConfirmCreate(long clubId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.ConfirmCreate);
            pWriter.WriteLong(clubId);
            pWriter.WriteInt();
            pWriter.WriteShort();
            return pWriter;
        }

        public static Packet Disband(Club club)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.Disband);
            pWriter.WriteLong(club.Id);
            pWriter.WriteUnicodeString(club.Leader.Name);
            pWriter.WriteInt(0xCF); //unk
            return pWriter;
        }

        public static Packet ConfirmInvite(long clubId, Player other, byte response, string clubLeader)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.ConfirmInvite);
            pWriter.WriteLong(clubId);
            pWriter.WriteUnicodeString(clubLeader);
            pWriter.WriteByte(0x2); //unk
            pWriter.WriteLong(); //unk
            pWriter.WriteLong(other.AccountId);
            pWriter.WriteLong(other.CharacterId);
            pWriter.WriteUnicodeString(other.Name);
            pWriter.WriteByte();
            pWriter.WriteInt(other.JobGroupId);
            pWriter.WriteInt(other.JobId);
            pWriter.WriteShort(other.Level);
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

        public static Packet LeaveNotice(Club club, Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.LeaveNotice);
            pWriter.WriteLong(club.Id);
            pWriter.WriteUnicodeString(player.Name);
            return pWriter;
        }

        public static Packet LogoutNotice(Player player, Club club)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.LogoutNotice);
            pWriter.WriteLong(club.Id);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteLong(); // current timestamp
            return pWriter;
        }

        public static Packet AssignNewLeader(Player player, Club club)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.AssignNewLeader);
            pWriter.WriteLong(club.Id);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteUnicodeString(""); // new leader
            pWriter.WriteByte(0x1);
            return pWriter;
        }

        public static Packet ChangeBuff(Club club, int buffId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.ChangeBuff);
            pWriter.WriteLong(club.Id);
            pWriter.WriteInt(buffId);
            pWriter.WriteInt(0x1);
            return pWriter;
        }

        public static Packet UpdatePlayerClubList(Player player, Club club)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.UpdatePlayerClubList);
            pWriter.WriteLong(club.Id);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteLong(player.AccountId);
            pWriter.WriteLong(player.CharacterId);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteByte();
            pWriter.WriteInt(player.JobGroupId);
            pWriter.WriteInt(player.JobId);
            pWriter.WriteShort(player.Level);
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

        public static Packet LoginNotice(Player player, Club club)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.LoginNotice);
            pWriter.WriteLong(club.Id);
            pWriter.WriteUnicodeString(player.Name);
            return pWriter;
        }

        public static Packet Rename(Club club, string clubNewName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.Rename);
            pWriter.WriteLong(club.Id);
            pWriter.WriteUnicodeString(clubNewName);
            pWriter.WriteLong(); //unk
            return pWriter;
        }

        public static Packet Join(Player player, Club club)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.Join);
            pWriter.WriteLong(club.Id);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteUnicodeString(club.Name);
            return pWriter;
        }
    }
}