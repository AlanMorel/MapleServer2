using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class DungeonWaitPacket
    {
        public static Packet Show(int dungeonId, int playerCountDisplay)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.DUNGEON_WAIT);
            pWriter.WriteInt(dungeonId);
            pWriter.WriteInt(playerCountDisplay);
            return pWriter;
        }
    }
}
