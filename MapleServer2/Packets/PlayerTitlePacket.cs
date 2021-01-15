using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;

namespace MapleServer2.Packets
{
    public static class PlayerTitlePacket
    {
        public static Packet UpdatePlayerTitle(GameSession session, int titleId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.USER_ENV);
            pWriter.WriteByte(01); // Mode update
            pWriter.WriteInt(session.FieldPlayer.ObjectId);
            pWriter.WriteInt(titleId);
            return pWriter;
        }
    }
}
