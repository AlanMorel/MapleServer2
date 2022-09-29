using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class SkillCooldownPacket
{
    public static PacketWriter SetCooldown(int skillId, int endTick)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SkillDamage);

        pWriter.WriteByte(1); // count

        pWriter.WriteLong(skillId);
        pWriter.WriteInt(endTick);
        pWriter.WriteInt(0); // unknown

        return pWriter;
    }

    public static PacketWriter SetCooldowns(int[] skillIds, int[] endTicks)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SkillDamage);

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
