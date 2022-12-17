using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class SkillCooldownPacket
{
    public static PacketWriter SetCooldown(long skillId, int endTick)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SkillCooldown);

        pWriter.WriteByte(1); // count

        pWriter.WriteLong(skillId);
        pWriter.WriteInt(endTick);
        pWriter.WriteInt(0); // unknown

        return pWriter;
    }

    public static PacketWriter SetCooldowns(long[] skillIds, int[] endTicks)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SkillCooldown);

        pWriter.WriteByte((byte) skillIds.Length); // count

        for (int i = 0; i < skillIds.Length; ++i)
        {
            pWriter.WriteLong(skillIds[i]);
            pWriter.WriteInt(endTicks[i]);
            pWriter.WriteInt(0); // unknown
        }

        return pWriter;
    }
}
