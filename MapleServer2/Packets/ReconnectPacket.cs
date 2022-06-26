using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class ReconnectPacket
{
    public static PacketWriter Send()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Reconnect);

        pWriter.WriteInt(1);
        pWriter.WriteString();
        pWriter.WriteInt();

        return pWriter;
    }
}
