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
            return PacketWriter.Of(SendOp.CHANGE_ATTRIBUTES)
                .WriteByte(0x01)
                .WriteLong(item.Uid)
                .WriteItem(item);
        }

        public static Packet SelectNewItem(Item item)
        {
            return PacketWriter.Of(SendOp.CHANGE_ATTRIBUTES)
                .WriteByte(0x02)
                .WriteLong(item.Uid)
                .WriteItem(item);
        }
    }
}
