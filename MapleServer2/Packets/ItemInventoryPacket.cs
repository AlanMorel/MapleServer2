using System.Collections.Generic;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class ItemInventoryPacket
    {
        public static Packet ResetTab(InventoryTab tab)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
            pWriter.WriteByte(0x0D);
            pWriter.WriteInt((int) tab); // index

            return pWriter;
        }

        public static Packet LoadTab(InventoryTab tab)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
            pWriter.WriteByte(0x0E);
            pWriter.WriteByte((byte) tab);
            pWriter.WriteInt();

            return pWriter;
        }

        public static Packet Add(Item item)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
            pWriter.WriteByte(0x00);
            pWriter.WriteInt(item.Id);
            pWriter.WriteLong(item.Uid);
            pWriter.WriteShort(item.Slot);
            pWriter.WriteInt(item.Rarity);
            pWriter.WriteUnicodeString("");
            pWriter.WriteItem(item);
            pWriter.WriteUnicodeString("");

            return pWriter;
        }

        public static Packet Remove(long uid)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
            pWriter.WriteByte(0x01);
            pWriter.WriteLong(uid);

            return pWriter;
        }

        public static Packet Update(long uid, int amount)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
            pWriter.WriteByte(0x02);
            pWriter.WriteLong(uid);
            pWriter.WriteInt(amount);

            return pWriter;
        }

        public static Packet Move(long dstUid, short srcSlot, long srcUid, short dstSlot)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
            pWriter.WriteByte(0x03);
            pWriter.WriteLong(dstUid);
            pWriter.WriteShort(srcSlot);
            pWriter.WriteLong(srcUid);
            pWriter.WriteShort(dstSlot);

            return pWriter;
        }

        public static Packet LoadItem(List<Item> items)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
            pWriter.WriteByte(0x07);

            pWriter.WriteShort((short) items.Count);
            foreach (Item item in items)
            {
                pWriter.WriteItem(item);
            }

            return pWriter;
        }

        // Marks an item in inventory as new
        public static Packet MarkItemNew(Item item, int amount)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
            pWriter.WriteByte(0x08);
            pWriter.WriteLong(item.Uid);
            pWriter.WriteInt(amount);
            pWriter.WriteUnicodeString("");

            return pWriter;
        }

        public static Packet LoadItemsToTab(InventoryTab tab, ICollection<Item> items)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
            pWriter.WriteByte(0x0A);
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
    }
}
