using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class StorageInventoryPacket
    {
        private enum ItemStorageMode : byte
        {
            Add = 0x00,
            Remove = 0x01,
            Move = 0x02,
            Mesos = 0x03,
            LoadItems = 0x05,
            ExpandAnim = 0x07,
            Sort = 0x08,
            Update = 0x0B,
            Expand = 0x0D,
        }

        public static Packet Add(Item item)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.STORAGE_INVENTORY);
            pWriter.WriteEnum(ItemStorageMode.Add);
            pWriter.WriteLong();
            pWriter.WriteInt(item.Id);
            pWriter.WriteLong(item.Uid);
            pWriter.WriteShort(item.Slot);
            pWriter.WriteInt(item.Rarity);
            pWriter.WriteItem(item);

            return pWriter;
        }

        public static Packet Remove(long uid)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.STORAGE_INVENTORY);
            pWriter.WriteEnum(ItemStorageMode.Remove);
            pWriter.WriteLong();
            pWriter.WriteLong(uid);

            return pWriter;
        }

        public static Packet Move(long srcUid, short srcSlot, long dstUid, short dstSlot)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.STORAGE_INVENTORY);
            pWriter.WriteEnum(ItemStorageMode.Move);
            pWriter.WriteLong();
            pWriter.WriteLong(srcUid);
            pWriter.WriteShort(srcSlot);
            pWriter.WriteLong(dstUid);
            pWriter.WriteShort(dstSlot);

            return pWriter;
        }

        public static Packet UpdateMesos(long amount)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.STORAGE_INVENTORY);
            pWriter.WriteEnum(ItemStorageMode.Mesos);
            pWriter.WriteLong(amount);

            return pWriter;
        }

        public static Packet LoadItems(Item[] items)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.STORAGE_INVENTORY);

            pWriter.WriteEnum(ItemStorageMode.LoadItems);
            pWriter.LoadHelper(items);

            return pWriter;
        }

        public static Packet ExpandAnim()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.STORAGE_INVENTORY);
            pWriter.WriteEnum(ItemStorageMode.ExpandAnim);

            return pWriter;
        }

        public static Packet Sort(Item[] items)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.STORAGE_INVENTORY);
            pWriter.WriteEnum(ItemStorageMode.Sort);
            pWriter.LoadHelper(items);

            return pWriter;
        }

        public static Packet Update()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.STORAGE_INVENTORY);
            pWriter.WriteEnum(ItemStorageMode.Update);

            return pWriter;
        }

        public static Packet Expand(int amount)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.STORAGE_INVENTORY);
            pWriter.WriteEnum(ItemStorageMode.Expand);
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
}
