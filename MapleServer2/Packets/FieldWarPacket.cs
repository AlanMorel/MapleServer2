using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class FieldWarPacket
{
    private enum Mode : byte
    {
        LegionPopup = 0x0
    }

    public static PacketWriter LegionPopup(int fieldWarId, long epochTime)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldWar);
        pWriter.Write(Mode.LegionPopup);
        pWriter.WriteInt(fieldWarId);
        pWriter.WriteLong(epochTime);
        return pWriter;
    }
}
