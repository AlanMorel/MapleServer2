using System.Collections.Immutable;
using System.Net;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Extensions;
using MapleServer2.Packets;
using MapleServer2.Servers.Login;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Login
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class LoginHandler : LoginPacketHandler
    {
        public override RecvOp OpCode => RecvOp.RESPONSE_LOGIN;

        // TODO: This data needs to be dynamic
        private readonly ImmutableList<IPEndPoint> ServerIPs;
        private readonly string ServerName;

        public LoginHandler(ILogger<LoginHandler> logger) : base(logger)
        {
            ImmutableList<IPEndPoint>.Builder builder = ImmutableList.CreateBuilder<IPEndPoint>();
            string ipAddress = Environment.GetEnvironmentVariable("IP");
            int port = int.Parse(Environment.GetEnvironmentVariable("LOGIN_PORT"));
            builder.Add(new IPEndPoint(IPAddress.Parse(ipAddress), port));

            ServerIPs = builder.ToImmutable();
            ServerName = Environment.GetEnvironmentVariable("NAME");
        }

        public override void Handle(LoginSession session, PacketReader packet)
        {
            byte mode = packet.ReadByte();
            string username = packet.ReadUnicodeString();
            string password = packet.ReadUnicodeString();

            Account account;
            if (DatabaseManager.Accounts.AccountExists(username.ToLower()))
            {
                if (!DatabaseManager.Accounts.Authenticate(username, password, out account))
                {
                    session.Send(LoginResultPacket.IncorrectPassword());
                    return;
                }
            }
            else
            {
                // Hash the password with BCrypt
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
                account = new Account(username, passwordHash); // Create a new account if username doesn't exist
            }

            Logger.Debug($"Logging in with account ID: {account.Id}");
            session.AccountId = account.Id;

            switch (mode)
            {
                case 1:
                    PacketWriter pWriter = PacketWriter.Of(SendOp.NPS_INFO);
                    pWriter.WriteLong();
                    pWriter.WriteUnicodeString(account.Username);

                    session.Send(pWriter);

                    List<Banner> banners = DatabaseManager.Banners.FindAllBanners();
                    session.Send(BannerListPacket.SetBanner(banners));
                    session.Send(ServerListPacket.SetServers(ServerName, ServerIPs));
                    break;
                case 2:
                    List<Player> characters = DatabaseManager.Characters.FindAllByAccountId(session.AccountId);

                    Logger.Debug($"Initializing login with account id: {session.AccountId}");
                    session.Send(LoginResultPacket.InitLogin(session.AccountId));
                    session.Send(UgcPacket.SetEndpoint("http://localhost:3000/ws.asmx?wsdl", "http://localhost:3000"));
                    session.Send(CharacterListPacket.SetMax(account.CharacterSlots));
                    session.Send(CharacterListPacket.StartList());
                    // Send each character data
                    session.Send(CharacterListPacket.AddEntries(characters));
                    session.Send(CharacterListPacket.EndList());
                    break;
            }
        }
    }
}
