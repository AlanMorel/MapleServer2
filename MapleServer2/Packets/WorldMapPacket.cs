using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    class WorldMapPacket
    {
        public static Packet Open(Player player)
        {
            var pWriter = PacketWriter.Of(SendOp.WORLD_MAP);

            // Expect 4 bytes
            // 0X 00 00 00 breaks map
            // 00 01 00 00 breaks map => wants 1 int
            // 00 00 0X 00 breaks map => wants 1 more byte
            // 00 00 00 0X breaks map => wants 1 more byte
            // seems like map data seems to be sent elsewhere, perhaps FIELD_ADD_USER

            pWriter.WriteByte(0);
            pWriter.WriteByte(0);
            pWriter.WriteByte(0);
            pWriter.WriteByte(0);

            return pWriter;
        }
    }
}
