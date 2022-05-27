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
        pWriter.WriteByte(1);
        pWriter.Write(FieldPropertyMode.ChangeGravity);
        pWriter.WriteFloat(value);

        return pWriter;
    }

    public static PacketWriter SetCharacterInvisible()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldProperty);
        pWriter.WriteByte(1);
        pWriter.WriteByte(3);

        return pWriter;
    }

    public static PacketWriter SetCharacterVisible()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldProperty);
        pWriter.WriteByte(2);
        pWriter.WriteByte(3);

        return pWriter;
    }
}
