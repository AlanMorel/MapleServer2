using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public class HomeCommandPacket
    {
        public static Packet HomeCommand(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.HOME_COMMAND);
            pWriter.WriteByte();
            pWriter.WriteLong(player.AccountId); // acc id
            pWriter.WriteLong(); // timestamp

            return pWriter;
        }
    }
}
