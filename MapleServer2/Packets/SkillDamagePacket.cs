using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
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
            UnkMode8 = 0x8,
        }

        public static Packet SyncDamage(long skillSN, CoordF position, CoordF rotation, IFieldObject<Player> player, List<int> sourceId, byte count, List<int> atkCount, List<int> entityId, List<short> animation)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_DAMAGE);
            SkillCast skillCast = SkillUsePacket.SkillCastMap[skillSN];

            pWriter.WriteEnum(SkillDamageMode.SyncDamage);
            pWriter.WriteLong(skillSN);
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
                pWriter.WriteByte();
                pWriter.WriteByte();
            }

            return pWriter;
        }

        public static Packet Damage(long skillSN, int unkValue, CoordF position, CoordF rotation, IFieldObject<Player> player, List<(IFieldObject<Mob>, DamageHandler)> effects)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_DAMAGE);
            SkillCast skillCast = SkillUsePacket.SkillCastMap[skillSN];

            pWriter.WriteEnum(SkillDamageMode.Damage);
            pWriter.WriteLong(skillSN);
            pWriter.WriteInt(unkValue);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteInt(skillCast.SkillId);
            pWriter.WriteShort(skillCast.SkillLevel);
            // This values appears on some SkillsId, and others like BossSkill, sometimes is 0
            pWriter.WriteByte(skillCast.MotionPoint); // The value is not always 0
            pWriter.WriteByte(skillCast.AttackPoint); // The value is not always 0, also seems to crash if its not a correct value
            pWriter.Write(position.ToShort());
            pWriter.Write(rotation.ToShort()); // Position of the image effect of the skillUse, seems to be rotation (0, 0, rotation).
            // TODO: Check if is a player or mob
            pWriter.WriteByte((byte) effects.Count);
            foreach ((IFieldObject<Mob> mob, DamageHandler damage) in effects)
            {
                pWriter.WriteInt(mob.ObjectId);
                pWriter.WriteBool(damage.Damage > 0);
                pWriter.WriteBool(damage.IsCrit);
                if (damage.Damage != 0)
                {
                    pWriter.WriteLong(-1 * (long) damage.Damage);
                }
            }

            return pWriter;
        }

        public static Packet DotDamage(int ownerId, int targetId, int tick, DamageType damageType, int damage)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_USE);

            pWriter.WriteEnum(SkillDamageMode.DotDamage);
            pWriter.WriteInt(ownerId);
            pWriter.WriteInt(targetId);
            pWriter.WriteInt(tick);
            pWriter.WriteEnum(damageType);
            pWriter.WriteInt(damage);

            return pWriter;
        }

        public static Packet Heal(Status status, int healAmount)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_DAMAGE);

            pWriter.WriteEnum(SkillDamageMode.Heal);
            pWriter.WriteInt(status.Source);
            pWriter.WriteInt(status.Target);
            pWriter.WriteInt(status.UniqueId);
            pWriter.WriteInt(healAmount);
            pWriter.WriteLong();
            pWriter.WriteByte(1);

            return pWriter;
        }

        public static Packet RegionDamage(long skillSN, int objectId, int unkInt, byte count, byte count2, IFieldObject<Player> player, CoordF direction, CoordS blockPosition, DamageType damageType, long damage)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_USE);

            pWriter.WriteEnum(SkillDamageMode.RegionDamage);
            pWriter.WriteLong(skillSN);
            pWriter.WriteInt(objectId);
            pWriter.WriteInt(unkInt);
            pWriter.WriteByte();
            pWriter.WriteByte(count);
            for (int i = 0; i < count; i++)
            {
                pWriter.WriteInt(player.ObjectId);
                pWriter.WriteByte(count2);
                pWriter.Write(blockPosition);
                pWriter.Write(direction);
                for (int j = 0; j < count2; j++)
                {
                    pWriter.WriteEnum(damageType);
                    pWriter.WriteLong(damage);
                }
            }

            return pWriter;
        }

        public static Packet TileSkill(long skillSN, byte targetCount, IFieldObject<Player> player, byte count2, CoordF position, CoordF direction, DamageType damageType, long damage)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_USE);
            SkillCast skillCast = SkillUsePacket.SkillCastMap[skillSN];

            pWriter.WriteEnum(SkillDamageMode.RegionDamage);
            pWriter.WriteLong(skillSN);
            pWriter.WriteInt(skillCast.SkillId);
            pWriter.WriteShort(skillCast.SkillLevel);
            pWriter.WriteByte(targetCount);
            for (int i = 0; i < targetCount; i++)
            {
                pWriter.WriteInt(player.ObjectId);
                pWriter.WriteByte(count2);
                pWriter.Write(position);
                pWriter.Write(direction);
                for (int j = 0; j < count2; j++)
                {
                    pWriter.WriteEnum(damageType);
                    pWriter.WriteLong(damage);
                }
            }

            return pWriter;
        }

        public static Packet UnkDamageMode(int unkInt, int unkInt2, int count)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_USE);

            pWriter.WriteEnum(SkillDamageMode.UnkMode7);
            pWriter.WriteInt(unkInt);
            pWriter.WriteInt(count);
            for (int i = 0; i < count; i++)
            {
                pWriter.WriteInt(unkInt2);
            }

            return pWriter;
        }

        public static Packet UnkDamageMode2(long unkLong, bool unkBool, int unkInt, int unkInt2, short unkShort, int unkInt3)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_USE);

            pWriter.WriteEnum(SkillDamageMode.UnkMode8);
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
}
