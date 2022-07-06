using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class AttendancePacket
{
    private enum Mode : byte
    {
        Notice = 0x9
    }

    public static PacketWriter Notice(byte noticeId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Attendance);
        pWriter.Write(Mode.Notice);
        pWriter.WriteByte(noticeId);
        return pWriter;
    }
}
