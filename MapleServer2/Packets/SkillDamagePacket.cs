using System.Collections.Generic;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class SkillDamagePacket
    {
        public static Packet ApplyDamage(IFieldObject<Player> player, long skillUid, int someValue, int skillId, short skillLevel, CoordF coords, List<IFieldObject<Mob>> mobs)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_DAMAGE);
            pWriter.WriteByte(1);
            pWriter.WriteLong(skillUid);
            pWriter.WriteInt(someValue);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteInt(skillId);
            pWriter.WriteInt(skillLevel);
            pWriter.Write(coords.ToShort());
            pWriter.Write(CoordS.From(0, 0, 0));
            /*pWriter.WriteInt(0); // Set as Int because 2 bytes after will set something.
            pWriter.WriteByte(0); // Define is (59 heal) or (9 damage). Not always the case
            pWriter.WriteByte(0); // Unknown - count?*/
            pWriter.WriteByte((byte) mobs.Count); // Mob count
            for (int i = 0; i < mobs.Count; i++)
            {
                pWriter.WriteInt(mobs[i].ObjectId);
                pWriter.WriteByte(1); // Unknown
                pWriter.WriteBool(false); // Crit flag 
                pWriter.WriteLong(-1 * 1); // TODO: Calculate damage
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
