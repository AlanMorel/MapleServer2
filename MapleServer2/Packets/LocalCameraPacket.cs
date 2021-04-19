using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class LocalCameraPacket
    {
        public static Packet Camera(int localCameraId, byte state, Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.LOCAL_CAMERA);
            pWriter.WriteInt(localCameraId);
            pWriter.WriteByte(state); // 00 = reset, 01 = hide player, 02 = show player
            pWriter.WriteInt(player.Session.FieldPlayer.ObjectId);
            return pWriter;
        }
    }
}
