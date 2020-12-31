using Autofac;
using MapleServer2.Network;
using MapleServer2.Tools;
using Microsoft.Extensions.Logging;

namespace MapleServer2.Servers.Game
{
    public class GameServer : Server<GameSession>
    {
        public const int PORT = 21001;
        public static readonly PlayerStorage Storage = new();
        public static readonly PartyManager PartyManager = new();
        public static readonly ClubManager ClubManager = new();

        public GameServer(PacketRouter<GameSession> router, ILogger<GameServer> logger, IComponentContext context) :
            base(router, logger, context) { }

        public void Start()
        {
            Start(PORT);
        }
    }
}