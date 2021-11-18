using System.Net;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data;
using MapleServer2.Network;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Common;

public class QuitHandler : CommonPacketHandler
{
    public override RecvOp OpCode => RecvOp.REQUEST_QUIT;
    private readonly IPEndPoint LoginEndpoint;

    public QuitHandler() : base()
    {
        string ipAddress = Environment.GetEnvironmentVariable("IP");
        int port = int.Parse(Environment.GetEnvironmentVariable("LOGIN_PORT"));
        LoginEndpoint = new(IPAddress.Parse(ipAddress), port);
    }

    private enum QuitMode : byte
    {
        ChangeCharacter = 0x00,
        Quit = 0x01
    }

    protected override void HandleCommon(Session session, PacketReader packet)
    {
        QuitMode mode = (QuitMode) packet.ReadByte();

        switch (mode)
        {
            case QuitMode.ChangeCharacter:
                if (session is GameSession gameSession)
                {
                    HandleChangeCharacter(gameSession);
                }
                break;
            case QuitMode.Quit:
                if (session is GameSession gameSession2)
                {
                    HandleQuit(gameSession2);
                }
                session.Dispose();
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private void HandleChangeCharacter(GameSession session)
    {
        session.FieldManager.RemovePlayer(session);

        AuthData authData = AuthStorage.GetData(session.Player.AccountId);

        session.Send(MigrationPacket.GameToLogin(LoginEndpoint, authData));
    }

    private static void HandleQuit(GameSession session)
    {
        session.Disconnect();
    }
}
