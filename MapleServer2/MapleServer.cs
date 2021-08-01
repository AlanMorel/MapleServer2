using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Autofac;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Network;
using MapleServer2.Servers.Game;
using MapleServer2.Servers.Login;
using MapleServer2.Tools;
using NLog;
using Pastel;

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

            Logger.Info($"MapleServer started with {args.Length} args: {string.Join(", ", args)}");

            IContainer loginContainer = LoginContainerConfig.Configure();
            using ILifetimeScope loginScope = loginContainer.BeginLifetimeScope();
            LoginServer loginServer = loginScope.Resolve<LoginServer>();
            loginServer.Start();

            IContainer gameContainer = GameContainerConfig.Configure();
            using ILifetimeScope gameScope = gameContainer.BeginLifetimeScope();
            GameServer = gameScope.Resolve<GameServer>();
            GameServer.Start();

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
            if (DatabaseContext.Exists())
            {
                Logger.Info("Database already exists.");
                return;
            }

            Logger.Info("Creating database...");
            DatabaseContext.CreateDatabase();

            Logger.Info("Seeding shops...");
            ShopsSeeding.Seed();

            Logger.Info("Seeding Meret Market...");
            MeretMarketItemSeeding.Seed();

            Logger.Info("Seeding Mapleopoly...");
            MapleopolySeeding.Seed();

            Logger.Info("Seeding events...");
            GameEventSeeding.Seed();

            Logger.Info("Seeding card reverse game...");
            CardReverseGameSeeding.Seed();

            Logger.Info("Database created.".Pastel("#aced66"));
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
    }
}
