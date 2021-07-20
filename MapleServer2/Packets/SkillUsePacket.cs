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

        private enum SkillUseMode : byte
        {
            Mode0 = 0x0,
            Mode1 = 0x1,
            Mode3 = 0x3,
            Mode4 = 0x4,
            Mode5 = 0x5
        }

        public static Packet SkillUse(SkillCast skillCast, CoordF coords)
        {
            SkillCastMap[skillCast.SkillSN] = skillCast;
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_USE);
            pWriter.WriteEnum(SkillUseMode.Mode0);
            pWriter.WriteLong(skillCast.SkillSN);
            pWriter.WriteInt(skillCast.UnkValue);
            pWriter.WriteInt(skillCast.SkillId);
            pWriter.WriteShort(skillCast.SkillLevel);
            pWriter.WriteShort();
            pWriter.Write(coords);
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteInt();
            pWriter.WriteInt(skillCast.UnkValue + 50);
            pWriter.WriteInt();
            pWriter.WriteShort();

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

        public static Packet SkillUseMode1(SkillCast skillCast, int unkInt, int objectId, CoordS coords, short unkShort, byte unkByte, int unkInt2, short unkShort2, long damage)
        {
            SkillCastMap[skillCast.SkillSN] = skillCast;
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_USE);
            pWriter.WriteEnum(SkillUseMode.Mode1);
            pWriter.WriteLong(skillCast.SkillSN);
            pWriter.WriteInt(unkInt);
            pWriter.WriteInt(objectId);
            pWriter.WriteInt(objectId);
            pWriter.WriteInt(skillCast.SkillId);
            pWriter.WriteShort(skillCast.SkillLevel);
            pWriter.WriteShort();
            pWriter.Write(coords);
            pWriter.WriteInt();
            pWriter.WriteShort(unkShort);
            pWriter.WriteByte(unkByte);
            pWriter.WriteInt(unkInt2);
            pWriter.WriteShort(unkShort2);
            pWriter.WriteLong(damage);

            return pWriter;
        }

        public static Packet SkillUseMode3(SkillCast skillCast, int unkInt, int unkInt2, byte unkByte, int unkInt3, long damage)
        {
            SkillCastMap[skillCast.SkillSN] = skillCast;
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_USE);
            pWriter.WriteEnum(SkillUseMode.Mode3);
            pWriter.WriteInt(unkInt);
            pWriter.WriteInt(unkInt2);
            pWriter.WriteByte(unkByte);
            pWriter.WriteInt(unkInt3);
            pWriter.WriteLong(damage);

            return pWriter;
        }

        public static Packet SkillUseMode4(SkillCast skillCast, int objectId)
        {
            SkillCastMap[skillCast.SkillSN] = skillCast;
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_USE);
            pWriter.WriteEnum(SkillUseMode.Mode4);
            pWriter.WriteInt(objectId);
            pWriter.WriteInt(objectId);
            pWriter.WriteLong(skillCast.SkillSN);
            pWriter.WriteLong();
            pWriter.WriteByte(1);

            return pWriter;
        }

        public static Packet SkillUseMode5(int objectId, int unkInt)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_USE);
            pWriter.WriteEnum(SkillUseMode.Mode5);
            pWriter.WriteLong();
            pWriter.WriteInt(objectId);
            pWriter.WriteInt(unkInt);
            pWriter.WriteShort();

            return pWriter;
        }
    }
}
