using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class CashPacket
{
    private enum CashMode : byte
    {
        Mode09 = 0x09
    }

    public static PacketWriter Unknown09()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Cash);
        pWriter.Write(CashMode.Mode09);
        pWriter.WriteByte();

        return pWriter;
    }
}
