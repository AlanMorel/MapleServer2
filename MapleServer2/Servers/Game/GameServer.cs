using System.Reflection;
using Autofac;
using MapleServer2.Commands.Core;
using MapleServer2.Database;
using MapleServer2.Network;
using MapleServer2.Managers;
using MapleServer2.Types;

namespace MapleServer2.Servers.Game
{
    public class GameServer : Server<GameSession>
    {
        public static readonly PlayerStorage Storage = new();
        public static readonly PartyManager PartyManager = new();
        public static readonly ClubManager ClubManager = new();
        public static readonly DungeonManager DungeonManager = new();
        public static readonly GuildManager GuildManager = new();
        public static readonly GroupChatManager GroupChatManager = new();
        public static readonly HongBaoManager HongBaoManager = new();
        public static readonly BuddyManager BuddyManager = new();
        public static readonly HomeManager HomeManager = new();
        public static readonly CommandManager CommandManager = new();
        public static readonly GlobalEventManager GlobalEventManager = new();

        public GameServer(PacketRouter<GameSession> router, IComponentContext context) : base(router, context) { }

        public void Start()
        {
            List<Guild> guilds = DatabaseManager.Guilds.FindAll();
            foreach (Guild guild in guilds)
            {
                GuildManager.AddGuild(guild);
            }

            ushort port = ushort.Parse(Environment.GetEnvironmentVariable("GAME_PORT"));
            Start(port);
            CommandManager.RegisterAll(Assembly.GetAssembly(typeof(CommandBase)));
        }
    }
}
