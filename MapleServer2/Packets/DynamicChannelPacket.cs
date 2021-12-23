using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public class DynamicChannelPacket
{
    public static PacketWriter DynamicChannel()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.DYNAMIC_CHANNEL);
        pWriter.WriteByte();
        pWriter.WriteShort(10);
        pWriter.WriteShort(9);
        pWriter.WriteShort(9);
        pWriter.WriteShort(9);
        pWriter.WriteShort(9);
        pWriter.WriteShort(10);
        pWriter.WriteShort(10);
        pWriter.WriteShort(10);

        return pWriter;
    }
}
