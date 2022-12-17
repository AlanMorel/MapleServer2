using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class InsigniaPacket
{
    public static PacketWriter UpdateInsignia(int playerObjectId, short insigniaId, bool show)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.NameTagSymbol);
        pWriter.WriteInt(playerObjectId);
        pWriter.WriteShort(insigniaId);
        pWriter.WriteBool(show);
        return pWriter;
    }
}
