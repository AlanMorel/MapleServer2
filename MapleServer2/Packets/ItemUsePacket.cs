using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;

namespace MapleServer2.Packets
{
    public static class ItemUsePacket
    {
        public static Packet Use(int id, int amount)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_USE);

            pWriter.WriteInt(id);
            pWriter.WriteInt(amount);
            pWriter.WriteShort(2); // Unknown

            return pWriter;
        }
    }
}