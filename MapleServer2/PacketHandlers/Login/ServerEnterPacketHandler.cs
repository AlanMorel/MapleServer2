using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using MaplePacketLib2.Tools;
using MapleServer2.Config;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Login;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Login
{
    public class ServerEnterPacketHandler : LoginPacketHandler
    {
        public override RecvOp OpCode => RecvOp.RESPONSE_SERVER_ENTER;

        // TODO: This data needs to be dynamic
        private readonly ImmutableList<IPEndPoint> ServerIPs;
        private readonly string ServerName;

        public ServerEnterPacketHandler(ILogger<ServerEnterPacketHandler> logger) : base(logger)
        {
            ImmutableList<IPEndPoint>.Builder builder = ImmutableList.CreateBuilder<IPEndPoint>();
            builder.Add(new IPEndPoint(IPAddress.Any, Convert.ToInt16(ConfigHandler.Data["Server"]["LoginPort"])));

            ServerIPs = builder.ToImmutable();
            ServerName = ConfigHandler.Data["Server"]["Name"];
        }

        public override void Handle(LoginSession session, PacketReader packet)
        {
            session.Send(BannerListPacket.SetBanner());
            session.Send(ServerListPacket.SetServers(ServerName, ServerIPs));

            List<Player> characters = DatabaseManager.GetAccountCharacters(session.AccountId);

            session.Send(CharacterListPacket.SetMax(4, 6));
            session.Send(CharacterListPacket.StartList());
            // Send each character data
            session.Send(CharacterListPacket.AddEntries(characters));
            session.Send(CharacterListPacket.EndList());
        }
    }
}
