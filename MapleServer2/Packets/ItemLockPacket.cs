using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class ItemLockPacket
    {
        private enum ItemLockMode : byte
        {
            Add = 0x01,
            Remove = 0x02,
            Update = 0x03,
        }

        public static Packet Add(long uid, short slot)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_LOCK);
            pWriter.WriteEnum(ItemLockMode.Add);
            pWriter.WriteLong(uid);
            pWriter.WriteShort(slot);

            return pWriter;
        }

        public static Packet Remove(long uid)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_LOCK);
            pWriter.WriteEnum(ItemLockMode.Remove);
            pWriter.WriteLong(uid);

            return pWriter;
        }

        public static Packet UpdateItems(List<Item> items)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_LOCK);
            pWriter.WriteEnum(ItemLockMode.Update);
            pWriter.WriteByte((byte) items.Count);
            foreach (Item item in items)
            {
                pWriter.WriteLong(item.Uid);
                pWriter.WriteItem(item);
            }

            return pWriter;
        }
    }
}
