using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{

    public static class ItemExchangePacket
    {
        private enum ItemExchangePacketMode : byte
        {
            Notice = 0x2,
        }

        public static Packet Notice(short noticeId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_EXCHANGE);
            pWriter.WriteEnum(ItemExchangePacketMode.Notice);
            pWriter.WriteShort(noticeId);
            return pWriter;
        }
    }
}
