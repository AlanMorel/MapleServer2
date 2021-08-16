using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class ItemPacket
    {
        public static Packet ItemData(Item item)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM);

            // Get item (would normally search database for item but since we don't have that just use passed in item)

            // Write item data
            pWriter.WriteLong(item.Uid);
            pWriter.WriteInt(1); // Unknown (amount?)
            pWriter.WriteInt(0); // Unknown
            pWriter.WriteInt(-1); // Unknown
            pWriter.WriteLong(item.CreationTime); // Item creation time
            pWriter.WriteZero(52); // Unknown 52 zero bytes
            pWriter.WriteInt(-1); // Unknown
            pWriter.WriteZero(102); // Unknown 102 zero bytes
            pWriter.WriteInt(1); // Unknown
            pWriter.WriteZero(14); // Unknown 14 zero bytes
            pWriter.WriteInt(14); // Unknown
            pWriter.WriteInt(1); // Unknown
            pWriter.WriteZero(6); // Unknown 6 zero bytes
            pWriter.WriteByte(1); // Unknown
            pWriter.WriteByte(1); // Unknown
            pWriter.WriteLong(item.OwnerCharacterId); // Item owner character id
            pWriter.WriteUnicodeString(item.OwnerCharacterName); // Item owner name
            pWriter.WriteZero(20); // Unknown 20 zero bytes

            return pWriter;
        }
    }
}
