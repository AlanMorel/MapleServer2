using Maple2Storage.Tools;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class SkillUsePacket
{
    public static readonly Dictionary<long, SkillCast> SkillCastMap = new();

    public static PacketWriter SkillUse(SkillCast skillCast, CoordF position, CoordF direction, CoordF rotation)
    {
        SkillCastMap[skillCast.SkillSN] = skillCast;
        PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_USE);

        pWriter.WriteLong(skillCast.SkillSN);
        pWriter.WriteInt(skillCast.ServerTick);
        pWriter.WriteInt(skillCast.EntityId);
        pWriter.WriteInt(skillCast.SkillId);
        pWriter.WriteShort(skillCast.SkillLevel);
        pWriter.WriteByte();
        pWriter.Write(position.ToShort());
        pWriter.Write(direction);
        pWriter.Write(rotation); // rotation
        pWriter.WriteShort();
        pWriter.WriteByte();
        pWriter.WriteByte();

        return pWriter;
    }

    // TODO: change to SkillCast (refactor SkillCast / SkillManager)
    public static PacketWriter MobSkillUse(IFieldObject mob, int skillId, short skillLevel, byte part)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_USE);
        pWriter.WriteInt(RandomProvider.Get().Next()); // Seems to be an incrementing number - unique id
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
