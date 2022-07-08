using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class NewsNotificationPacket
{
    private enum Mode : byte
    {
        OpenBrowser = 0x0,
        OpenSidebar = 0x2
    }

    public static PacketWriter OpenBrowser()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.NewsNotification);
        pWriter.WriteByte();
        pWriter.WriteUnicodeString("86BFAEA2-DC42-4AEA-ADD5-D234E8810E08"); // random key to display banners
        pWriter.WriteByte(0x1);
        pWriter.Write(Mode.OpenBrowser);
        pWriter.WriteInt();
        return pWriter;
    }

    public static PacketWriter OpenSidebar()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.NewsNotification);
        pWriter.WriteByte();
        pWriter.WriteUnicodeString("86BFAEA2-DC42-4AEA-ADD5-D234E8810E08"); // random key to display banners
        pWriter.WriteByte(0x1);
        pWriter.Write(Mode.OpenSidebar);
        pWriter.WriteInt();
        return pWriter;
    }
}
