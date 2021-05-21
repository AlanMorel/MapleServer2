using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public class LoginPacket
    {
        public static PacketWriter LoginRequired(long accountId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.LOGIN_REQUIRED);
            pWriter.WriteByte(0x17);
            pWriter.WriteLong(accountId);
            pWriter.WriteInt();
            pWriter.WriteByte();
            pWriter.WriteLong();
            pWriter.WriteInt(1);
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteLong();

            return pWriter;
        }
    }
}
