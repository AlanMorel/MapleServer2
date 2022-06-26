using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class BypassKeyPacket
{
    public static PacketWriter Send()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.BypassKey);
        pWriter.WriteInt();
        pWriter.WriteLong();

        return pWriter;
    }
}
