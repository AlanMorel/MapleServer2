using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class StatPacket
    {
        public static Packet SetStats(IFieldObject<Player> player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.STAT);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteByte();
            pWriter.WriteByte(0x23);
            pWriter.Write(player.Value.Stats);

            return pWriter;
        }

        public static Packet UpdateMobStats(IFieldObject<Mob> mob)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.STAT);
            pWriter.WriteInt(mob.ObjectId);
            pWriter.WriteByte();
            pWriter.WriteByte(1);
            pWriter.WriteByte(4); // value
            // Stats 
            // Damage should be update through this packet
            for (int i = 0; i < 3; i++)
            {
                pWriter.WriteLong(mob.Value.Stats.Hp[i]);
            }

            return pWriter;
        }

        public static void DefaultStatsMob(this PacketWriter pWriter, IFieldObject<Mob> mob)
        {
            pWriter.WriteByte(0x23);
            // Flag dependent
            for (int i = 0; i < 3; i++)
            {
                pWriter.WriteLong(mob.Value.Stats.Hp[i]);
                pWriter.WriteInt();
            }
        }

        public static void DefaultStatsNpc(this PacketWriter pWriter)
        {
            byte flag = 0x23;
            pWriter.WriteByte(flag);
            if (flag == 1)
            {
                byte value = 0;
                pWriter.WriteByte(value); // value
                if (value == 4)
                {
                    pWriter.WriteLong();
                    pWriter.WriteLong();
                    pWriter.WriteLong();
                }
                else
                {
                    pWriter.WriteInt();
                    pWriter.WriteInt();
                    pWriter.WriteInt();
                }
            }
            else
            {
                pWriter.WriteLong(5);
                pWriter.WriteInt();
                pWriter.WriteLong(5);
                pWriter.WriteInt();
                pWriter.WriteLong(5);
                pWriter.WriteInt();
            }
        }

        public static void WriteTotalStats(this PacketWriter pWriter, ref PlayerStats stats)
        {
            pWriter.WriteByte(0x23);
            for (int i = 0; i < 3; i++)
            {
                pWriter.WriteLong(stats.Hp[i]);
                pWriter.WriteInt(stats.AtkSpd[i]);
                pWriter.WriteInt(stats.MoveSpd[i]);
                pWriter.WriteInt(stats.MountSpeed[i]);
                pWriter.WriteInt(stats.JumpHeight[i]);
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
