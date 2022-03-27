using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class UserMoveByPortalPacket
{
    public static PacketWriter Move(IFieldObject<Player> fieldPlayer, CoordF coords, CoordF rotation, bool isTrigger = false)
    {
        coords.Z += 25; // make sure coord is above ground

        PacketWriter pWriter = PacketWriter.Of(SendOp.UserMoveByPortal);
        pWriter.WriteInt(fieldPlayer.ObjectId);
        pWriter.Write(coords);
        pWriter.Write(rotation);
        pWriter.WriteBool(isTrigger);

        return pWriter;
    }
}
