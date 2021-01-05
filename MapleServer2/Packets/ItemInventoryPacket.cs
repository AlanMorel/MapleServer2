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
            return PacketWriter.Of(SendOp.ITEM_INVENTORY)
                .WriteByte(0x0D)
                .WriteInt((int) tab); // index
        }

        public static Packet LoadTab(InventoryTab tab)
        {
            return PacketWriter.Of(SendOp.ITEM_INVENTORY)
                .WriteByte(0x0E)
                .WriteByte((byte) tab)
                .WriteInt();
        }

        public static Packet Add(Item item)
        {
            return PacketWriter.Of(SendOp.ITEM_INVENTORY)
                .WriteByte(0x00)
                .WriteInt(item.Id)
                .WriteLong(item.Uid)
                .WriteShort(item.Slot)
                .WriteInt(item.Rarity)
                .WriteUnicodeString("")
                .WriteItem(item);
        }

        public static Packet Remove(long uid)
        {
            return PacketWriter.Of(SendOp.ITEM_INVENTORY)
                .WriteByte(0x01)
                .WriteLong(uid);
        }

        public static Packet Update(long uid, int amount)
        {
            return PacketWriter.Of(SendOp.ITEM_INVENTORY)
                .WriteByte(0x02)
                .WriteLong(uid)
                .WriteInt(amount);
        }

        public static Packet Move(long dstUid, short srcSlot, long srcUid, short dstSlot)
        {
            return PacketWriter.Of(SendOp.ITEM_INVENTORY)
                .WriteByte(0x03)
                .WriteLong(dstUid)
                .WriteShort(srcSlot)
                .WriteLong(srcUid)
                .WriteShort(dstSlot);
        }

        public static Packet LoadItem(List<Item> items)
        {
            var pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY)
                .WriteByte(0x07);
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
            return PacketWriter.Of(SendOp.ITEM_INVENTORY)
                .WriteByte(0x08)
                .WriteLong(item.Uid)
                .WriteInt(amount)
                .WriteUnicodeString("");
        }

        public static Packet LoadItemsToTab(InventoryTab tab, ICollection<Item> items)
        {
            var pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY)
                .WriteByte(0x0A)
                .WriteInt((int) tab);
            pWriter.WriteShort((short) items.Count);
            foreach (Item item in items)
            {
                pWriter.WriteInt(item.Id)
                    .WriteLong(item.Uid)
                    .WriteShort(item.Slot)
                    .WriteInt(item.Rarity)
                    .WriteItem(item);
            }

            return pWriter;
        }
    }
}
