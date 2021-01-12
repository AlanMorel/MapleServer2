using System;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class SkillUsePacket
    {
        public static Packet SkillUse(IFieldObject<Player> player, int value, long count, CoordF coords)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_USE);
            pWriter.WriteLong(count);
            pWriter.WriteInt(value);    // Unknown
            pWriter.WriteInt(player.Value.ActiveSkillId);
            pWriter.WriteShort(player.Value.ActiveSkillLevel);
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

        public static Packet MobSkillUse(IFieldObject<Mob> mob, int skillId, short skillLevel, byte part)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_USE);
            pWriter.WriteInt(new Random().Next()); // Seems to be an incrementing number - unique id
            pWriter.WriteInt(mob.ObjectId);
            pWriter.WriteInt();
            pWriter.WriteInt(mob.ObjectId);
            pWriter.WriteInt(skillId);
            pWriter.WriteShort(skillLevel);
            pWriter.WriteByte(part);
            pWriter.Write(mob.Coord.ToShort());
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.Write(mob.Coord.X);
            return pWriter;
        }
    }
}
