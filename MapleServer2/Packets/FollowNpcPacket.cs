using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class FollowNpcPacket
{
    public static PacketWriter FollowNpc(int objectId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FollowNPC);
        pWriter.WriteInt(objectId);

        return pWriter;
    }
}
