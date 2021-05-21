using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class FurnishingInventoryPacket
    {
        private enum FurnishingInventoryPacketMode : byte
        {
            StartList = 0x0,
            Load = 0x1,
            EndList = 0x4
        }

        public static Packet Load(Item item)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FURNISHING_INVENTORY);
            pWriter.WriteEnum(FurnishingInventoryPacketMode.Load);
            pWriter.WriteInt(item.Id);
            pWriter.WriteLong(item.Uid);
            pWriter.WriteLong();
            pWriter.WriteByte();
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
