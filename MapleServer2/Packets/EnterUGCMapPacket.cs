using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public class EnterUGCMapPacket
{
    private enum EnterUGCMapMode : byte
    {
        RequestPassword = 0x03,
        WrongPassword = 0x04
    }

    public static PacketWriter RequestPassword(long accountId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ENTER_UGC_MAP);
        pWriter.Write(EnterUGCMapMode.RequestPassword);
        pWriter.WriteInt();
        pWriter.WriteLong();
        pWriter.WriteLong(accountId);
        pWriter.WriteInt();
        pWriter.WriteByte(1);

        return pWriter;
    }

    public static PacketWriter WrongPassword(long accountId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ENTER_UGC_MAP);
        pWriter.Write(EnterUGCMapMode.WrongPassword);
        pWriter.WriteInt();
        pWriter.WriteLong();
        pWriter.WriteLong(accountId);

        return pWriter;
    }
}
