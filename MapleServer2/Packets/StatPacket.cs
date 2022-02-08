using Maple2Storage.Enums;
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

    public static PacketWriter UpdateStats(IFieldActor player, StatId statId, params StatId[] otherIds)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.STAT);
        pWriter.WriteInt(player.ObjectId);
        pWriter.Write(StatsMode.UpdateStats);
        pWriter.WriteByte((byte) (1 + otherIds.Length));
        pWriter.Write(statId);
        pWriter.WriteStat(player.Stats, statId);
        foreach (StatId otherId in otherIds)
        {
            pWriter.Write(otherId);
            pWriter.WriteStat(player.Stats, statId);
        }

        return pWriter;
    }

    public static PacketWriter UpdateStats(IFieldActor<Player> player, IEnumerable<StatId> statIds)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.STAT);
        pWriter.WriteInt(player.ObjectId);
        pWriter.Write(StatsMode.UpdateStats);
        pWriter.WriteByte((byte) statIds.Count());
        foreach (StatId statId in statIds)
        {
            pWriter.Write(statId);
            pWriter.WriteStat(player.Stats, statId);
        }

        return pWriter;
    }

    public static PacketWriter SetStats(IFieldActor<Player> player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.STAT);
        pWriter.WriteInt(player.ObjectId);
        pWriter.WriteByte(); // Unknown (0x00/0x01)
        pWriter.Write(StatsMode.SendStats);
        foreach ((StatId statId, Stat _) in player.Stats.Data)
        {
            pWriter.WriteStat(player.Stats, statId);
        }

        return pWriter;
    }

    public static PacketWriter UpdateMobStats(IFieldActor mob)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.STAT);
        pWriter.WriteInt(mob.ObjectId);
        pWriter.WriteByte();
        pWriter.WriteByte(1);
        pWriter.Write(StatsMode.UpdateMobStats);
        pWriter.WriteStat(mob.Stats, StatId.Hp);

        return pWriter;
    }

    public static void DefaultStatsMob(this PacketWriter pWriter, IFieldActor mob)
    {
        pWriter.Write(StatsMode.SendStats);
        pWriter.WriteLong(mob.Stats[StatId.Hp].Bonus);
        pWriter.WriteInt(100); // Move speed (?)
        pWriter.WriteLong(mob.Stats[StatId.Hp].Base);
        pWriter.WriteInt(100); // Move speed (?)
        pWriter.WriteLong(mob.Stats[StatId.Hp].Total);
        pWriter.WriteInt(100); // Move speed (?)
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

    public static void WriteStat(this PacketWriter pWriter, Stats stats, StatId statId)
    {
        if (statId == StatId.Hp)
        {
            pWriter.WriteLong(stats[statId].BonusLong);
            pWriter.WriteLong(stats[statId].BaseLong);
            pWriter.WriteLong(stats[statId].TotalLong);
            return;
        }

        for (int i = 0; i < 3; i++)
        {
            pWriter.WriteInt(stats[statId][i]);
        }
    }

    public static void WriteStats(this PacketWriter pWriter, Stats stats)
    {
        foreach ((StatId statId, Stat _) in stats.Data)
        {
            pWriter.WriteStat(stats, statId);
        }
    }

    public static void WriteFieldStats(this PacketWriter pWriter, Stats stats)
    {
        pWriter.Write(StatsMode.SendStats);
        for (int i = 0; i < 3; i++)
        {
            switch (i)
            {
                case 0:
                    pWriter.WriteLong(stats[StatId.Hp].Bonus);
                    break;
                case 1:
                    pWriter.WriteLong(stats[StatId.Hp].Base);
                    break;
                default:
                    pWriter.WriteLong(stats[StatId.Hp].Total);
                    break;
            }
            pWriter.WriteInt(stats[StatId.AttackSpeed][i]);
            pWriter.WriteInt(stats[StatId.MovementSpeed][i]);
            pWriter.WriteInt(stats[StatId.MountMovementSpeed][i]);
            pWriter.WriteInt(stats[StatId.JumpHeight][i]);
        }
    }
}
