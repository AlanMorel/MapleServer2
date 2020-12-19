using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class MyInfoPacket
    {
        public static Packet SetMotto(Player p, string motto)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MY_INFO)
                .WriteLong(p.CharacterId)
                .WriteUnicodeString(motto)
                .WriteByte(1);

            return pWriter;
        }

    }
}