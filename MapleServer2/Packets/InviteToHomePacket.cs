using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class InviteToHomePacket
{
    public static PacketWriter InviteToHome(Player sourcePlayer)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.InviteToHome);
        pWriter.WriteByte(2);
        pWriter.WriteLong(sourcePlayer.AccountId);
        pWriter.WriteUnicodeString(sourcePlayer.Name);

        return pWriter;
    }
}
