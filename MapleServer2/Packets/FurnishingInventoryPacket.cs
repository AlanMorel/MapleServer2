using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class FurnishingInventoryPacket
    {
        public static Packet AddEntry()
        {
            return PacketWriter.Of(SendOp.FURNISHING_INVENTORY)
                .WriteByte(0x01);
            // ...
        }

        public static Packet StartList()
        {
            return PacketWriter.Of(SendOp.FURNISHING_INVENTORY)
                .WriteByte(0x00);
        }

        public static Packet EndList()
        {
            return PacketWriter.Of(SendOp.FURNISHING_INVENTORY)
                .WriteByte(0x04);
        }
    }
}