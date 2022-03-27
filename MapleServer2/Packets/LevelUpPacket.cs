using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class LevelUpPacket
{
    public static PacketWriter LevelUp(int playerObjectId, short level)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.LevelUp);

        pWriter.WriteInt(playerObjectId);
        pWriter.WriteShort(level);

        return pWriter;
    }
}
