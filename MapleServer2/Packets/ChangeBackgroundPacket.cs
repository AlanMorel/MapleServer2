using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class ChangeBackgroundPacket
{
    public static PacketWriter ChangeBackground(string dds)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ChangeBackground);

        pWriter.WriteString(dds);
        return pWriter;
    }
}
