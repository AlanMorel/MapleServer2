using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class ChatStickerHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.STICKER;

        public ChatStickerHandler(ILogger<ChatStickerHandler> logger) : base(logger) { }

        private enum ChatStickerMode : byte
        {
            OpenWindow = 0x1,
            UseSticker = 0x3,
            Favorite = 0x5,
            Unfavorite = 0x6,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            ChatStickerMode mode = (ChatStickerMode) packet.ReadByte();

            switch (mode)
            {
                case ChatStickerMode.OpenWindow:
                    HandleOpenWindow(session, packet);
                    break;
                case ChatStickerMode.UseSticker:
                    HandleUseSticker(session, packet);
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

        private void HandleOpenWindow(GameSession session, PacketReader packet)
        {
            // TODO: if user has any expired stickers, use the packet below
            //session.Send(ChatStickerPacket.ExpiredStickerNotification());
        }

        private void HandleUseSticker(GameSession session, PacketReader packet)
        {
            int stickerId = packet.ReadInt();
            string script = packet.ReadUnicodeString();

            session.Send(ChatStickerPacket.UseSticker(stickerId, script));
        }

        private void HandleFavorite(GameSession session, PacketReader packet)
        {
            int stickerId = packet.ReadInt();

            session.Send(ChatStickerPacket.Favorite(stickerId));
        }

        private void HandleUnfavorite(GameSession session, PacketReader packet)
        {
            int stickerId = packet.ReadInt();

            session.Send(ChatStickerPacket.Unfavorite(stickerId));
        }
    }
}
