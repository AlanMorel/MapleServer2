using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class CharacterNameChangePacket
{
    public static PacketWriter NameResult(bool nameBeingUsed, string characterName, long itemUid)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CheckCharNameResult);
        pWriter.WriteBool(nameBeingUsed);
        pWriter.WriteLong(itemUid);
        pWriter.WriteUnicodeString(characterName);
        return pWriter;
    }
}
