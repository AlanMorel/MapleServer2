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
            Damage = 0x1,
            Mode3 = 0x3,
            Heal = 0x4,
            Mode5 = 0x5
        }

        public static Packet Damage(long skillSN, int unkValue, CoordF coords, IFieldObject<Player> player, List<(IFieldObject<Mob>, DamageHandler)> effects)
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
            pWriter.WriteByte(); // The value is not always 0
            pWriter.WriteByte(); // The value is not always 0, also seems to crash if its not a correct value
            pWriter.Write(coords.ToShort());
            pWriter.Write(CoordS.From(0, 0, 0)); // Position of the image effect of the skillUse, seems to be rotation (0, 0, rotation).
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

        public static Packet Mode3(int unkInt, int unkInt2, byte unkByte, int unkInt3, long damage)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_USE);
            pWriter.WriteEnum(SkillDamageMode.Mode3);
            pWriter.WriteInt(unkInt);
            pWriter.WriteInt(unkInt2);
            pWriter.WriteByte(unkByte);
            pWriter.WriteInt(unkInt3);
            pWriter.WriteLong(damage);

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

        public static Packet Mode5(int objectId, int unkInt)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_USE);
            pWriter.WriteEnum(SkillDamageMode.Mode5);
            pWriter.WriteLong();
            pWriter.WriteInt(objectId);
            pWriter.WriteInt(unkInt);
            pWriter.WriteShort();

            return pWriter;
        }
    }
}
