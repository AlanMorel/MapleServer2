using System;
using System.Collections.Generic;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class SkillUsePacket
    {
        public static readonly Dictionary<long, SkillCast> SkillCastMap = new Dictionary<long, SkillCast>();

        public static Packet SkillUse(SkillCast skillCast, CoordF coords)
        {
            SkillCastMap[skillCast.SkillSN] = skillCast;
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_USE);
            pWriter.WriteLong(skillCast.SkillSN);
            pWriter.WriteInt(skillCast.UnkValue);
            pWriter.WriteInt(skillCast.SkillId);
            pWriter.WriteShort(skillCast.SkillLevel);
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
            pWriter.WriteInt();

            return pWriter;
        }
    }
}
