using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data;
using MapleServer2.Extensions;
using MapleServer2.Packets;
using MapleServer2.Servers.Login;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Login {
    // ReSharper disable once ClassNeverInstantiated.Global
    public class LoginHandler : LoginPacketHandler {
        public override RecvOp OpCode => RecvOp.RESPONSE_LOGIN;

        // TODO: This data needs to be dynamic
        private readonly ImmutableList<IPEndPoint> serverIps;
        private readonly string serverName;

        public LoginHandler(ILogger<LoginHandler> logger) : base(logger) {
            var builder = ImmutableList.CreateBuilder<IPEndPoint>();
            builder.Add(new IPEndPoint(IPAddress.Loopback, LoginServer.PORT));

            this.serverIps = builder.ToImmutable();
            this.serverName = "Paperwood";
        }

        public override void Handle(LoginSession session, PacketReader packet) {
            byte mode = packet.ReadByte();
            string username = packet.ReadUnicodeString();
            string pass = packet.ReadUnicodeString();
            logger.Debug($"Logging in with username: '{username}' pass:'{pass}'");

            // TODO: From this user/pass lookup we should be able to find the accountId
            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(pass)) {
                // send error / account credentials not found
            }

            session.AccountId = AccountStorage.DEFAULT_ACCOUNT_ID;

            switch (mode) {
                case 1:
                    session.Send(PacketWriter.Of(SendOp.NPS_INFO).WriteLong().WriteUnicodeString(""));
                    session.Send(BannerListPacket.SetBanner());
                    session.Send(ServerListPacket.SetServers(serverName, serverIps));
                    break;
                case 2:
                    List<Player> characters = new List<Player>();
                    foreach (long characterId in AccountStorage.ListCharacters(session.AccountId)) {
                        characters.Add(AccountStorage.GetCharacter(characterId));
                    }

                    logger.Debug($"Initializing login with account id: {session.AccountId}");
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