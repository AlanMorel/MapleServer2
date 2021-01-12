using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;

namespace MapleServer2.Packets
{
    class MesosPacket
    {
        public static Packet UpdateMesos(GameSession session)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MONEY);

            pWriter.WriteLong(session.Player.Wallet.Meso.Amount); // Total amount of mesos
            pWriter.WriteInt(); // unknown int

            return pWriter;
        }
    }
}
