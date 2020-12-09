using MaplePacketLib2.Tools;

namespace MapleServer2.Packets.Helpers
{
    public static class UgcPacketHelper
    {
        public static PacketWriter WriteUgc(this PacketWriter pWriter)
        {
            return pWriter.WriteLong()
                .WriteUnicodeString("") // UUID (filename)
                .WriteUnicodeString("") // Name (itemname)
                .WriteByte()
                .WriteInt()
                .WriteLong() // AccountId
                .WriteLong() // CharacterId
                .WriteUnicodeString("") // CharacterName
                .WriteLong() // CreationTime
                .WriteUnicodeString("") // URL (no domain)
                .WriteByte();
        }
    }
}