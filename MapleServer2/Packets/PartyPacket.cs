using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class PartyPacket
    {
        public static Packet SendInvite(Player recipient, Player sender)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteByte(11) //Mode?
                .WriteUnicodeString(sender.Name)
                .WriteShort(3843)
                .WriteByte()
                .WriteByte();

            return pWriter;
        }

        public static Packet JoinParty(Player joiner, Player leader)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                 .WriteByte(0x0009) //Mode
                 .WriteByte(1)
                 .WriteInt(3843)
                 .WriteLong(leader.CharacterId)
                 .WriteByte(1)
                 .WriteByte();

            CharacterListPacket.WriteCharacter(joiner, pWriter);

            pWriter.WriteTotalStats(ref joiner.Stats);

            pWriter.WriteInt(joiner.JobId);
            pWriter.WriteByte(1);
            pWriter.WriteInt(joiner.JobGroupId);
            JobPacket.WriteSkills(pWriter, joiner);

            return pWriter;
        }

        public static Packet JoinParty2(Player joiner, Player leader)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                 .WriteByte(0x0002); //Mode

            CharacterListPacket.WriteCharacter(joiner, pWriter);

            pWriter.WriteTotalStats(ref joiner.Stats);

            pWriter.WriteInt(joiner.JobId);
            pWriter.WriteByte(1);
            pWriter.WriteInt(joiner.JobGroupId);
            JobPacket.WriteSkills(pWriter, joiner);

            return pWriter;
        }

        public static Packet JoinParty3(Player joiner, Player leader)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteByte(0x0013)
                .WriteLong(leader.CharacterId)
                .WriteLong(joiner.AccountId)
                .WriteInt()
                .WriteInt()
                .WriteShort();
            return pWriter;
        }

        private static void WriteTotalStats(this PacketWriter pWriter, ref PlayerStats stats)
        {
            pWriter.WriteByte(0x23);
            for (int i = 0; i < 3; i++)
            {
                pWriter.WriteLong(stats.Hp[i])
                    .WriteInt(stats.AtkSpd[i])
                    .WriteInt(stats.MoveSpd[i])
                    .WriteInt(stats.MountSpeed[i])
                    .WriteInt(stats.JumpHeight[i]);
            }

            /* Alternative Stat Struct
            pWriter.WriteByte(); // Count
            for (int i = 0; i < count; i++) {
                pWriter.WriteByte(); // Type
                if (type == 4) pWriter.WriteLong();
                else pWriter.WriteInt();
            }
            */
        }

    }
}

// Party invite (Client -> Server)
// 01 09 00 42 00 75 00 62 00 62 00 6C 00 65 00 47 00 75 00 6E 00
// 