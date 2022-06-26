using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Managers.Actors;

namespace MapleServer2.Packets;

public static class UserStatePacket
{
    public static PacketWriter Send(Character character)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UserState);
        pWriter.WriteInt(character.ObjectId);
        pWriter.WriteByte(5);

        return pWriter;
    }
}
