using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class RevivalCountPacket
{
    public static PacketWriter Send(int count)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RevivalCount);

        pWriter.WriteInt(count);

        return pWriter;
    }
}
