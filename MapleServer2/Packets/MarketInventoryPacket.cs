using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets {
    public static class MarketInventoryPacket {
        public static Packet AddEntry() {
            var pWriter = PacketWriter.Of(SendOp.MARKET_INVENTORY);
            pWriter.WriteByte(0x03);
            // ...

            return pWriter;
        }

        public static Packet Count(int count) {
            return PacketWriter.Of(SendOp.MARKET_INVENTORY)
                .WriteByte(0x02)
                .WriteInt(count);
        }

        public static Packet StartList() {
            return PacketWriter.Of(SendOp.MARKET_INVENTORY)
                .WriteByte(0x01);
        }

        public static Packet EndList() {
            return PacketWriter.Of(SendOp.MARKET_INVENTORY)
                .WriteByte(0x08);
        }
    }
}