using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class WarehouseInventoryPacket
{
    private enum WarehouseInventoryPacketMode : byte
    {
        StartList = 0x1,
        Count = 0x2,
        Load = 0x3,
        Remove = 0x4,
        GainItemMessage = 0x5,
        UpdateAmount = 0x7,
        EndList = 0x8
    }

    public static PacketWriter StartList()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.WarehouseInventory);
        pWriter.Write(WarehouseInventoryPacketMode.StartList);

        return pWriter;
    }

    public static PacketWriter Count(int amount)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.WarehouseInventory);
        pWriter.Write(WarehouseInventoryPacketMode.Count);
        pWriter.WriteInt(amount);

        return pWriter;
    }

    public static PacketWriter Load(Item item, int counter)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.WarehouseInventory);
        pWriter.Write(WarehouseInventoryPacketMode.Load);
        pWriter.WriteInt(item.Id);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteByte(1); // unknown
        pWriter.WriteInt(counter);
        pWriter.WriteItem(item);

        return pWriter;
    }

    public static PacketWriter Remove(long itemUid)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.WarehouseInventory);
        pWriter.Write(WarehouseInventoryPacketMode.Remove);
        pWriter.WriteLong(itemUid);

        return pWriter;
    }

    public static PacketWriter GainItemMessage(Item item, int amount)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.WarehouseInventory);
        pWriter.Write(WarehouseInventoryPacketMode.GainItemMessage);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteInt(amount);

        return pWriter;
    }

    public static PacketWriter UpdateAmount(long itemUid, int amount)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.WarehouseInventory);
        pWriter.Write(WarehouseInventoryPacketMode.UpdateAmount);
        pWriter.WriteLong(itemUid);
        pWriter.WriteInt(amount);

        return pWriter;
    }

    public static PacketWriter EndList()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.WarehouseInventory);
        pWriter.Write(WarehouseInventoryPacketMode.EndList);

        return pWriter;
    }
}
