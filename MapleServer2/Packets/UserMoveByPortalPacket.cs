using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;

namespace MapleServer2.Packets
{
    public static class UserMoveByPortalPacket
    {
        public static Packet Move(GameSession session, CoordF coords, CoordF rotation)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.USER_MOVE_BY_PORTAL);
            pWriter.WriteInt(session.FieldPlayer.ObjectId);
            pWriter.Write(coords);
            pWriter.Write(rotation);
            pWriter.WriteByte(); // Unknown
            return pWriter;
        }
    }
}
