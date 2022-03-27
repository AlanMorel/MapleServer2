using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class ChatPacket
{
    public static PacketWriter Send(Player player, string message, ChatType type, long clubId = 0)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UserChat);
        pWriter.WriteLong(player.AccountId);
        pWriter.WriteLong(player.CharacterId);
        pWriter.WriteUnicodeString(player.Name);
        pWriter.WriteByte();
        pWriter.WriteUnicodeString(message);
        pWriter.WriteInt((int) type);
        pWriter.WriteByte();
        pWriter.WriteInt(player.ChannelId);

        switch (type)
        {
            case ChatType.WhisperFrom:
                pWriter.WriteUnicodeString("???");
                break;
            case ChatType.Super:
                pWriter.WriteInt(player.SuperChat);
                break;
            case ChatType.Club:
                pWriter.WriteLong(clubId);
                break;
        }

        pWriter.WriteByte();
        return pWriter;
    }

    public static PacketWriter Error(Player player, SystemNotice error, ChatType type)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UserChat);
        pWriter.WriteLong();
        pWriter.WriteLong();
        pWriter.WriteUnicodeString(player.Name);
        pWriter.WriteByte(1);
        pWriter.WriteInt((int) error);
        pWriter.WriteInt((int) type);
        pWriter.WriteByte();
        pWriter.WriteInt(); // Channel
        pWriter.WriteByte();
        return pWriter;
    }

}
// SendUserChatItem
// 01 00 00 00 C5 1C FE 04 AD 97 CB 27 74 68 B0 00 05 00 00 00 01 00 00 00 00 00 00 00 FF FF FF FF ED 29 10 5E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 09 00 00 00 00 00 00 00 01 A2 8C 4B 5E 00 00 00 00 01 00 00 00 00 00 00 3F 53 9E FF 16 2E 7C FF 13 1F 44 FF 0A 00 00 00 05 00 00 00 00 02 00 01 00 32 00 00 00 00 00 00 00 14 00 55 11 00 00 00 00 00 00 00 00 00 00 00 00 02 00 14 00 83 02 00 00 00 00 00 00 04 00 49 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 0C 00 80 6A 3C 3D 00 00 00 00 0D 00 00 00 00 00 00 00 80 40 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0F 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 14 00 00 00 08 00 00 00 01 00 00 00 00 01 14 00 00 00 00 00 00 00 00 00 00 40 00 00 00 00 00 00 00 00 00 00 00 00 18 00 00 00 01 00 00 00 00 00 00 00 00 00 01 01 6C 00 3C E8 87 12 43 26 05 00 4C 00 69 00 7A 00 7A 00 75 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
// Item Enchant Msg
// 3,2867552357120351429,1,Lizzu
