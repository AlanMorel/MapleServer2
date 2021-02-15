using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class StatPacket
    {
        private enum StatsMode : byte
        {
            SendStats = 0x23,
            UpdateStats = 0x4
        }

        public static Packet SetStats(IFieldObject<Player> player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.STAT);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteByte();
            pWriter.WriteEnum(StatsMode.SendStats);
            pWriter.Write(player.Value.Stats);

            return pWriter;
        }

        public static Packet UpdateMobStats(IFieldObject<Mob> mob)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.STAT);
            pWriter.WriteInt(mob.ObjectId);
            pWriter.WriteByte();
            pWriter.WriteByte(1);
            pWriter.WriteEnum(StatsMode.UpdateStats);
            pWriter.WriteLong(mob.Value.Stats.Hp.Total);
            pWriter.WriteLong(mob.Value.Stats.Hp.Min);
            pWriter.WriteLong(mob.Value.Stats.Hp.Max);

            return pWriter;
        }

        public static void DefaultStatsMob(this PacketWriter pWriter, IFieldObject<Mob> mob)
        {
            pWriter.WriteEnum(StatsMode.SendStats);
            for (int i = 0; i < 3; i++)
            {
                pWriter.WriteLong(mob.Value.Stats.Hp[i]);
                pWriter.WriteInt();
            }
        }

        public static void DefaultStatsNpc(this PacketWriter pWriter)
        {
            byte flag = (byte) StatsMode.SendStats;
            pWriter.WriteEnum(StatsMode.SendStats);
            if (flag == 1)
            {
                byte value = 0;
                pWriter.WriteByte(value);
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
            pWriter.WriteEnum(StatsMode.SendStats);
            for (int i = 0; i < 3; i++)
            {
                pWriter.WriteLong(stats.Hp[i]);
                pWriter.WriteInt(stats.AtkSpd[i]);
                pWriter.WriteInt(stats.MoveSpd[i]);
                pWriter.WriteInt(stats.MountSpeed[i]);
                pWriter.WriteInt(stats.JumpHeight[i]);
            }
        }
    }
}
