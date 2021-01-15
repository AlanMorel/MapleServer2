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

            pWriter.WriteLong(); // Total amount of merets
            pWriter.WriteLong(session.Player.Wallet.Meret.Amount); // Meret
            pWriter.WriteLong(session.Player.Wallet.GameMeret.Amount); // GameMeret
            pWriter.WriteLong(session.Player.Wallet.EventMeret.Amount); // EventMeret
            pWriter.WriteLong(); // Message: 'You won {amount} merets', expects long

            return pWriter;
        }
    }
}
