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

namespace MapleServer2
{
    public static class MapleServer
    {
        private static GameServer gameServer;

        public static void Main(string[] args)
        {
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

            // No DI here because MapleServer is static
            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info($"MapleServer started with {args.Length} args: {string.Join(", ", args)}");

            IContainer loginContainer = LoginContainerConfig.Configure();
            using ILifetimeScope loginScope = loginContainer.BeginLifetimeScope();
            LoginServer loginServer = loginScope.Resolve<LoginServer>();
            loginServer.Start();

            IContainer gameContainer = GameContainerConfig.Configure();
            using ILifetimeScope gameScope = gameContainer.BeginLifetimeScope();
            gameServer = gameScope.Resolve<GameServer>();
            gameServer.Start();

            // Input commands to the server
            while (true)
            {
                string[] input = (Console.ReadLine() ?? string.Empty).Split(" ", 2);
                switch (input[0])
                {
                    case "exit":
                    case "quit":
                        gameServer.Stop();
                        loginServer.Stop();
                        return;
                    case "send":
                        string packet = input[1];
                        PacketWriter pWriter = new PacketWriter();
                        pWriter.Write(packet.ToByteArray());
                        logger.Info(pWriter);

                        foreach (Session session in GetSessions(loginServer, gameServer))
                        {
                            logger.Info($"Sending packet to {session}: {pWriter}");
                            session.Send(pWriter);
                        }

                        break;
                    case "resolve":
                        PacketStructureResolver resolver = PacketStructureResolver.Parse(input[1]);
                        GameSession first = gameServer.GetSessions().Single();
                        resolver.Start(first);
                        break;
                    default:
                        logger.Info($"Unknown command:{input[0]} args:{(input.Length > 1 ? input[1] : "N/A")}");
                        break;
                }
            }
        }

        private static void InitDatabase()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                // Creates the database if not exists
                if (context.Database.EnsureCreated())
                {
                    Console.WriteLine("Creating database.");
                    Console.WriteLine("Seeding shops");
                    ShopsSeeding.Seed();
                    return;
                }
                Console.WriteLine("Database already exists.");
            }
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
            IEnumerable<GameSession> sessions = gameServer.GetSessions();
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
    }
}
