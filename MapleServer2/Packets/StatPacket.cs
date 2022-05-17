using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class StatPacket
{
    private enum StatsMode : byte
    {
        SetStats = 0x00,
        UpdateStats = 0x01,
        SendAllStats = 0x23
    }

    /// <summary>
    /// Update specific stats.
    /// </summary>
    public static PacketWriter UpdateStats(IFieldActor actor, params StatAttribute[] attributes)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Stat);
        pWriter.WriteInt(actor.ObjectId);
        pWriter.WriteByte(); // Unknown when to use 0 or 1
        pWriter.WriteByte((byte) attributes.Length);
        foreach (StatAttribute attribute in attributes)
        {
            pWriter.WriteByte((byte) attribute);
            pWriter.WriteStat(attribute, actor.Stats[attribute]);
        }

        return pWriter;
    }

    /// <summary>
    /// Update all stats.
    /// </summary>
    public static PacketWriter SetStats(IFieldActor actor)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Stat);
        pWriter.WriteInt(actor.ObjectId);
        pWriter.WriteByte(); // Unknown (0x00/0x01)
        pWriter.Write(StatsMode.SendAllStats);
        for (int i = 0; i < 35; i++)  // Always 35.
        {
            StatAttribute statAttribute = (StatAttribute) i;
            pWriter.WriteStat(statAttribute, actor.Stats.Data[statAttribute]);
        }

        return pWriter;
    }

    public static void DefaultStatsMob(this PacketWriter pWriter, IFieldActor mob)
    {
        pWriter.Write(StatsMode.SendAllStats);
        pWriter.WriteLong(mob.Stats[StatAttribute.Hp].Bonus);
        pWriter.WriteInt(100); // Move speed (?)
        pWriter.WriteLong(mob.Stats[StatAttribute.Hp].Base);
        pWriter.WriteInt(100); // Move speed (?)
        pWriter.WriteLong(mob.Stats[StatAttribute.Hp].Total);
        pWriter.WriteInt(100); // Move speed (?)
    }

    public static void WriteFieldStats(this PacketWriter pWriter, Stats stats)
    {
        pWriter.Write(StatsMode.SendAllStats);
        for (int i = 0; i < 3; i++)
        {
            pWriter.WriteLong(stats[StatAttribute.Hp][i]);
            pWriter.WriteInt(stats[StatAttribute.AttackSpeed][i]);
            pWriter.WriteInt(stats[StatAttribute.MovementSpeed][i]);
            pWriter.WriteInt(stats[StatAttribute.MountMovementSpeed][i]);
            pWriter.WriteInt(stats[StatAttribute.JumpHeight][i]);
        }
    }

    private static void WriteStat(this PacketWriter pWriter, StatAttribute statAttribute, Stat stat)
    {
        if (statAttribute is StatAttribute.Hp)
        {
            for (int i = 0; i < 3; i++)
            {
                pWriter.WriteLong(stat[i]);
            }

            return;
        }

        for (int i = 0; i < 3; i++)
        {
            pWriter.WriteInt(stat[i]);
        }
    }
}
