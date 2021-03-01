using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    class WalletPacket
    {
        public static Packet UpdateWallet(CurrencyType type, long amount)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MONEY_TOKEN);

            pWriter.WriteByte((byte) type); // type of currency
            pWriter.WriteLong(amount); // currency amount
            pWriter.WriteLong(-1); // always the same
            pWriter.WriteShort(52); // always the same
            pWriter.WriteLong(); // unk
            pWriter.WriteShort(); // unk

            return pWriter;
        }
    }
}
