using System.Collections.Generic;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class SkillDamagePacket
    {
        public static Packet ApplyDamage(long skillSN, int unkValue, CoordF coords, IFieldObject<Player> player, List<(IFieldObject<Mob>, DamageHandler)> effects)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_DAMAGE);
            SkillCast skillCast = SkillUsePacket.SkillCastMap[skillSN];

            pWriter.WriteByte(1);
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

        public static Packet ApplyHeal(Status status, int healAmount)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_DAMAGE);
            pWriter.WriteByte(4);
            pWriter.WriteInt(status.Source);
            pWriter.WriteInt(status.Owner);
            pWriter.WriteInt(status.UniqueId);
            pWriter.WriteInt(healAmount);
            pWriter.WriteLong();
            pWriter.WriteByte(1);

            return pWriter;
        }
    }
}
