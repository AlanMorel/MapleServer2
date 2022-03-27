using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class SetCameraPacket
{
    public static PacketWriter Set(float interpolationTime)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SetCamera);
        pWriter.WriteFloat(interpolationTime);
        return pWriter;
    }
}
