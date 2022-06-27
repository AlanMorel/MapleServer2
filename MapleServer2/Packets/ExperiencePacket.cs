using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class ExperiencePacket
{
    public static PacketWriter ExpUp(long expGained, long expTotal, long restExp)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ExpUp);

        pWriter.WriteByte();
        pWriter.WriteLong(expGained);
        pWriter.WriteShort(); // means something
        pWriter.WriteLong(expTotal);
        pWriter.WriteLong(restExp);
        pWriter.WriteInt(); // counter? increments after every exp_up
        pWriter.WriteByte();

        return pWriter;
    }
}
