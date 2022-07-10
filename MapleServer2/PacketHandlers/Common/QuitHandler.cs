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
    private readonly IPEndPoint LoginLocalEndpoint;

    public QuitHandler()
    {
        string ipAddress = Environment.GetEnvironmentVariable("IP");
        int port = int.Parse(Environment.GetEnvironmentVariable("LOGIN_PORT"));
        LoginEndpoint = new(IPAddress.Parse(ipAddress), port);
        LoginLocalEndpoint = new(IPAddress.Parse("127.0.0.1"), port);
    }

    private enum Mode : byte
    {
        ChangeCharacter = 0x00,
        Quit = 0x01
    }

    protected override void HandleCommon(Session session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();

        switch (mode)
        {
            case Mode.ChangeCharacter:
                if (session is GameSession gameSession)
                {
                    HandleChangeCharacter(gameSession);
                }
                break;
            case Mode.Quit:
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
        session.SendFinal(MigrationPacket.GameToLogin(session.IsLocalHost() ? LoginLocalEndpoint : LoginEndpoint, session.Player.Account.AuthData), logoutNotice: true);
    }

    private static void HandleQuit(GameSession session)
    {
        session.Disconnect(logoutNotice: true);
    }
}
