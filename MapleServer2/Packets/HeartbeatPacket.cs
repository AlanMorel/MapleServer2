using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class HeartbeatPacket
{
    public static PacketWriter Request()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RequestHeartBeat);
        pWriter.WriteInt(Environment.TickCount);
        return pWriter;
    }
}
