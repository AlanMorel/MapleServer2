using System.Net;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Network;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Common;

public class QuitHandler : CommonPacketHandler<QuitHandler>
{
    public override RecvOp OpCode => RecvOp.RequestQuit;
    private readonly IPEndPoint LoginEndpoint;

    public QuitHandler()
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
                LogUnknownMode(mode);
                break;
        }
    }

    private void HandleChangeCharacter(GameSession session)
    {
        session.SendFinal(MigrationPacket.GameToLogin(LoginEndpoint, session.Player.Account.AuthData), logoutNotice: true);
    }

    private static void HandleQuit(GameSession session)
    {
        session.Disconnect(logoutNotice: true);
    }
}
