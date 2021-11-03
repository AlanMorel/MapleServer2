using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class WebOpenPacket
    {
        public static PacketWriter Open(string url)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.WEB_OPEN);
            pWriter.WriteByte(1);
            pWriter.WriteUnicodeString(url);
            return pWriter;
        }
    }
}
