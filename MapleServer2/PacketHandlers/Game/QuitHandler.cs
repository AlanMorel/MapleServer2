using System.Net;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game
{
    public class QuitHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_QUIT;
        private readonly IPEndPoint LoginEndpoint;

        public QuitHandler() : base()
        {
            string ipAddress = Environment.GetEnvironmentVariable("IP");
            int port = int.Parse(Environment.GetEnvironmentVariable("LOGIN_PORT"));
            LoginEndpoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
        }

        private enum QuitMode : byte
        {
            ChangeCharacter = 0x00,
            Quit = 0x01
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            QuitMode mode = (QuitMode) packet.ReadByte();

            switch (mode)
            {
                case QuitMode.ChangeCharacter:
                    HandleChangeCharacter(session);
                    break;
                case QuitMode.Quit:
                    HandleQuit(session);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private void HandleChangeCharacter(GameSession session)
        {
            session.ReleaseField(session.Player);
            session.FieldManager.RemovePlayer(session, session.FieldPlayer);
            DatabaseManager.Characters.Update(session.Player);
            AuthData authData = AuthStorage.GetData(session.Player.AccountId);

            session.SendFinal(MigrationPacket.GameToLogin(LoginEndpoint, authData));
        }

        private static void HandleQuit(GameSession session)
        {
            session.ReleaseField(session.Player);
            session.FieldManager.RemovePlayer(session, session.FieldPlayer);
            DatabaseManager.Characters.Update(session.Player);
            session.Dispose();
        }
    }
}
