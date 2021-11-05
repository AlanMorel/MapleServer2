using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class OneTimeEffectPacket
{
    public static PacketWriter View(int id, bool enable, string path = "")
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ONE_TIME_EFFECT);
        pWriter.WriteInt(id);
        pWriter.WriteBool(enable);
        if (enable)
        {
            pWriter.WriteUnicodeString(path);
        }
        return pWriter;
    }
}
