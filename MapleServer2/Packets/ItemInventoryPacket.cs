using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class ItemInventoryPacket
{
    private enum InventoryMode : byte
    {
        Add = 0x00,
        Remove = 0x01,
        Update = 0x02,
        Move = 0x03,
        LoadItem = 0x07,
        MarkItemNew = 0x08,
        LoadItemsToTab = 0x0A,
        Expand = 0x0C,
        ResetTab = 0x0D,
        LoadTab = 0x0E
    }

    public static PacketWriter Add(Item item)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
        pWriter.Write(InventoryMode.Add);
        pWriter.WriteInt(item.Id);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteShort(item.Slot);
        pWriter.WriteInt(item.Rarity);
        pWriter.WriteUnicodeString();
        pWriter.WriteItem(item);
        pWriter.WriteUnicodeString();

        return pWriter;
    }

    public static PacketWriter Remove(long uid)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
        pWriter.Write(InventoryMode.Remove);
        pWriter.WriteLong(uid);

        return pWriter;
    }

    public static PacketWriter Update(long uid, int amount)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
        pWriter.Write(InventoryMode.Update);
        pWriter.WriteLong(uid);
        pWriter.WriteInt(amount);

        return pWriter;
    }

    public static PacketWriter Move(long dstUid, short srcSlot, long srcUid, short dstSlot)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
        pWriter.Write(InventoryMode.Move);
        pWriter.WriteLong(dstUid);
        pWriter.WriteShort(srcSlot);
        pWriter.WriteLong(srcUid);
        pWriter.WriteShort(dstSlot);

        return pWriter;
    }

    public static PacketWriter LoadItem(ICollection<Item> items)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
        pWriter.Write(InventoryMode.LoadItem);

        pWriter.WriteShort((short) items.Count);
        foreach (Item item in items)
        {
            pWriter.WriteInt(item.Id);
            pWriter.WriteLong(item.Uid);
            pWriter.WriteShort(item.Slot);
            pWriter.WriteInt(item.Rarity);
            pWriter.WriteItem(item);
        }

        return pWriter;
    }

    // Marks an item in inventory as new
    public static PacketWriter MarkItemNew(Item item, int amount)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
        pWriter.Write(InventoryMode.MarkItemNew);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteInt(amount);
        pWriter.WriteUnicodeString();

        return pWriter;
    }

    public static PacketWriter LoadItemsToTab(InventoryTab tab, ICollection<Item> items)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
        pWriter.Write(InventoryMode.LoadItemsToTab);
        pWriter.WriteInt((int) tab);
        pWriter.WriteShort((short) items.Count);
        foreach (Item item in items)
        {
            pWriter.WriteInt(item.Id);
            pWriter.WriteLong(item.Uid);
            pWriter.WriteShort(item.Slot);
            pWriter.WriteInt(item.Rarity);
            pWriter.WriteItem(item);
        }

        return pWriter;
    }

    public static PacketWriter Expand()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
        pWriter.Write(InventoryMode.Expand);

        return pWriter;
    }

    public static PacketWriter ResetTab(InventoryTab tab)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
        pWriter.Write(InventoryMode.ResetTab);
        pWriter.WriteInt((int) tab); // index

        return pWriter;
    }

    public static PacketWriter LoadTab(InventoryTab tab, short extraSlots)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
        pWriter.Write(InventoryMode.LoadTab);
        pWriter.WriteByte((byte) tab);
        pWriter.WriteInt(extraSlots);

        return pWriter;
    }
}
