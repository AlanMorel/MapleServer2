using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class StatPacket
{
    private enum StatsMode : byte
    {
        UpdateStats = 0x01,
        SendStats = 0x23,
        UpdateMobStats = 0x4
    }

    public static PacketWriter UpdateStats(IFieldObject<Player> player, PlayerStatId statId, params PlayerStatId[] otherIds)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.STAT);
        pWriter.WriteInt(player.ObjectId);
        pWriter.Write(StatsMode.UpdateStats);
        pWriter.WriteByte((byte) (1 + otherIds.Length));
        pWriter.Write(statId);
        pWriter.WriteStat(statId, player.Value.Stats[statId]);
        foreach (PlayerStatId otherId in otherIds)
        {
            pWriter.Write(otherId);
            pWriter.WriteStat(otherId, player.Value.Stats[otherId]);
        }

        return pWriter;
    }

    public static PacketWriter UpdateStats(IFieldObject<Player> player, List<PlayerStatId> statIds)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.STAT);
        pWriter.WriteInt(player.ObjectId);
        pWriter.Write(StatsMode.UpdateStats);
        pWriter.WriteByte((byte) statIds.Count);
        foreach (PlayerStatId statId in statIds)
        {
            pWriter.Write(statId);
            pWriter.WriteStat(statId, player.Value.Stats[statId]);
        }

        return pWriter;
    }

    public static PacketWriter SetStats(IFieldObject<Player> player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.STAT);
        pWriter.WriteInt(player.ObjectId);
        pWriter.WriteByte(); // Unknown (0x00/0x01)
        pWriter.Write(StatsMode.SendStats);
        foreach (KeyValuePair<PlayerStatId, PlayerStat> entry in player.Value.Stats.Data)
        {
            pWriter.WriteStat(entry.Key, entry.Value);
        }

        return pWriter;
    }

    public static PacketWriter UpdateMobStats(IFieldObject<Mob> mob)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.STAT);
        pWriter.WriteInt(mob.ObjectId);
        pWriter.WriteByte();
        pWriter.WriteByte(1);
        pWriter.Write(StatsMode.UpdateMobStats);
        pWriter.WriteLong(mob.Value.Stats.Hp.Bonus);
        pWriter.WriteLong(mob.Value.Stats.Hp.Base);
        pWriter.WriteLong(mob.Value.Stats.Hp.Total);

        return pWriter;
    }

    public static void DefaultStatsMob(this PacketWriter pWriter, IFieldObject<Mob> mob)
    {
        pWriter.Write(StatsMode.SendStats);
        for (int i = 0; i < 3; i++)
        {
            pWriter.WriteLong(mob.Value.Stats.Hp[i]);
            pWriter.WriteInt(100); // Move speed (?)
        }
    }

    public static void DefaultStatsNpc(this PacketWriter pWriter)
    {
        byte flag = (byte) StatsMode.SendStats;
        pWriter.Write(StatsMode.SendStats);
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

    public static void WriteStat(this PacketWriter pWriter, PlayerStatId statId, PlayerStat stat)
    {
        if (statId == PlayerStatId.Hp)
        {
            for (int i = 0; i < 3; i++)
            {
                pWriter.WriteLong(stat[i]);
            }
            return;
        }
        pWriter.Write(stat);
    }

    public static void WriteFieldStats(this PacketWriter pWriter, PlayerStats stats)
    {
        pWriter.Write(StatsMode.SendStats);
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
