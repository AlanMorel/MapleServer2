using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class SystemShopPacket
{
    private enum Mode : byte
    {
        Open = 0x0A
    }

    public static PacketWriter Open()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SystemShop);
        pWriter.Write(Mode.Open);
        pWriter.WriteByte(0x1);
        return pWriter;
    }
}
