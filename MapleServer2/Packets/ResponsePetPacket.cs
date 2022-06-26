using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class ResponsePetPacket
{
    private enum ResponsePetMode : byte
    {
        Mode07 = 0x07,
        Mode0F = 0x0F,
    }

    public static PacketWriter Mode07()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ResponsePet);
        pWriter.Write(ResponsePetMode.Mode07);
        pWriter.WriteInt();

        return pWriter;
    }

    public static PacketWriter Mode0F()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ResponsePet);
        pWriter.Write(ResponsePetMode.Mode0F);
        pWriter.WriteByte(1);

        return pWriter;
    }
}
