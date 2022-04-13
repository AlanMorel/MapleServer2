using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class ItemEnchantTransferPacket
{
    public static PacketWriter Transfer()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemEnchantTransform);
        return pWriter;
    }
}
