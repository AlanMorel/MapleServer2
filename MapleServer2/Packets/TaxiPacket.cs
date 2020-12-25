using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;
using System;

namespace MapleServer2.Packets
{
    public static class TaxiPacket
    {
        public static Packet DiscoverTaxi(int mapId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TAXI)
                .WriteInt(1)
                .WriteInt(mapId)
                .WriteByte(1);
            return pWriter;
        }
    }
}
