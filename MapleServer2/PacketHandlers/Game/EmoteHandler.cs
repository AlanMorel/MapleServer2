using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class EmoteHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.EMOTION;

        public EmoteHandler(ILogger<EmoteHandler> logger) : base(logger) { }

        private enum EmoteMode : byte
        {
            LearnEmote = 0x1,
            UseEmote = 0x2,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            EmoteMode mode = (EmoteMode) packet.ReadByte();

            switch (mode)
            {
                case EmoteMode.LearnEmote:
                    HandleLearnEmote(session, packet);
                    break;
                case EmoteMode.UseEmote:
                    HandleUseEmote(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private void HandleLearnEmote(GameSession session, PacketReader packet)
        {
            long emoteItemUid = packet.ReadLong();
            // TODO grab emoteId from emoteItemUid
            session.Send(EmotePacket.LearnEmote());
        }

        private void HandleUseEmote(GameSession session, PacketReader packet)
        {
            int emoteId = packet.ReadInt();
            string animationName = packet.ReadUnicodeString();
            // animationName is the name in /Xml/anikeytext.xml
        }
    }
}
