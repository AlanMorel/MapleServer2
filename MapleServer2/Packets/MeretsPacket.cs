using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;

namespace MapleServer2.Packets
{
    class MeretsPacket
    {
        public static Packet UpdateMerets(GameSession session)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MERET);

            pWriter.WriteLong(session.Player.GetCurrency(Enums.CurrencyType.Meret)); // Total amount of merets
            pWriter.WriteLong(); // expects another long
            pWriter.WriteLong(); // expects another long
            pWriter.WriteLong(); // expects another long
            pWriter.WriteLong(); // expects another long

            return pWriter;
        }
    }
}
