using System.Collections.Immutable;
using System.Net;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Network;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Servers.Login;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Login;

// ReSharper disable once ClassNeverInstantiated.Global
public class LoginHandler : LoginPacketHandler<LoginHandler>
{
    public override RecvOp OpCode => RecvOp.ResponseLogin;

    // TODO: This data needs to be dynamic
    private readonly ImmutableList<IPEndPoint> ServerIPs;
    private readonly ImmutableList<IPEndPoint> ServerLocalIPs;
    private readonly string ServerName;
    private readonly short ChannelCount;

    private enum Mode : byte
    {
        Banners = 0x01,
        SendCharacters = 0x02
    }

    public LoginHandler()
    {
        ImmutableList<IPEndPoint>.Builder builder = ImmutableList.CreateBuilder<IPEndPoint>();
        string ipAddress = Environment.GetEnvironmentVariable("IP");
        int port = int.Parse(Environment.GetEnvironmentVariable("LOGIN_PORT"));
        builder.Add(new(IPAddress.Parse(ipAddress), port));

        ImmutableList<IPEndPoint>.Builder localBuilder = ImmutableList.CreateBuilder<IPEndPoint>();
        localBuilder.Add(new(IPAddress.Parse(Constant.LocalHost), port));

        ServerIPs = builder.ToImmutable();
        ServerLocalIPs = localBuilder.ToImmutable();
        ServerName = Environment.GetEnvironmentVariable("NAME");
        ChannelCount = short.Parse(ConstantsMetadataStorage.GetConstant("ChannelCount"));
    }

    public override void Handle(LoginSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();
        string username = packet.ReadUnicodeString();
        string password = packet.ReadUnicodeString();

        if (string.IsNullOrEmpty(username))
        {
            session.Send(LoginResultPacket.SendLoginMode(Packets.LoginMode.IncorrectId));
            return;
        }

        Account account;
        if (DatabaseManager.Accounts.AccountExists(username.ToLower()))
        {
            if (!DatabaseManager.Accounts.Authenticate(username, password, out account))
            {
                session.Send(LoginResultPacket.SendLoginMode(Packets.LoginMode.IncorrectPassword));
                return;
            }

            Session loggedInAccount = MapleServer.GetSessions(MapleServer.GetLoginServer(), MapleServer.GetGameServer()).FirstOrDefault(p => p switch
            {
                LoginSession s => s.AccountId == account.Id,
                GameSession s => s.Player?.AccountId == account.Id,
                _ => false
            });

            if (loggedInAccount != null)
            {
                loggedInAccount.Disconnect(logoutNotice: true);
                session.Send(LoginResultPacket.SendLoginMode(Packets.LoginMode.AccountAlreadyLoggedIn));
                return;
            }
        }
        else
        {
            // Hash the password with BCrypt
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            account = new(username, passwordHash); // Create a new account if username doesn't exist
        }

        Logger.Debug("Logging in with account ID: {accountId}", account.Id);
        session.AccountId = account.Id;
        account.LastLogTime = TimeInfo.Now();
        DatabaseManager.Accounts.Update(account);

        switch (mode)
        {
            case Mode.Banners:
                SendBanners(session, account);
                break;
            case Mode.SendCharacters:
                SendCharacters(session, account);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private void SendBanners(LoginSession session, Account account)
    {
        List<Banner> banners = DatabaseManager.Banners.FindAllBanners();
        session.Send(NpsInfoPacket.SendUsername(account.Username));
        session.Send(BannerListPacket.SetBanner(banners));
        session.SendFinal(ServerListPacket.SetServers(ServerName, session.IsLocalHost() ? ServerLocalIPs : ServerIPs, ChannelCount), logoutNotice: true);
    }

    private void SendCharacters(LoginSession session, Account account)
    {
        string serverIp = session.IsLocalHost() ? Constant.LocalHost : Environment.GetEnvironmentVariable("IP");
        string webServerPort = Environment.GetEnvironmentVariable("WEB_PORT");
        string url = $"http://{serverIp}:{webServerPort}";

        List<Player> characters = DatabaseManager.Characters.FindAllByAccountId(session.AccountId);

        Logger.Debug("Initializing login with account id: {AccountId}", session.AccountId);
        session.Send(LoginResultPacket.InitLogin(session.AccountId));
        session.Send(UGCPacket.SetEndpoint($"{url}/ws.asmx?wsdl", url));
        session.Send(CharacterListPacket.SetMax(account.CharacterSlots));
        session.Send(CharacterListPacket.StartList());
        // Send each character data
        session.Send(CharacterListPacket.AddEntries(characters));
        session.Send(CharacterListPacket.EndList());
    }
}
