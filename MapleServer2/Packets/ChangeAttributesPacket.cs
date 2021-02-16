using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class ChangeAttributesPacket
    {
        public static Packet PreviewNewItem(Item item)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHANGE_ATTRIBUTES);
            pWriter.WriteByte(0x01);
            pWriter.WriteLong(item.Uid);
            pWriter.WriteItem(item);

            return pWriter;
        }

        public static Packet SelectNewItem(Item item)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHANGE_ATTRIBUTES);
            pWriter.WriteByte(0x02);
            pWriter.WriteLong(item.Uid);
            pWriter.WriteItem(item);

            return pWriter;
        }
    }
}
