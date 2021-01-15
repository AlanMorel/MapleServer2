using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class SkillDamagePacket
    {
        public static Packet SkillDamage(IFieldObject<Player> player, long skillUid, int someValue, int skillId, short skillLevel, CoordF coords, int objectId)
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
            pWriter.WriteShort(); // coords2
            pWriter.WriteShort(); // coords2
            pWriter.WriteShort(); // coords2 not always 0? 2B 01 => 11009
            pWriter.WriteByte(1); // Mob count
            pWriter.WriteInt(objectId);
            pWriter.WriteByte(1); // Unknown
            pWriter.WriteBool(false); // Crit flag
            pWriter.WriteLong(-1 * 1); // Damage
            return pWriter;
        }
    }
}
