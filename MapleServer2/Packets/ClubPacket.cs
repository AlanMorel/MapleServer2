using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class ClubPacket
    {
        // TODO convert all send mode bytes to use an enum
        // TODO re-order these packet methods in order of their mode values

        public static Packet CreateClub(Party party, string clubName, long clubId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte(0x2);
            pWriter.WriteLong(clubId);
            pWriter.WriteUnicodeString(clubName);
            pWriter.WriteLong(party.Leader.AccountId);
            pWriter.WriteLong(party.Leader.CharacterId);
            pWriter.WriteUnicodeString(party.Leader.Name);
            pWriter.WriteLong(); // timestamp
            pWriter.WriteByte(0x1); // all hail the magic boolean ;)
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteLong();
            pWriter.WriteByte((byte) party.Members.Count);

            foreach (Player member in party.Members)
            {
                pWriter.WriteByte((byte)party.Members.Count);
                pWriter.WriteLong(clubId);
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
                pWriter.WriteInt(); // adventure trophy count
                pWriter.WriteInt(); // lifestyle trophy count
                pWriter.WriteInt();
                pWriter.WriteLong(); // timestamp
                pWriter.WriteLong(); // timestamp
                pWriter.WriteByte();
            }
            return pWriter;
        }

        public static Packet UpdateClub(Player player, long clubId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte(0x18);
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
            pWriter.WriteInt(); // adventure trophy count
            pWriter.WriteInt(); // lifestyle trophy count
            pWriter.WriteInt();
            return pWriter;
        }

        public static Packet ClubCreated(long clubId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte(0xF);
            pWriter.WriteLong(clubId);
            pWriter.WriteInt();
            pWriter.WriteShort();
            return pWriter;
        }

        public static Packet AssignLeader(Player player, long clubId, string clubName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte(0x1E);
            pWriter.WriteLong(clubId);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteUnicodeString(clubName);
            return pWriter;
        }

        public static Packet EstablishClub(long clubId, string clubName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte(0x01);
            pWriter.WriteLong(clubId);
            pWriter.WriteUnicodeString(clubName);
            return pWriter;
        }
    }
}