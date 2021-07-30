using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public class InviteToHomePacket
    {
        public static Packet InviteToHome(Player sourcePlayer)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INVITE_TO_HOME);
            pWriter.WriteByte(2);
            pWriter.WriteLong(sourcePlayer.AccountId);
            pWriter.WriteUnicodeString(sourcePlayer.Name);

            return pWriter;
        }
    }
}
