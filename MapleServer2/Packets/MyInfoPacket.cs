using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class MyInfoPacket
    {
        public static PacketWriter SetMotto(IFieldObject<Player> pObject, string motto)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MY_INFO);
            pWriter.WriteInt(pObject.ObjectId);
            pWriter.WriteUnicodeString(motto);
            pWriter.WriteInt(); //Unk
            pWriter.WriteShort(); //Unk

            return pWriter;
        }
    }
}
