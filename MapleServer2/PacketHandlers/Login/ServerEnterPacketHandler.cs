using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data;
using MapleServer2.Packets;
using MapleServer2.Servers.Login;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Login {
    public class ServerEnterPacketHandler : LoginPacketHandler {
        public override ushort OpCode => RecvOp.RESPONSE_SERVER_ENTER;

        private readonly IAccountStorage accountStorage;

        // TODO: This data needs to be dynamic
        private readonly ImmutableList<IPEndPoint> serverIps;
        private readonly string serverName;

        public ServerEnterPacketHandler(IAccountStorage accountStorage, ILogger<ServerEnterPacketHandler> logger) : base(logger) {
            this.accountStorage = accountStorage;

            var builder = ImmutableList.CreateBuilder<IPEndPoint>();
            builder.Add(new IPEndPoint(IPAddress.Any, LoginServer.PORT));

            this.serverIps = builder.ToImmutable();
            this.serverName = "Paperwood";
        }

        public override void Handle(LoginSession session, PacketReader packet) {
            session.Send(BannerListPacket.SetBanner());
            session.Send(ServerListPacket.SetServers(serverName, serverIps));

            List<Player> characters = new List<Player>();
            foreach (long characterId in accountStorage.ListCharacters(session.AccountId)) {
                characters.Add(accountStorage.GetCharacter(characterId));
            }

            session.Send(CharacterListPacket.SetMax(4, 6));
            session.Send(CharacterListPacket.StartList());
            // Send each character data
            session.Send(CharacterListPacket.AddEntries(characters));
            session.Send(CharacterListPacket.EndList());
        }
    }
}