using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class ResponseCharCreatePacket
{
    public static PacketWriter NameTaken()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CHARACTER_CREATE);
        pWriter.WriteByte(0x0B); // Name is taken
        pWriter.WriteShort(); // idk

        return pWriter;
    }
}
