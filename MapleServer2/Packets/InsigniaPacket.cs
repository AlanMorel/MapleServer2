using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;

namespace MapleServer2.Packets
{
    public static class InsigniaPacket
    {
        public static PacketWriter UpdateInsignia(GameSession session, short insigniaId, bool show)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.NAME_TAG_SYMBOL);
            pWriter.WriteInt(session.FieldPlayer.ObjectId);
            pWriter.WriteShort(insigniaId);
            pWriter.WriteBool(show);
            return pWriter;
        }
    }
}
