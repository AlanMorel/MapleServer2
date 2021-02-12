using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class MarketInventoryPacket
    {
        public static Packet AddEntry()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MARKET_INVENTORY);
            pWriter.WriteByte(0x03);
            // ...

            return pWriter;
        }

        public static Packet Count(int count)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MARKET_INVENTORY);
            pWriter.WriteByte(0x02);
            pWriter.WriteInt(count);

            return pWriter;
        }

        public static Packet StartList()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MARKET_INVENTORY);
            pWriter.WriteByte(0x01);

            return pWriter;
        }

        public static Packet EndList()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MARKET_INVENTORY);
            pWriter.WriteByte(0x08);

            return pWriter;
        }
    }
}
