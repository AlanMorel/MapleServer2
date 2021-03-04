using System.Collections.Generic;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class SkillDamagePacket
    {
        public static Packet ApplyDamage(IFieldObject<Player> player, long skillSN, int unkValue, CoordF coords, List<IFieldObject<Mob>> mobs)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_DAMAGE);
            SkillCast skillCast = SkillUsePacket.SkillCastMap[skillSN];
            DamageHandler damage = DamageHandler.CalculateSkillDamage(skillCast);

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
            pWriter.WriteByte((byte) mobs.Count);
            for (int i = 0; i < mobs.Count; i++)
            {
                IFieldObject<Mob> mob = mobs[i];
                if (mob == null)
                {
                    // Invalid mob? Investigate.
                    continue;
                }
                pWriter.WriteInt(mobs[i].ObjectId);
                pWriter.WriteByte((byte) damage.GetDamage() > 0 ? 1 : 0);
                pWriter.WriteBool(damage.IsCritical());
                if (damage.GetDamage() != 0)
                {
                    pWriter.WriteLong(-1 * (long) damage.GetDamage());
                }
            }

            return pWriter;
        }

        public static Packet ApplyHeal(IFieldObject<Player> player, Status status)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_DAMAGE);
            pWriter.WriteByte(4);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteInt(status.Source);
            pWriter.WriteInt(status.UniqueId);
            pWriter.WriteInt(status.Stacks);
            pWriter.WriteLong();
            pWriter.WriteByte(1);

            return pWriter;
        }
    }
}
