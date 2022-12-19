using System.Diagnostics;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class SkillUsePacket
{
    public static PacketWriter SkillUse(SkillCast skillCast)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SkillUse);

        pWriter.WriteLong(skillCast.SkillSn);
        pWriter.WriteInt(skillCast.ServerTick);
        Debug.Assert(skillCast.Caster != null, "skillCast.Caster != null");
        pWriter.WriteInt(skillCast.Caster.ObjectId);
        pWriter.WriteInt(skillCast.SkillId);
        pWriter.WriteShort(skillCast.SkillLevel);
        pWriter.WriteByte();
        pWriter.Write(skillCast.Position.ToShort());
        pWriter.Write(skillCast.Direction);
        pWriter.Write(skillCast.Rotation);
        pWriter.WriteShort();
        pWriter.WriteByte();
        pWriter.WriteByte();

        return pWriter;
    }

    // TODO: change to SkillCast (refactor SkillCast / SkillManager)
    public static PacketWriter MobSkillUse(IFieldObject mob, int skillId, short skillLevel, byte part)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SkillUse);
        pWriter.WriteInt(Random.Shared.Next()); // Seems to be an incrementing number - unique id
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
