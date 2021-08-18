using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    internal class MeretsPacket
    {
        public static Packet UpdateMerets(Account account, long amountGain = 0)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MERET);

            pWriter.WriteLong(); // Total amount of merets
            pWriter.WriteLong(account.Meret.Amount);
            pWriter.WriteLong(account.GameMeret.Amount);
            pWriter.WriteLong(account.EventMeret.Amount);
            pWriter.WriteLong(amountGain); // Message: 'You won {amount} merets'

            return pWriter;
        }
    }
}
