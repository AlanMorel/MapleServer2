using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class TimeScalePacket
{
    public static PacketWriter SetTimeScale(bool enable, float startScale, float endScale, float duration, byte interpolator)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.TimeScale);
        pWriter.WriteBool(enable);
        pWriter.WriteFloat(startScale);
        pWriter.WriteFloat(endScale);
        pWriter.WriteFloat(duration);
        pWriter.WriteByte(interpolator);
        return pWriter;
    }
}
