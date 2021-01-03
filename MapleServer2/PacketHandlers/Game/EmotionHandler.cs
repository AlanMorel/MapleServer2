using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class EmotionHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.EMOTION;

        public EmotionHandler(ILogger<GamePacketHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte function = packet.ReadByte();
            packet.ReadInt(); // ??
            string emotion = packet.ReadUnicodeString();

            session.Send(PacketWriter.Of(SendOp.PROXY_GAME_OBJ)
                .WriteInt(session.FieldPlayer.ObjectId)
                .WriteByte(0x40) // Emote Type
                .WriteShort(11) // EmoteId (last 2 hex digits of actual id?)
            );
        }
    }
}
