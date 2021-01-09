using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;

namespace MapleServer2.Packets
{
    class WalletPacket
    {
        public static Packet UpdateWallet(CurrencyType type, long amount)
        {
            byte currency = 0;
            switch (type)
            {
                case CurrencyType.ValorToken:
                    currency = 3;
                    break;
                case CurrencyType.Treva:
                    currency = 4;
                    break;
                case CurrencyType.Rue:
                    currency = 5;
                    break;
                case CurrencyType.HaviFruit:
                    currency = 6;
                    break;
                default:
                    break;
            }

            PacketWriter pWriter = PacketWriter.Of(SendOp.MONEY_TOKEN);

            pWriter.WriteByte(currency); // type of currency
            pWriter.WriteLong(amount); // currency amount
            pWriter.Write("FF FF FF FF FF FF FF FF".ToByteArray()); // always the same
            pWriter.WriteShort(52); // always the same
            pWriter.WriteLong(); // unk
            pWriter.WriteShort(); // unk

            return pWriter;
        }
    }
}
