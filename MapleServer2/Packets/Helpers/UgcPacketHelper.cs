using MaplePacketLib2.Tools;

namespace MapleServer2.Packets.Helpers
{
    public static class UgcPacketHelper
    {
        public static PacketWriter WriteUgc(this PacketWriter pWriter)
        {
            pWriter.WriteLong();
            pWriter.WriteUnicodeString(""); // UUID (filename)
            pWriter.WriteUnicodeString(""); // Name (itemname)
            pWriter.WriteByte();
            pWriter.WriteInt();
            pWriter.WriteLong(); // AccountId
            pWriter.WriteLong(); // CharacterId
            pWriter.WriteUnicodeString(""); // CharacterName
            pWriter.WriteLong(); // CreationTime
            pWriter.WriteUnicodeString(""); // URL (no domain)
            pWriter.WriteByte();

            return pWriter;
        }
    }
}
