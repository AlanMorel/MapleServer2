using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class ClubPacket
    {
        public static Packet CreateClub(Player player, Player partyLeader, string clubName, List<Player> members)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte(0x2); // mode
            pWriter.WriteLong(4); //clubID
            pWriter.WriteUnicodeString(clubName);
            pWriter.WriteLong(partyLeader.AccountId);
            pWriter.WriteLong(partyLeader.CharacterId);
            pWriter.WriteUnicodeString(partyLeader.Name);
            pWriter.WriteLong(); //unk
            pWriter.WriteByte(); //unk
            pWriter.WriteInt(); //unk
            pWriter.WriteInt(); //unk
            pWriter.WriteLong(); //unk
            pWriter.WriteByte((byte)members.Count); //loop amount

            foreach (Player member in members)
            {
                pWriter.WriteByte((byte)members.Count); // loop
                pWriter.WriteLong(4); //clubID
                pWriter.WriteLong(member.AccountId);
                pWriter.WriteLong(member.CharacterId);
                pWriter.WriteUnicodeString(member.Name);
                pWriter.WriteByte(00);
                pWriter.WriteInt();
                pWriter.WriteInt();
                pWriter.WriteShort();
                pWriter.WriteInt(member.MapId);
                pWriter.WriteShort();
                pWriter.WriteUnicodeString(member.ProfileUrl); //profile pic
                pWriter.WriteInt(); //player house location
                pWriter.WriteLong();
                pWriter.WriteLong();
                pWriter.WriteInt();
                pWriter.WriteInt();
                pWriter.WriteInt();
                pWriter.WriteLong(); //timestamp
                pWriter.WriteLong(); //timestamp
                pWriter.WriteByte(00);
            }
            return pWriter;
        }
        public static Packet UpdateClubs(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte(0x18); //mode
            pWriter.WriteLong(4); //clubID
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteLong(player.AccountId);
            pWriter.WriteLong(player.CharacterId);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteByte(0x1);
            pWriter.WriteInt(player.JobGroupId);
            pWriter.WriteInt(player.JobId);
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteUnicodeString(player.ProfileUrl);
            pWriter.WriteInt(); //player house mapId
            pWriter.WriteLong(0x3); //unk
            pWriter.WriteLong(0x6007CF34); //unk
            pWriter.WriteLong(); //player house plot expiration timestamp
            pWriter.WriteInt(); //adventure trophy count
            pWriter.WriteInt(); //lifestyle trophy count
            return pWriter;
        }
        public static Packet Invite()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte(0xF); //mode
            pWriter.WriteLong(4); //clubId
            pWriter.WriteInt();
            pWriter.WriteShort();
            return pWriter;
        }
        public static Packet AssignLeader(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte(0x1E); //mode
            pWriter.WriteLong(4); //clubId
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteUnicodeString("Test"); //clubName
            return pWriter;
        }
        public static Packet EstablishClub(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CLUB);
            pWriter.WriteByte(0x01);
            pWriter.WriteLong(4);
            pWriter.WriteUnicodeString("Test"); //clubName
            return pWriter;
        }
    }
}