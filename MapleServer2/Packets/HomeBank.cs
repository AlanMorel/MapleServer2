using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class HomeBank
    {
        public static Packet OpenBank()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.HOME_BANK);
            pWriter.WriteInt(); // some id
            pWriter.WriteInt(); // 0

            return pWriter;
        }
    }
}
