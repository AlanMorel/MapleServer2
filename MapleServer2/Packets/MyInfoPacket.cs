using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class MyInfoPacket
    {
        public static Packet SetMotto(IFieldObject<Player> pObject, string motto)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MY_INFO)
                .WriteInt(pObject.ObjectId)
                .WriteUnicodeString(motto)
                .WriteInt() //Unk
                .WriteShort(); //Unk
            return pWriter;
        }

    }
}
