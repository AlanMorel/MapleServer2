using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class UserMoveByPortalPacket
    {
        public static Packet Move(int objectId, CoordF coords, CoordF rotation)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.USER_MOVE_BY_PORTAL);
            pWriter.WriteInt(objectId);
            pWriter.Write(coords);
            pWriter.Write(rotation);
            pWriter.WriteByte(); // Unknown

            return pWriter;
        }
    }
}
