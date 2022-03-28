using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class ItemExchangePacket
{
    private enum ItemExchangePacketMode : byte
    {
        Notice = 0x2
    }

    public static PacketWriter Notice(short noticeId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemExchange);
        pWriter.Write(ItemExchangePacketMode.Notice);
        pWriter.WriteShort(noticeId);
        return pWriter;
    }
}
