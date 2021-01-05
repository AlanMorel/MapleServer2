using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class BannerListPacket
    {
        public static Packet SetBanner()
        {
            short count = 0; // TODO: Load banners

            PacketWriter pWriter = PacketWriter.Of(SendOp.BANNER_LIST);
            pWriter.WriteShort(count);
            for (int i = 0; i < count; i++)
            {
                pWriter.WriteInt(); // Id
                pWriter.WriteUnicodeString("name"); // Name
                pWriter.WriteUnicodeString("merat"); // Type
                pWriter.WriteUnicodeString(""); // Unknown
                pWriter.WriteUnicodeString(""); // Unknown
                pWriter.WriteUnicodeString("url"); // Url
                pWriter.WriteInt(); // Language
                pWriter.WriteLong(); // Timestamp
                pWriter.WriteLong(); // Unknown
            }

            return pWriter;
        }
    }
}
