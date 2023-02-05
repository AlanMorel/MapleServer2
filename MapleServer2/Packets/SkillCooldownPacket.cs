using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class SkillCooldownPacket
{
    public static PacketWriter SetCooldown(int skillId, int originSkillId, int endTick)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SkillCooldown);

        pWriter.WriteByte(1); // count

        pWriter.WriteInt(skillId);
        pWriter.WriteInt(originSkillId);
        pWriter.WriteInt(endTick);
        pWriter.WriteInt(); // unknown

        return pWriter;
    }

    public static PacketWriter SetCooldowns(int[] skillIds, int[]? originSkillIds, int[]? endTicks, int endTickOffset = 0)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SkillCooldown);

        pWriter.WriteByte((byte) skillIds.Length); // count

        for (int i = 0; i < skillIds.Length; ++i)
        {
            pWriter.WriteInt(skillIds[i]);
            pWriter.WriteInt(originSkillIds?[i]);
            pWriter.WriteInt(endTicks?[i] + endTickOffset);
            pWriter.WriteInt(0); // unknown
        }

        return pWriter;
    }
}
