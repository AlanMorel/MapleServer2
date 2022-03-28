using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class SkillDamagePacket
{
    private enum SkillDamageMode : byte
    {
        SyncDamage = 0x0,
        Damage = 0x1,
        DotDamage = 0x3,
        Heal = 0x4,
        RegionDamage = 0x5,
        TileSkill = 0x6,
        UnkMode7 = 0x7,
        UnkMode8 = 0x8
    }

    public static PacketWriter SyncDamage(SkillCast skillCast, CoordF position, CoordF rotation, IFieldObject<Player> player, List<int> sourceId, byte count,
        List<int> atkCount, List<int> entityId, List<short> animation)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SkillDamage);

        pWriter.Write(SkillDamageMode.SyncDamage);
        pWriter.WriteLong(skillCast.SkillSn);
        pWriter.WriteInt(player.ObjectId);
        pWriter.WriteInt(skillCast.SkillId);
        pWriter.WriteShort(skillCast.SkillLevel);
        pWriter.WriteByte(skillCast.MotionPoint);
        pWriter.WriteByte(skillCast.AttackPoint);
        pWriter.Write(position.ToShort());
        pWriter.Write(rotation);
        pWriter.WriteByte();
        pWriter.WriteInt(skillCast.ServerTick);
        pWriter.WriteByte(count);
        for (int i = 0; i < count; i++)
        {
            pWriter.WriteLong();
            pWriter.WriteInt(atkCount[i]);
            pWriter.WriteInt(sourceId[i]);
            pWriter.WriteInt(entityId[i]); // objectId of the Impact
            pWriter.WriteShort(animation[i]);
        }

        return pWriter;
    }

    public static PacketWriter Damage(SkillCast skillCast, int attackCount, CoordF position, CoordF rotation,
        List<(int targetId, byte damageType, double damage)> damages)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SkillDamage);

        pWriter.Write(SkillDamageMode.Damage);
        pWriter.WriteLong(skillCast.SkillSn);
        pWriter.WriteInt(attackCount);
        pWriter.WriteInt(skillCast.CasterObjectId);
        pWriter.WriteInt(skillCast.CasterObjectId);
        pWriter.WriteInt(skillCast.SkillId);
        pWriter.WriteShort(skillCast.SkillLevel);
        // This values appears on some SkillsId, and others like BossSkill, sometimes is 0
        pWriter.WriteByte(skillCast.MotionPoint); // The value is not always 0
        pWriter.WriteByte(skillCast.AttackPoint); // The value is not always 0, also seems to crash if its not a correct value
        pWriter.Write(position.ToShort());
        pWriter.Write(rotation.ToShort());
        // TODO: Check if is a player or mob
        pWriter.WriteByte((byte) damages.Count);
        foreach ((int targetId, byte damageType, double damage) in damages)
        {
            pWriter.WriteInt(targetId);

            bool flag = damage > 0;

            pWriter.WriteBool(flag);
            if (!flag)
            {
                continue;
            }

            pWriter.WriteByte(damageType); // 0 = normal, 1 = critical, 2 = miss
            pWriter.WriteLong(-1 * (long) damage);
        }

        return pWriter;
    }

    public static PacketWriter DotDamage(int ownerId, int targetId, int tick, DamageType damageType, int damage)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SkillDamage);

        pWriter.Write(SkillDamageMode.DotDamage);
        pWriter.WriteInt(ownerId);
        pWriter.WriteInt(targetId);
        pWriter.WriteInt(tick);
        pWriter.Write(damageType);
        pWriter.WriteInt(damage);

        return pWriter;
    }

    public static PacketWriter Heal(Status status, int healAmount)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SkillDamage);

        pWriter.Write(SkillDamageMode.Heal);
        pWriter.WriteInt(status.Source);
        pWriter.WriteInt(status.Target);
        pWriter.WriteInt(status.UniqueId);
        pWriter.WriteInt(healAmount);
        pWriter.WriteLong();
        pWriter.WriteByte(1);

        return pWriter;
    }

    public static PacketWriter RegionDamage(SkillCast skillCast, List<DamageHandler> damageHandlers)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SkillDamage);

        pWriter.Write(SkillDamageMode.RegionDamage);
        pWriter.WriteLong(); // always 0??
        pWriter.WriteInt(skillCast.CasterObjectId);
        pWriter.WriteInt(skillCast.SkillObjectId);
        pWriter.WriteByte();

        byte damageHandlersCount = (byte) damageHandlers.Count;
        pWriter.WriteByte(damageHandlersCount);
        foreach (DamageHandler damageHandler in damageHandlers)
        {
            pWriter.WriteInt(damageHandler.Target.ObjectId);
            bool flag = damageHandler.Damage > 0;
            pWriter.WriteBool(flag);
            if (!flag)
            {
                continue;
            }

            pWriter.Write(damageHandler.Target.Coord.ToShort());
            pWriter.Write(damageHandler.Target.Velocity);
            pWriter.WriteByte(); // 0 = normal, 1 = critical, 2 = miss
            pWriter.WriteLong((long) (-1 * damageHandler.Damage));
        }

        return pWriter;
    }

    public static PacketWriter TileSkill(SkillCast skillCast, byte targetCount, IFieldObject<Player> player, byte count2, CoordF position, CoordF direction,
        long damage)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SkillDamage);

        pWriter.Write(SkillDamageMode.RegionDamage);
        pWriter.WriteLong(skillCast.SkillSn);
        pWriter.WriteInt(skillCast.SkillId);
        pWriter.WriteShort(skillCast.SkillLevel);
        pWriter.WriteByte(targetCount);
        for (int i = 0; i < targetCount; i++)
        {
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteByte(count2);
            pWriter.Write(position);
            pWriter.Write(direction);
            for (int j = 0; j < count2; j++) // this maybe isn't a loop
            {
                pWriter.WriteByte(1); // 0 = normal, 1 = critical, 2 = miss
                pWriter.WriteLong(damage);
            }
        }

        return pWriter;
    }

    public static PacketWriter UnkDamageMode(int unkInt, int unkInt2, int count)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SkillDamage);

        pWriter.Write(SkillDamageMode.UnkMode7);
        pWriter.WriteInt(unkInt);
        pWriter.WriteInt(count);
        for (int i = 0; i < count; i++)
        {
            pWriter.WriteInt(unkInt2);
        }

        return pWriter;
    }

    public static PacketWriter UnkDamageMode2(long unkLong, bool unkBool, int unkInt, int unkInt2, short unkShort, int unkInt3)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SkillUse);

        pWriter.Write(SkillDamageMode.UnkMode8);
        pWriter.WriteLong(unkLong);
        pWriter.WriteBool(unkBool);
        if (unkBool)
        {
            pWriter.WriteInt(unkInt);
            pWriter.WriteShort(unkShort);
            pWriter.WriteInt(unkInt2);
            pWriter.WriteInt(unkInt3);
        }

        return pWriter;
    }
}
