using System.Collections.Generic;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class SkillDamagePacket
    {
        public static Packet ApplyDamage(IFieldObject<Player> player, long skillSN, int someValue, CoordF coords, List<IFieldObject<Mob>> mobs)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_DAMAGE);
            SkillCast skillCast = SkillUsePacket.SkillCastMap[skillSN];
            DamageHandler damage = DamageHandler.CalculateSkillDamage(skillCast);

            pWriter.WriteByte(1);
            pWriter.WriteLong(skillSN);
            pWriter.WriteInt(someValue);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteInt(skillCast.GetSkillId());
            pWriter.WriteShort(skillCast.GetSkillLevel());
            // This values appears on some SkillsId, and others like BossSkill, sometimes is 0
            pWriter.WriteByte(); // Unknown value
            pWriter.WriteByte(); // Unknown value, also seems to crash if its not a correct value
            pWriter.Write(coords.ToShort());
            pWriter.Write(CoordS.From(0, 0, 0)); // Position of the image effect of the skillUse, seems to be rotation (0, 0, rotation).
            // TODO: Check if is a player or mob
            pWriter.WriteByte((byte) mobs.Count); // Mob count
            for (int i = 0; i < mobs.Count; i++)
            {
                pWriter.WriteInt(mobs[i].ObjectId);
                pWriter.WriteByte((byte) damage.GetDamage() > 0 ? 1 : 0); // Unknown
                pWriter.WriteBool(damage.IsCritical()); // Crit flag 
                if (damage.GetDamage() != 0)
                {
                    pWriter.WriteLong(-1 * (long) damage.GetDamage()); // TODO: Calculate damage
                }
            }
            return pWriter;
        }

        public static Packet ApplyHeal(IFieldObject<Player> player, int statusUid, int sourceId, int amount)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_DAMAGE);
            pWriter.WriteByte(4);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteInt(sourceId);
            pWriter.WriteInt(statusUid);
            pWriter.WriteInt(amount);
            pWriter.WriteLong();
            pWriter.WriteByte(1);
            return pWriter;
        }

    }
}
