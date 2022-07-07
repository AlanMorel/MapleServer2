using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class ItemInventoryPacket
{
    private enum Mode : byte
    {
        Add = 0x00,
        Remove = 0x01,
        UpdateAmount = 0x02,
        Move = 0x03,
        LoadItem = 0x07,
        MarkItemNew = 0x08,
        LoadItemsToTab = 0x0A,
        Expand = 0x0C,
        ResetTab = 0x0D,
        LoadTab = 0x0E,
        UpdateBind = 0x10
    }

    public static PacketWriter Add(Item item)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemInventory);
        pWriter.Write(Mode.Add);
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
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemInventory);
        pWriter.Write(Mode.Remove);
        pWriter.WriteLong(uid);

        return pWriter;
    }

    public static PacketWriter UpdateAmount(long uid, int amount)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemInventory);
        pWriter.Write(Mode.UpdateAmount);
        pWriter.WriteLong(uid);
        pWriter.WriteInt(amount);

        return pWriter;
    }

    public static PacketWriter Move(long dstUid, short srcSlot, long srcUid, short dstSlot)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemInventory);
        pWriter.Write(Mode.Move);
        pWriter.WriteLong(dstUid);
        pWriter.WriteShort(srcSlot);
        pWriter.WriteLong(srcUid);
        pWriter.WriteShort(dstSlot);

        return pWriter;
    }

    public static PacketWriter LoadItem(IReadOnlyCollection<Item> items)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemInventory);
        pWriter.Write(Mode.LoadItem);

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
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemInventory);
        pWriter.Write(Mode.MarkItemNew);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteInt(amount);
        pWriter.WriteUnicodeString();

        return pWriter;
    }

    public static PacketWriter LoadItemsToTab(InventoryTab tab, IReadOnlyCollection<Item> items)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemInventory);
        pWriter.Write(Mode.LoadItemsToTab);
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
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemInventory);
        pWriter.Write(Mode.Expand);

        return pWriter;
    }

    public static PacketWriter ResetTab(InventoryTab tab)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemInventory);
        pWriter.Write(Mode.ResetTab);
        pWriter.WriteInt((int) tab); // index

        return pWriter;
    }

    public static PacketWriter LoadTab(InventoryTab tab, short extraSlots)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemInventory);
        pWriter.Write(Mode.LoadTab);
        pWriter.WriteByte((byte) tab);
        pWriter.WriteInt(extraSlots);

        return pWriter;
    }

    public static PacketWriter UpdateItem(Item item)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemInventory);
        pWriter.Write(Mode.UpdateBind);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteItem(item);
        return pWriter;
    }
}
