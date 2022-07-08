using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class EnterUgcMapPacket
{
    private enum Mode : byte
    {
        RequestPassword = 0x03,
        WrongPassword = 0x04
    }

    public static PacketWriter RequestPassword(long accountId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.EnterUGCMap);
        pWriter.Write(Mode.RequestPassword);
        pWriter.WriteInt();
        pWriter.WriteLong();
        pWriter.WriteLong(accountId);
        pWriter.WriteInt();
        pWriter.WriteByte(1);

        return pWriter;
    }

    public static PacketWriter WrongPassword(long accountId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.EnterUGCMap);
        pWriter.Write(Mode.WrongPassword);
        pWriter.WriteInt();
        pWriter.WriteLong();
        pWriter.WriteLong(accountId);

        return pWriter;
    }
}
