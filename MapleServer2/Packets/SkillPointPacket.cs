using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class SkillPointPacket
{
    public static PacketWriter ExtraSkillPoints()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SkillPoint);
        pWriter.WriteInt(); // total points
        pWriter.WriteInt(); // source count
        foreach (int i in Enumerable.Range(0, 0))
        {
            pWriter.WriteInt(); // source type
            pWriter.WriteInt(); // count
            foreach (int j in Enumerable.Range(0, 0))
            {
                pWriter.WriteShort(); // job rank
                pWriter.WriteInt(); // points
            }
        }

        return pWriter;
    }
}
