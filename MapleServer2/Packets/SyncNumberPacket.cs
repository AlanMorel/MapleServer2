using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class SyncNumberPacket
    {
        public static Packet Send()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SYNC_NUMBER);
            pWriter.WriteByte();
            return pWriter;
        }
    }
}
