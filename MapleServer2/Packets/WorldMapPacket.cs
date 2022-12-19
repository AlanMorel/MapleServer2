using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class WorldMapPacket
{
    public static PacketWriter Open()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.WorldMap);

        // Expect 4 bytes
        // 0X 00 00 00 breaks map
        // 00 01 00 00 breaks map => wants 1 int
        // 00 00 0X 00 breaks map => wants 1 more byte
        // 00 00 00 0X breaks map => wants 1 more byte
        // seems like map data seems to be sent elsewhere, perhaps FIELD_ADD_USER

        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteByte();

        return pWriter;
    }
}
