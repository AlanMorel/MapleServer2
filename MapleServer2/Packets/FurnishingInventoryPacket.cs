using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class FurnishingInventoryPacket
    {
        public static Packet AddEntry()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FURNISHING_INVENTORY);
            pWriter.WriteByte(0x01);
            // ...

            return pWriter;
        }

        public static Packet StartList()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FURNISHING_INVENTORY);
            pWriter.WriteByte(0x00);

            return pWriter;
        }

        public static Packet EndList()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FURNISHING_INVENTORY);
            pWriter.WriteByte(0x04);

            return pWriter;
        }
    }
}
