using System.Globalization;
using Autofac;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Extensions;
using MapleServer2.Network;
using MapleServer2.Servers.Game;
using MapleServer2.Servers.Login;
using MapleServer2.Tools;
using MapleServer2.Types;
using NLog;

namespace MapleServer2
{
    public static class MapleServer
    {
        private static GameServer GameServer;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionEventHandler);
            currentDomain.ProcessExit += new EventHandler(SaveAll);

            // Force Globalization to en-US because we use periods instead of commas for decimals
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            // Load .env file
            string dotenv = Path.Combine(Paths.SOLUTION_DIR, ".env");

            if (!File.Exists(dotenv))
            {
                throw new ArgumentException(".env file not found!");
            }
            DotEnv.Load(dotenv);
            InitDatabase();

            // Load Mob AI files
            string mobAiSchema = Path.Combine(Paths.AI_DIR, "mob-ai.xsd");
            MobAIManager.Load(Paths.AI_DIR, mobAiSchema);

            // Initialize all metadata.
            MetadataHelper.InitializeAll();

            IContainer loginContainer = LoginContainerConfig.Configure();
            using ILifetimeScope loginScope = loginContainer.BeginLifetimeScope();
            LoginServer loginServer = loginScope.Resolve<LoginServer>();
            loginServer.Start();

            IContainer gameContainer = GameContainerConfig.Configure();
            using ILifetimeScope gameScope = gameContainer.BeginLifetimeScope();
            GameServer = gameScope.Resolve<GameServer>();
            GameServer.Start();

            Logger.Info("Server Started.".ColorGreen());

            // Input commands to the server
            while (true)
            {
                string[] input = (Console.ReadLine() ?? string.Empty).Split(" ", 2);
                switch (input[0])
                {
                    case "exit":
                    case "quit":
                        GameServer.Stop();
                        loginServer.Stop();
                        return;
                    case "send":
                        if (input.Length <= 1)
                        {
                            break;
                        }
                        string packet = input[1];
                        PacketWriter pWriter = new PacketWriter();
                        pWriter.Write(packet.ToByteArray());
                        Logger.Info(pWriter);

                        foreach (Session session in GetSessions(loginServer, GameServer))
                        {
                            Logger.Info($"Sending packet to {session}: {pWriter}");
                            session.Send(pWriter);
                        }

                        break;
                    case "resolve":
                        PacketStructureResolver resolver = PacketStructureResolver.Parse(input[1]);
                        GameSession first = GameServer.GetSessions().Single();
                        resolver.Start(first);
                        break;
                    default:
                        Logger.Info($"Unknown command:{input[0]} args:{(input.Length > 1 ? input[1] : "N/A")}");
                        break;
                }
            }
        }

        private static void InitDatabase()
        {
            if (DatabaseManager.DatabaseExists())
            {
                Logger.Info("Database already exists.");
                return;
            }
            Logger.Info("Creating database...");
            DatabaseManager.CreateDatabase();

            Logger.Info("Seeding shops...");
            DatabaseManager.SeedShops();

            Logger.Info("Seeding shop items...");
            DatabaseManager.SeedShopItems();

            Logger.Info("Seeding Meret Market...");
            DatabaseManager.SeedMeretMarket();

            Logger.Info("Seeding Mapleopoly...");
            DatabaseManager.SeedMapleopoly();

            Logger.Info("Seeding events...");
            DatabaseManager.SeedEvents();

            Logger.Info("Seeding card reverse game...");
            DatabaseManager.SeedCardReverseGame();

            Logger.Info("Database created.".ColorGreen());
        }

        public static void BroadcastPacketAll(Packet packet, GameSession sender = null)
        {
            BroadcastAll(session =>
            {
                if (session == sender)
                {
                    return;
                }
                session.Send(packet);
            });
        }

        public static void BroadcastAll(Action<GameSession> action)
        {
            IEnumerable<GameSession> sessions = GameServer.GetSessions();
            lock (sessions)
            {
                foreach (GameSession session in sessions)
                {
                    action?.Invoke(session);
                }
            }
        }

        // Testing Stuff outside of a main arg
        private static IEnumerable<Session> GetSessions(LoginServer loginServer, GameServer gameServer)
        {
            List<Session> sessions = new List<Session>();
            sessions.AddRange(loginServer.GetSessions());
            sessions.AddRange(gameServer.GetSessions());

            return sessions;
        }

        private static void UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception) args.ExceptionObject;
            Logger.Fatal($"Exception Type: {e.GetType()}\nMessage: {e.Message}\nStack Trace: {e.StackTrace}\n");
        }

        private static void SaveAll(object sender, EventArgs e)
        {
            List<Player> players = GameServer.Storage.GetAllPlayers();
            foreach (Player item in players)
            {
                DatabaseManager.Characters.Update(item);
            }

            List<Guild> guilds = GameServer.GuildManager.GetAllGuilds();
            foreach (Guild item in guilds)
            {
                DatabaseManager.Guilds.Update(item);
            }

            List<Home> homes = GameServer.HomeManager.GetAllHomes();
            foreach (Home home in homes)
            {
                DatabaseManager.Homes.Update(home);
            }
        }
    }
}
