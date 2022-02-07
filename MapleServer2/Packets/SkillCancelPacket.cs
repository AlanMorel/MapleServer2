using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class SkillCancelPacket
{
    public static PacketWriter SkillCancel(long skillSn, int playerObjectId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_CANCEL);
        pWriter.WriteLong(skillSn);
        pWriter.WriteInt(playerObjectId);

        return pWriter;
    }
}
