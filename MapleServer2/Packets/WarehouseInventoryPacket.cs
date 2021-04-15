using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class WarehouseInventoryPacket
    {
        private enum WarehouseInventoryPacketMode : byte
        {
            StartList = 0x1,
            Count = 0x2,
            Load = 0x3,
            Add = 0x4,
            UpdateQuantity = 0x5,
            EndList = 0x8,
        }

        public static Packet StartList()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.WAREHOUSE_INVENTORY);
            pWriter.WriteEnum(WarehouseInventoryPacketMode.StartList);
            return pWriter;
        }

        public static Packet Load(Item item) // this packet is wrong. It needs a modified version of WriteItem
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.WAREHOUSE_INVENTORY);
            pWriter.WriteEnum(WarehouseInventoryPacketMode.Load);
            pWriter.WriteItem(item);
            return pWriter;
        }

        public static Packet Count()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.WAREHOUSE_INVENTORY);
            pWriter.WriteEnum(WarehouseInventoryPacketMode.Count);
            pWriter.WriteInt();
            return pWriter;
        }

        public static Packet Add(long itemUid)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.WAREHOUSE_INVENTORY);
            pWriter.WriteEnum(WarehouseInventoryPacketMode.Add);
            pWriter.WriteLong(itemUid);
            return pWriter;
        }

        public static Packet UpdateQuantity(Item item)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.WAREHOUSE_INVENTORY);
            pWriter.WriteEnum(WarehouseInventoryPacketMode.UpdateQuantity);
            pWriter.WriteLong(item.Uid);
            pWriter.WriteInt(item.Amount);
            return pWriter;
        }

        public static Packet EndList()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.WAREHOUSE_INVENTORY);
            pWriter.WriteEnum(WarehouseInventoryPacketMode.EndList);
            return pWriter;
        }
    }
}
