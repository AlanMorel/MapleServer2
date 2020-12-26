using System.Net;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Servers.Login;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game {
    public class QuitHandler : GamePacketHandler {
        public override RecvOp OpCode => RecvOp.REQUEST_QUIT;

        private readonly IPEndPoint loginEndpoint;

        public QuitHandler(ILogger<GamePacketHandler> logger) : base(logger) {
            loginEndpoint = new IPEndPoint(IPAddress.Loopback, LoginServer.PORT);
        }

        public override void Handle(GameSession session, PacketReader packet) {
            byte function = packet.ReadByte();

            if (function == 0) {
                session.FieldManager.RemovePlayer(session, session.FieldPlayer);
                AuthData authData = AuthStorage.GetData(session.Player.AccountId);

                session.SendFinal(MigrationPacket.GameToLogin(loginEndpoint, authData));
            }
        }
    }
}