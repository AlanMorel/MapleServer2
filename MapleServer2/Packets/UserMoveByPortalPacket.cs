using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class UserMoveByPortalPacket
    {
        public static PacketWriter Move(IFieldObject<Player> fieldPlayer, CoordF coords, CoordF rotation)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.USER_MOVE_BY_PORTAL);
            pWriter.WriteInt(fieldPlayer.ObjectId);
            pWriter.Write(coords);
            pWriter.Write(rotation);
            pWriter.WriteByte(); // Unknown

            return pWriter;
        }
    }
}
