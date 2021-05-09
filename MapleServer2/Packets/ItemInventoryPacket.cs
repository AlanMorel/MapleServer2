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

        public static Packet Add(Item item)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
            pWriter.WriteEnum(InventoryMode.Add);
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
            pWriter.WriteEnum(InventoryMode.Remove);
            pWriter.WriteLong(uid);

            return pWriter;
        }

        public static Packet Update(long uid, int amount)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
            pWriter.WriteEnum(InventoryMode.Update);
            pWriter.WriteLong(uid);
            pWriter.WriteInt(amount);

            return pWriter;
        }

        public static Packet Move(long dstUid, short srcSlot, long srcUid, short dstSlot)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
            pWriter.WriteEnum(InventoryMode.Move);
            pWriter.WriteLong(dstUid);
            pWriter.WriteShort(srcSlot);
            pWriter.WriteLong(srcUid);
            pWriter.WriteShort(dstSlot);

            return pWriter;
        }

        public static Packet LoadItem(List<Item> items)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
            pWriter.WriteEnum(InventoryMode.LoadItem);

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
        public static Packet MarkItemNew(Item item, int amount)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
            pWriter.WriteEnum(InventoryMode.MarkItemNew);
            pWriter.WriteLong(item.Uid);
            pWriter.WriteInt(amount);
            pWriter.WriteUnicodeString("");

            return pWriter;
        }

        public static Packet LoadItemsToTab(InventoryTab tab, ICollection<Item> items)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
            pWriter.WriteEnum(InventoryMode.LoadItemsToTab);
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

        public static Packet Expand()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
            pWriter.WriteEnum(InventoryMode.Expand);

            return pWriter;
        }

        public static Packet ResetTab(InventoryTab tab)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
            pWriter.WriteEnum(InventoryMode.ResetTab);
            pWriter.WriteInt((int) tab); // index

            return pWriter;
        }

        public static Packet LoadTab(InventoryTab tab, short extraSlots)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_INVENTORY);
            pWriter.WriteEnum(InventoryMode.LoadTab);
            pWriter.WriteByte((byte) tab);
            pWriter.WriteInt(extraSlots);

            return pWriter;
        }
    }
}
