using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;
using MapleServer2.Servers.Game;

namespace MapleServer2.Packets
{
    public static class InsigniaPacket
    {
        public static Packet UpdateInsignia(GameSession session, short insigniaID)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.NAME_TAG_SYMBOL);
            pWriter.WriteInt(session.FieldPlayer.ObjectId);
            pWriter.WriteShort(insigniaID);
            pWriter.WriteByte(01); //01 = Display. 00 = Do not display.
            return pWriter;
        }
    }
}