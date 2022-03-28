using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class DungeonListPacket
{
    public static PacketWriter DungeonList()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.DungeonList);
        pWriter.WriteByte();
        pWriter.WriteInt(1); // dungeon count
        for (int i = 0; i < 1; i++)
        {
            pWriter.WriteInt(25001001); // dungeon id
            pWriter.WriteBool(true); // Eligibility: false = rookie, true = veteran, if id isn't sent = "insufficient level"
        }

        return pWriter;
    }
}
