using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class ChatPacket
    {
        public static Packet Send(Player player, string message, ChatType type)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.USER_CHAT)
                .WriteLong(player.AccountId)
                .WriteLong(player.CharacterId)
                .WriteUnicodeString(player.Name)
                .WriteByte()
                .WriteUnicodeString(message)
                .WriteInt((int) type)
                .WriteByte()
                .WriteInt(); // Channel

            switch (type)
            {
                case ChatType.WhisperFrom:
                    pWriter.WriteUnicodeString("???");
                    break;
                case ChatType.Super:
                    pWriter.WriteInt(20800017); // item id?
                    break;
                case ChatType.UnknownPurple:
                    pWriter.WriteLong(); // char id?
                    break;
            }

            return pWriter.WriteByte();
        }
    }
}
// SendUserChatItem
// 01 00 00 00 C5 1C FE 04 AD 97 CB 27 74 68 B0 00 05 00 00 00 01 00 00 00 00 00 00 00 FF FF FF FF ED 29 10 5E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 09 00 00 00 00 00 00 00 01 A2 8C 4B 5E 00 00 00 00 01 00 00 00 00 00 00 3F 53 9E FF 16 2E 7C FF 13 1F 44 FF 0A 00 00 00 05 00 00 00 00 02 00 01 00 32 00 00 00 00 00 00 00 14 00 55 11 00 00 00 00 00 00 00 00 00 00 00 00 02 00 14 00 83 02 00 00 00 00 00 00 04 00 49 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 0C 00 80 6A 3C 3D 00 00 00 00 0D 00 00 00 00 00 00 00 80 40 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0F 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 14 00 00 00 08 00 00 00 01 00 00 00 00 01 14 00 00 00 00 00 00 00 00 00 00 40 00 00 00 00 00 00 00 00 00 00 00 00 18 00 00 00 01 00 00 00 00 00 00 00 00 00 01 01 6C 00 3C E8 87 12 43 26 05 00 4C 00 69 00 7A 00 7A 00 75 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
// Item Enchant Msg
// 3,2867552357120351429,1,Lizzu
