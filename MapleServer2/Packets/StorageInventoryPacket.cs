using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class StorageInventoryPacket
{
    private enum Mode : byte
    {
        Add = 0x00,
        Remove = 0x01,
        Move = 0x02,
        Mesos = 0x03,
        ItemCount = 0x04,
        LoadItems = 0x05,
        ExpandAnim = 0x07,
        Sort = 0x08,
        UpdateItem = 0x09,
        Update = 0x0B,
        Expand = 0x0D
    }

    public static PacketWriter Add(Item item)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.StorageInventory);
        pWriter.Write(Mode.Add);
        pWriter.WriteLong();
        pWriter.WriteInt(item.Id);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteShort(item.Slot);
        pWriter.WriteInt(item.Rarity);
        pWriter.WriteItem(item);

        return pWriter;
    }

    public static PacketWriter Remove(long uid)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.StorageInventory);
        pWriter.Write(Mode.Remove);
        pWriter.WriteLong();
        pWriter.WriteLong(uid);

        return pWriter;
    }

    public static PacketWriter Move(long srcUid, short srcSlot, long dstUid, short dstSlot)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.StorageInventory);
        pWriter.Write(Mode.Move);
        pWriter.WriteLong();
        pWriter.WriteLong(srcUid);
        pWriter.WriteShort(srcSlot);
        pWriter.WriteLong(dstUid);
        pWriter.WriteShort(dstSlot);

        return pWriter;
    }

    public static PacketWriter UpdateMesos(long amount)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.StorageInventory);
        pWriter.Write(Mode.Mesos);
        pWriter.WriteLong(amount);

        return pWriter;
    }

    public static PacketWriter ItemCount(short itemCount)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.StorageInventory);
        pWriter.Write(Mode.ItemCount);
        pWriter.WriteLong();
        pWriter.WriteShort(itemCount);

        return pWriter;
    }

    public static PacketWriter LoadItems(Item[] items)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.StorageInventory);

        pWriter.Write(Mode.LoadItems);
        pWriter.LoadHelper(items);

        return pWriter;
    }

    public static PacketWriter ExpandAnim()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.StorageInventory);
        pWriter.Write(Mode.ExpandAnim);

        return pWriter;
    }

    public static PacketWriter Sort(Item[] items)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.StorageInventory);
        pWriter.Write(Mode.Sort);
        pWriter.LoadHelper(items);

        return pWriter;
    }

    public static PacketWriter UpdateItem(Item item)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.StorageInventory);
        pWriter.Write(Mode.UpdateItem);
        pWriter.WriteLong();
        pWriter.WriteLong(item.Uid);
        pWriter.WriteInt(item.Amount);

        return pWriter;
    }

    public static PacketWriter Update()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.StorageInventory);
        pWriter.Write(Mode.Update);

        return pWriter;
    }

    public static PacketWriter Expand(int amount)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.StorageInventory);
        pWriter.Write(Mode.Expand);
        pWriter.WriteInt(amount);

        return pWriter;
    }

    private static PacketWriter LoadHelper(this PacketWriter pWriter, Item[] items)
    {
        pWriter.WriteLong();
        pWriter.WriteShort((short) items.Count(x => x != null));
        for (short i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                continue;
            }
            pWriter.WriteInt(items[i].Id);
            pWriter.WriteLong(items[i].Uid);
            pWriter.WriteShort(items[i].Slot);
            pWriter.WriteInt(items[i].Rarity);
            pWriter.WriteItem(items[i]);
        }

        return pWriter;
    }
}
