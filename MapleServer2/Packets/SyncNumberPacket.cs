using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class SyncNumberPacket
    {
        public static PacketWriter Send()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SYNC_NUMBER);
            pWriter.WriteByte();
            return pWriter;
        }
    }
}
