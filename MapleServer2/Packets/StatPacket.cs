using System.Collections;
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
            UpdateMobStats = 0x4
        }

        public static Packet SetStats(IFieldObject<Player> player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.STAT);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteByte();
            pWriter.WriteEnum(StatsMode.SendStats);
            WriteStats(ref pWriter, player.Value.Stats);

            return pWriter;
        }

        public static Packet UpdateStats(IFieldObject<Player> player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.STAT);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteByte();                // Unknown (0x00/0x01)
            pWriter.WriteByte(0x23);            // Unknown
            WriteStats(ref pWriter, player.Value.Stats);

            return pWriter;
        }

        public static Packet UpdateMobStats(IFieldObject<Mob> mob)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.STAT);
            pWriter.WriteInt(mob.ObjectId);
            pWriter.WriteByte();
            pWriter.WriteByte(1);
            pWriter.WriteEnum(StatsMode.UpdateMobStats);
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

        public static void WriteStats(ref PacketWriter pWriter, PlayerStats stats)
        {
            foreach (DictionaryEntry entry in stats.Data)
            {
                if (entry.Key.Equals(PlayerStatId.Hp))
                {
                    // Iterate through Bonuses, Base, Capped. "(Normal, Min, Max)"
                    for (int i = 0; i < 3; i++)
                    {
                        pWriter.WriteLong(((PlayerStat) entry.Value)[i]);
                    }
                    continue;
                }
                pWriter.Write((PlayerStat) entry.Value);
            }
        }

        public static void WriteFieldStats(this PacketWriter pWriter, PlayerStats stats)
        {
            pWriter.WriteEnum(StatsMode.SendStats);
            for (int i = 0; i < 3; i++)
            {
                pWriter.WriteLong(stats[PlayerStatId.Hp][i]);
                pWriter.WriteInt(stats[PlayerStatId.AtkSpd][i]);
                pWriter.WriteInt(stats[PlayerStatId.MoveSpd][i]);
                pWriter.WriteInt(stats[PlayerStatId.MountSpeed][i]);
                pWriter.WriteInt(stats[PlayerStatId.JumpHeight][i]);
            }
        }
    }
}
