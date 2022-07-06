using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class ItemExchangePacket
{
    private enum Mode : byte
    {
        Notice = 0x2
    }

    public static PacketWriter Notice(short noticeId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemExchange);
        pWriter.Write(Mode.Notice);
        pWriter.WriteShort(noticeId);
        return pWriter;
    }
}
