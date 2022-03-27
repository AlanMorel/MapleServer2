using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class MoveResultPacket
{
    public static PacketWriter SendStatus(byte status)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MoveResult);
        pWriter.WriteByte(status); // 0 = success, others = different error messages

        return pWriter;
    }
}
