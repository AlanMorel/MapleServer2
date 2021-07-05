using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class HomeBank
    {
        public static Packet OpenBank(long date = 0)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.HOME_BANK);
            pWriter.WriteLong(date); //used for cooldown timer

            return pWriter;
        }
    }
}
