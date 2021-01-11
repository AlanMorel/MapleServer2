using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class StatPacket
    {
        public static Packet SetStats(IFieldObject<Player> player)
        {
            return PacketWriter.Of(SendOp.STAT)
                .WriteInt(player.ObjectId)
                .WriteByte()
                .WriteByte(0x23)
                .Write(player.Value.Stats);
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
                pWriter.WriteLong(mob.Value.stats.Hp[i]);
            }
            return pWriter;
        }

        public static void DefaultStatsMob(this PacketWriter pWriter, IFieldObject<Mob> mob)
        {
            pWriter.WriteByte(0x23);
            // Flag dependent
            for (int i = 0; i < 3; i++)
            {
                pWriter.WriteLong(mob.Value.stats.Hp[i])
                    .WriteInt();
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
                    pWriter.WriteLong()
                        .WriteLong()
                        .WriteLong();
                }
                else
                {
                    pWriter.WriteInt()
                        .WriteInt()
                        .WriteInt();
                }
            }
            else
            {
                pWriter.WriteLong(5)
                    .WriteInt()
                    .WriteLong(5)
                    .WriteInt()
                    .WriteLong(5)
                    .WriteInt();
            }
        }

        public static void WriteTotalStats(this PacketWriter pWriter, ref PlayerStats stats)
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
