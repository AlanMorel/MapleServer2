using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class NewsNotificationPacket
    {
        private enum NewsNotificationPacketMode : byte
        {
            OpenBrowser = 0x0,
            OpenSidebar = 0x2,
        }

        public static Packet OpenBrowser()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.NEWS_NOTIFICATION);
            pWriter.WriteByte(0x0);
            pWriter.WriteUnicodeString("86BFAEA2-DC42-4AEA-ADD5-D234E8810E08"); // random key to display banners
            pWriter.WriteByte(0x1);
            pWriter.WriteEnum(NewsNotificationPacketMode.OpenBrowser);
            pWriter.WriteInt(0x0);
            return pWriter;
        }

        public static Packet OpenSidebar()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.NEWS_NOTIFICATION);
            pWriter.WriteByte(0x0);
            pWriter.WriteUnicodeString("86BFAEA2-DC42-4AEA-ADD5-D234E8810E08"); // random key to display banners
            pWriter.WriteByte(0x1);
            pWriter.WriteEnum(NewsNotificationPacketMode.OpenSidebar);
            pWriter.WriteInt(0x0);
            return pWriter;
        }
    }
}
