using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;

namespace MapleServer2.Packets
{
    class FallDamagePacket
    {
        public static Packet FallDamage(GameSession session, int hpLost)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.STATE_FALL_DAMAGE);
            pWriter.WriteInt(session.FieldPlayer.ObjectId);
            pWriter.WriteInt(hpLost);

            return pWriter;
        }
    }
}
