using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class ItemRepackagePacket
    {
        private enum ItemRepackagePacketMode : byte
        {
            Open = 0x0,
            Repackage = 0x2,
            Notice = 0x3,
        }

        public static Packet Open(long itemUid)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_REPACKAGE);
            pWriter.WriteEnum(ItemRepackagePacketMode.Open);
            pWriter.WriteLong(itemUid);
            return pWriter;
        }

        public static Packet Repackage(Item item)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_REPACKAGE);
            pWriter.WriteEnum(ItemRepackagePacketMode.Repackage);
            pWriter.WriteShort();
            pWriter.WriteLong(item.Uid);
            pWriter.WriteItem(item);
            return pWriter;
        }

        public static Packet Notice(int noticeId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_REPACKAGE);
            pWriter.WriteEnum(ItemRepackagePacketMode.Notice);
            pWriter.WriteByte();
            pWriter.WriteInt(noticeId);
            return pWriter;
        }
    }
}
