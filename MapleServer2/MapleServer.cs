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
using MapleServer2.Database;
//--------------------------------------------//
//--------------------------------------------//
//--------------------------------------------//



//--------------------------------------------//
//--------------------------------------------//
//--------------------------------------------//

namespace MapleServer2 {
    public static class MapleServer {
        public static void Main(string[] args) {
            // No DI here because MapleServer is static
            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info($"MapleServer started with {args.Length} args: {string.Join(", ", args)}");

            // Testing stuff

            #region Container
            IContainer loginContainer = LoginContainerConfig.Configure();
            using ILifetimeScope loginScope = loginContainer.BeginLifetimeScope();
            var loginServer = loginScope.Resolve<LoginServer>();
            loginServer.Start();

            IContainer gameContainer = GameContainerConfig.Configure();
            using ILifetimeScope gameScope = gameContainer.BeginLifetimeScope();
            var gameServer = gameScope.Resolve<GameServer>();
            gameServer.Start();

            // Input commands to the server
            while (true) {
                string[] input = (Console.ReadLine() ?? string.Empty).Split(" ", 2);
                switch (input[0]) {
                    case "exit":
                    case "quit":
                        gameServer.Stop();
                        loginServer.Stop();
                        return;
                    case "send":
                        // Remove whitespace
                        string packet = input[1].Replace(" ", "");
                        var pWriter = new PacketWriter();
                        pWriter.Write(packet.ToByteArray());
                        logger.Info(pWriter);
                        foreach (Session session in GetSessions(loginServer, gameServer)) {
                            logger.Info($"Sending packet to {session}: {pWriter}");
                            session.Send(pWriter);
                        }

                        break;
                    case "resolve":
                        PacketStructureResolver resolver = PacketStructureResolver.Parse(input[1]);
                        resolver.Start(GetSessions(loginServer, gameServer).Single());
                        break;
                    default:
                        logger.Info($"Unknown command:{input[0]} args:{(input.Length > 1 ? input[1] : "N/A")}");
                        break;
                }
            }
            #endregion
        }

        // Testing Stuff outside of a main arg

        #region Session
        private static IEnumerable<Session> GetSessions(LoginServer loginServer, GameServer gameServer) {
            List<Session> sessions = new List<Session>();
            sessions.AddRange(loginServer.GetSessions());
            sessions.AddRange(gameServer.GetSessions());

            return sessions;
        }
        #endregion
    }
}