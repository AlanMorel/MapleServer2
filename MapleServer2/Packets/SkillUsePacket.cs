using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class SkillUsePacket
    {
        public static Packet SkillUse(int value, long count, int skillId, short skillLevel, CoordF coords)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_USE);
            pWriter.WriteLong(count);
            pWriter.WriteInt(value);    // Unknown
            pWriter.WriteInt(skillId);
            pWriter.WriteShort(skillLevel);
            pWriter.WriteByte();
            pWriter.Write(coords);
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteInt();
            pWriter.WriteByte();
            pWriter.WriteByte();
            return pWriter;
        }

        public static Packet SkillDamage(IFieldObject<Player> player, long skillCount, int someValue, int skillId, short skillLevel, CoordF coords, int objectId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_DAMAGE);
            pWriter.WriteByte(1);
            pWriter.WriteLong(skillCount);
            pWriter.WriteInt(someValue);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteInt(skillId);
            pWriter.WriteShort(skillLevel);
            pWriter.WriteShort(1);
            pWriter.Write(coords);
            pWriter.WriteInt();
            pWriter.WriteByte(9); // Heal or Damage
            pWriter.WriteByte(1); //
            pWriter.WriteByte(1); // How many mobs where hit
            pWriter.WriteInt(objectId);
            pWriter.WriteByte(1);
            pWriter.WriteBool(false);
            pWriter.WriteLong(-1 * 1); // Probably damage deduct
            return pWriter;
        }
    }
}