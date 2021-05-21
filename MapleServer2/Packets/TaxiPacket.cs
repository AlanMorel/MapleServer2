using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class TaxiPacket
    {
        public static Packet DiscoverTaxi(int mapId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TAXI);
            pWriter.WriteInt(1);
            pWriter.WriteInt(mapId);
            pWriter.WriteByte(1);

            return pWriter;
        }
    }
}
