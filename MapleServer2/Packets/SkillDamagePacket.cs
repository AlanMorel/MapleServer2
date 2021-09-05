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
            Mode0 = 0x0,
            Damage = 0x1,
            DotDamage = 0x3,
            Heal = 0x4,
            RegionDamage = 0x5
        }

        public static Packet Mode0(long skillSN, CoordF position, CoordF rotation, IFieldObject<Player> player, byte count, int atkCount, int entityId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_DAMAGE);
            SkillCast skillCast = SkillUsePacket.SkillCastMap[skillSN];

            pWriter.WriteEnum(SkillDamageMode.Mode0);
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
            pWriter.WriteByte(count); // Loop
            for (int i = 0; i < count; i++)
            {
                pWriter.WriteLong();
                pWriter.WriteInt(atkCount);
                pWriter.WriteInt(player.ObjectId);
                pWriter.WriteInt(entityId);
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

        public static Packet DotDamage(int ownerId, int targetId, int tick, DamageTypeId damageType, int damage)
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

        public static Packet RegionDamage(int objectId, int unkInt, byte count)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_USE);
            pWriter.WriteEnum(SkillDamageMode.RegionDamage);
            pWriter.WriteLong();
            pWriter.WriteInt(objectId);
            pWriter.WriteInt(unkInt);
            pWriter.WriteByte();
            pWriter.WriteByte(count);
            for (int i = 0; i < count; i++)
            {

            }

            return pWriter;
        }
    }
}
