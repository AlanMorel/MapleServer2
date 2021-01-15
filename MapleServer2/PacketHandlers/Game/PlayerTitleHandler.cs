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

            switch (function)
            {
                case 0:
                    int playerTitleID = packet.ReadInt();
                    if (playerTitleID < 0)
                    {
                        return;
                    }
                    session.FieldManager.BroadcastPacket(PlayerTitlePacket.UpdatePlayerTitle(session, playerTitleID));
                    break;
                case 3:
                    //Load trophies
                    break;
            }
        }
    }
}
