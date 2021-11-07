using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game;

public class ChatStickerHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.CHAT_STICKER;

    public ChatStickerHandler() : base() { }

    private enum ChatStickerMode : byte
    {
        OpenWindow = 0x1,
        UseSticker = 0x3,
        GroupChatSticker = 0x4,
        Favorite = 0x5,
        Unfavorite = 0x6
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        ChatStickerMode mode = (ChatStickerMode) packet.ReadByte();

        switch (mode)
        {
            case ChatStickerMode.OpenWindow:
                HandleOpenWindow( /*session, packet*/);
                break;
            case ChatStickerMode.UseSticker:
                HandleUseSticker(session, packet);
                break;
            case ChatStickerMode.GroupChatSticker:
                HandleGroupChatSticker(session, packet);
                break;
            case ChatStickerMode.Favorite:
                HandleFavorite(session, packet);
                break;
            case ChatStickerMode.Unfavorite:
                HandleUnfavorite(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleOpenWindow( /*GameSession session, ByteReader packet*/)
    {
        // TODO: if user has any expired stickers, use the packet below
        //session.Send(ChatStickerPacket.ExpiredStickerNotification());
    }

    private static void HandleUseSticker(GameSession session, PacketReader packet)
    {
        int stickerId = packet.ReadInt();
        string script = packet.ReadUnicodeString();

        byte groupId = ChatStickerMetadataStorage.GetGroupId(stickerId);

        if (!session.Player.ChatSticker.Any(p => p.GroupId == groupId))
        {
            return;
        }

        session.Send(ChatStickerPacket.UseSticker(stickerId, script));
    }

    private static void HandleGroupChatSticker(GameSession session, PacketReader packet)
    {
        int stickerId = packet.ReadInt();
        string groupChatName = packet.ReadUnicodeString();

        byte groupId = ChatStickerMetadataStorage.GetGroupId(stickerId);

        if (!session.Player.ChatSticker.Any(p => p.GroupId == groupId))
        {
            return;
        }

        session.Send(ChatStickerPacket.GroupChatSticker(stickerId, groupChatName));
    }

    private static void HandleFavorite(GameSession session, PacketReader packet)
    {
        int stickerId = packet.ReadInt();

        if (session.Player.FavoriteStickers.Contains(stickerId))
        {
            return;
        }
        session.Player.FavoriteStickers.Add(stickerId);
        session.Send(ChatStickerPacket.Favorite(stickerId));
    }

    private static void HandleUnfavorite(GameSession session, PacketReader packet)
    {
        int stickerId = packet.ReadInt();

        if (!session.Player.FavoriteStickers.Contains(stickerId))
        {
            return;
        }
        session.Player.FavoriteStickers.Remove(stickerId);
        session.Send(ChatStickerPacket.Unfavorite(stickerId));
    }
}
