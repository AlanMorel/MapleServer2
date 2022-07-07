using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public class BirthdayPacket
{
    private enum Mode : byte
    {
        SetBirthday = 0x1
    }

    public static PacketWriter SetBirthday(int playerObjectId, long birthdate)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Birthday);
        pWriter.Write(Mode.SetBirthday);
        pWriter.WriteInt(playerObjectId);
        pWriter.WriteLong(birthdate);
        return pWriter;
    }
}
