using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class LocalCameraPacket
{
    public static PacketWriter Camera(int localCameraId, byte state, int objectId = 0)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.LOCAL_CAMERA);
        pWriter.WriteInt(localCameraId);
        pWriter.WriteByte(state); // 00 = reset, 01 = hide player, 02 = show player
        pWriter.WriteInt(objectId);
        return pWriter;
    }
}
