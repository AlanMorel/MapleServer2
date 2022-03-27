using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class FieldPropertyPacket
{
    private enum FieldPropertyMode : byte
    {
        ChangeGravity = 0x01
    }

    public static PacketWriter ChangeGravity(float value)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldProperty);
        pWriter.Write(FieldPropertyMode.ChangeGravity);
        pWriter.WriteByte(1);
        pWriter.WriteFloat(value);

        return pWriter;
    }
}
