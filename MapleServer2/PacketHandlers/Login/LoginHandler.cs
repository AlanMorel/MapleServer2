using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
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

            // Hash the password with BCrypt
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            // TODO: Change authenticate to return bool and add packet for wrong password
            Account account = DatabaseManager.Authenticate(username, password);

            // Auto add new accounts
            if (account == default)
            {
                account = new Account(username, passwordHash);
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
                    session.Send(BannerListPacket.SetBanner());
                    session.Send(ServerListPacket.SetServers(ServerName, ServerIPs));
                    break;
                case 2:
                    List<Player> characters = DatabaseManager.GetAccountCharacters(session.AccountId);

                    Logger.Debug($"Initializing login with account id: {session.AccountId}");
                    session.Send(LoginResultPacket.InitLogin(session.AccountId));
                    session.Send(UgcPacket.SetEndpoint("http://127.0.0.1/ws.asmx?wsdl", "http://127.0.0.1"));
                    session.Send(CharacterListPacket.SetMax(4, 6));
                    session.Send(CharacterListPacket.StartList());
                    // Send each character data
                    session.Send(CharacterListPacket.AddEntries(characters));
                    session.Send(CharacterListPacket.EndList());
                    break;
            }
        }
    }
}
