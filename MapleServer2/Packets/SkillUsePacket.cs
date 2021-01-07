using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class SkillUsePacket
    {
        public static Packet SkillUse(int value, long count, int skillId, short skillLevel, CoordF coords)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_USE);
            pWriter.WriteLong(count);
            pWriter.WriteInt(value);    // Unknown
            pWriter.WriteInt(skillId);
            pWriter.WriteShort(skillLevel);
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
    }
}
