using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using MaplePacketLib2.Tools;
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
                        string packet = input[1].Replace(" ", "");
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
