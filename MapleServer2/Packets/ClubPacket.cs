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
            EstablishClub = 0x1,
            Create = 0x2,
            InviteSentReceipt = 0x6,
            Invite = 0x7,
            InviteResponse = 0x8,
            UpdateList = 0x18,
            ClubCreated = 0xF,
            Join = 0x1E,
        }

        public static Packet UpdateClub(long clubId, string clubName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.UpdateClub);

            return pWriter;
        }

        public static Packet EstablishClub(Club club)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.EstablishClub);
            pWriter.WriteLong(club.Id);
            pWriter.WriteUnicodeString(club.Name);
            return pWriter;
        }

        public static Packet CreateClub(Party party, Club club)
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
            pWriter.WriteByte((byte)party.Members.Count);

            foreach (Player member in party.Members)
            {
                pWriter.WriteByte((byte)party.Members.Count);
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
                pWriter.WriteLong(); // timestamp
                pWriter.WriteLong(); // timestamp
                pWriter.WriteByte();
            }
            return pWriter;
        }

        public static Packet UpdatePlayerClubList(Player player, long clubId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.UpdateList);
            pWriter.WriteLong(clubId);
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

        public static Packet ClubCreated(long clubId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.ClubCreated);
            pWriter.WriteLong(clubId);
            pWriter.WriteInt();
            pWriter.WriteShort();
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

        public static Packet Invite(long clubId, string clubName, Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.Invite);
            pWriter.WriteLong(clubId);
            pWriter.WriteUnicodeString(clubName);
            pWriter.WriteUnicodeString(""); //clubLeader
            pWriter.WriteUnicodeString(player.Name);
            return pWriter;
        }

        public static Packet InviteSentReceipt(long clubId, string invitee)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte((byte)ClubPacketMode.InviteSentReceipt);
            pWriter.WriteLong(clubId);
            pWriter.WriteUnicodeString(invitee);
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
            pWriter.WriteByte(response); //00 = accept
            return pWriter;
        }
    }
}