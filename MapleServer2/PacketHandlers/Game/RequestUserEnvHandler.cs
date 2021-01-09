using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class RequestUserEnvHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_USER_ENV;

        public RequestUserEnvHandler(ILogger<RequestUserEnvHandler> logger) : base(logger) { }

        private enum TitleMode : byte
        {
            Unk = 0x0,
            Change = 0x1,
            Trophies = 0x3,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            TitleMode mode = (TitleMode) packet.ReadByte();

            switch (mode)
            {
                case TitleMode.Unk:
                    break;
                case TitleMode.Change:
                    HandleTitleChange(session, packet);
                    break;
                case TitleMode.Trophies:
                    //Load trophies
                    break;
            }
        }

        private static void HandleTitleChange(GameSession session, PacketReader packet)
        {
            int titleID = packet.ReadInt();

            if (titleID < 0)
            {
                return;
            }

            session.Player.TitleId = titleID;
            session.FieldManager.BroadcastPacket(PlayerTitlePacket.UpdatePlayerTitle(session, titleID));
        }
    }
}
