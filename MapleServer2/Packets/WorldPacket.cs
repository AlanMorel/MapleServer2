using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class WorldPacket
{
    public static PacketWriter Send()
    {
        return PacketWriter.Of(SendOp.World);
    }
}
