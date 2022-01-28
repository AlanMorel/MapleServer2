using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class ResponsePetPacket
{
    private enum ResponsePetMode : byte
    {
        Mode07 = 0x07,
    }

    public static PacketWriter Mode07()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_PET);
        pWriter.Write(ResponsePetMode.Mode07);
        pWriter.WriteInt();

        return pWriter;
    }
}
