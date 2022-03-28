using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class NpsInfoPacket
{
    public static PacketWriter SendUsername(string username)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.NpsInfo);
        pWriter.WriteLong();
        pWriter.WriteUnicodeString(username);

        return pWriter;
    }
}
