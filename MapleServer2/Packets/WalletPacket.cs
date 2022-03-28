using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

internal class WalletPacket
{
    public static PacketWriter UpdateWallet(CurrencyType type, long amount)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MesoToken);

        pWriter.WriteByte((byte) type); // type of currency
        pWriter.WriteLong(amount); // currency amount
        pWriter.WriteLong(-1); // always the same
        pWriter.WriteInt(52); // always the same
        pWriter.WriteLong(); // unk

        return pWriter;
    }
}
