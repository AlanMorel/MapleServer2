using Autofac;
using MapleServer2.Data;
using MapleServer2.Network;
using Microsoft.Extensions.Logging;

namespace MapleServer2.Servers.Game
{
    public class GameServer : Server<GameSession>
    {
        public const int PORT = 21001;
        public static readonly PlayerStorage Storage = new PlayerStorage();
        public static readonly PartyManager PartyManager = new PartyManager();

        public GameServer(PacketRouter<GameSession> router, ILogger<GameServer> logger, IComponentContext context)
            : base(router, logger, context)
        {
        }

        public void Start()
        {
            base.Start(PORT);
        }
    }
}