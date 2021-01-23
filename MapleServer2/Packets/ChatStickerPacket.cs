using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{

    public static class ChatStickerPacket
    {
        private enum ChatStickerMode : byte
        {
            LoadChatSticker = 0x0,
            ExpiredStickerNotification = 0x1,
            AddSticker = 0x2,
            UseSticker = 0x3,
            Favorite = 0x5,
            Unfavorite = 0x6,
        }

        public static Packet LoadChatSticker(Player player)
        {
            List<short> stickerSetIds = player.Stickers;

            PacketWriter pWriter = PacketWriter.Of(SendOp.CHAT_STICKER);
            pWriter.WriteEnum(ChatStickerMode.LoadChatSticker);
            pWriter.WriteShort();
            pWriter.WriteShort((short) stickerSetIds.Count);
            foreach (int sticker in stickerSetIds)
            {
                pWriter.WriteInt(sticker);
                pWriter.WriteLong(9223372036854775807); //expiration timestamp
            }
            return pWriter;
        }

        public static Packet ExpiredStickerNotification()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHAT_STICKER);
            pWriter.WriteEnum(ChatStickerMode.ExpiredStickerNotification);
            pWriter.WriteInt();
            pWriter.WriteInt(1);
            return pWriter;
        }

        public static Packet AddSticker(int itemId, int stickerGroupId, long expiration)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHAT_STICKER);
            pWriter.WriteEnum(ChatStickerMode.AddSticker);
            pWriter.WriteInt(itemId);
            pWriter.WriteInt(1);
            pWriter.WriteInt(stickerGroupId);
            pWriter.WriteLong(expiration);
            return pWriter;
        }

        public static Packet UseSticker(int stickerId, string script)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHAT_STICKER);
            pWriter.WriteEnum(ChatStickerMode.UseSticker);
            pWriter.WriteInt(stickerId);
            pWriter.WriteUnicodeString(script);
            pWriter.WriteByte();
            return pWriter;
        }

        public static Packet Favorite(int stickerId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHAT_STICKER);
            pWriter.WriteEnum(ChatStickerMode.Favorite);
            pWriter.WriteInt(stickerId);
            return pWriter;
        }

        public static Packet Unfavorite(int stickerId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHAT_STICKER);
            pWriter.WriteEnum(ChatStickerMode.Unfavorite);
            pWriter.WriteInt(stickerId);
            return pWriter;
        }
    }
}
