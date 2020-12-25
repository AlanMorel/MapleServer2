using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class PlayerTitleHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_USER_ENV;

        public PlayerTitleHandler(ILogger<PlayerTitleHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte function = packet.ReadByte();
            int titleId = packet.ReadInt();

            if (titleId < 0) {
                return;
            }

            session.Player.TitleId = titleId;
            session.FieldManager.BroadcastPacket(PlayerTitlePacket.UpdatePlayerTitle(session, titleId));
        }
    }
}