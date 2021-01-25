using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;

namespace MapleServer2.Packets
{
    public static class UserMoveByPortalPacket
    {

        public static Packet Move(GameSession session, CoordF lastCoord)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.USER_MOVE_BY_PORTAL);
            lastCoord.Z += 10; // Without this player will spawn inside the block
            // for some reason if coord is negative player is teleported one block over, which can result player being stuck inside a block
            if (session.FieldPlayer.Coord.Y < 0)
            {
                lastCoord.Y -= 150;
            }
            if (session.FieldPlayer.Coord.X < 0)
            {
                lastCoord.X -= 150;
            }
            pWriter.WriteInt(session.FieldPlayer.ObjectId);
            pWriter.Write(lastCoord);
            pWriter.WriteInt(); // Unknown
            pWriter.WriteInt(); // Unknown
            pWriter.WriteInt(); // Unknown
            pWriter.WriteByte(); // Unknown

            return pWriter;
        }
    }
}
