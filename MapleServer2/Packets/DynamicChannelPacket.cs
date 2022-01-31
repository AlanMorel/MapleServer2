using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class DynamicChannelPacket
{
    public static PacketWriter DynamicChannel(short channelCount)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.DYNAMIC_CHANNEL);
        pWriter.WriteByte();
        pWriter.WriteShort(10);
        pWriter.WriteShort(channelCount);
        pWriter.WriteShort(9);
        pWriter.WriteShort(9);
        pWriter.WriteShort(9);
        pWriter.WriteShort(10);
        pWriter.WriteShort(10);
        pWriter.WriteShort(10);

        return pWriter;
    }
}
