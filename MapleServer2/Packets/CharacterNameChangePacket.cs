using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class CharacterNameChangePacket
    {
        public static Packet NameResult(bool nameBeingUsed, string characterName, long itemUid)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHECK_CHAR_NAME_RESULT);
            pWriter.WriteBool(nameBeingUsed);
            pWriter.WriteLong(itemUid);
            pWriter.WriteUnicodeString(characterName);
            return pWriter;
        }
    }
}
