using System.Globalization;
using Autofac;
using Maple2Storage.Extensions;
using Maple2Storage.Tools;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Database;
using MapleServer2.Managers;
using MapleServer2.Network;
using MapleServer2.Servers.Game;
using MapleServer2.Servers.Login;
using MapleServer2.Tools;
using MapleServer2.Types;
using NLog;
using TaskScheduler = MapleServer2.Tools.TaskScheduler;

namespace MapleServer2;

public static class MapleServer
{
    private static GameServer _gameServer;
    private static LoginServer _loginServer;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public static async Task Main()
    {
        AppDomain currentDomain = AppDomain.CurrentDomain;
        currentDomain.UnhandledException += UnhandledExceptionEventHandler;
        currentDomain.ProcessExit += SaveAll;

        // Force Globalization to en-US because we use periods instead of commas for decimals
        CultureInfo.CurrentCulture = new("en-US");

        // Load .env file
        string dotenv = Path.Combine(Paths.SOLUTION_DIR, ".env");

        if (!File.Exists(dotenv))
        {
            throw new ArgumentException(".env file not found!");
        }

        DotEnv.Load(dotenv);

        DatabaseManager.Init();

        DateTimeOffset lastReset = DatabaseManager.ServerInfo.GetLastDailyReset();
        DateTimeOffset now = DateTimeOffset.UtcNow;
        DateTime lastMidnight = new(now.Year, now.Month, now.Day, 0, 0, 0, 0);

        // Check if lastReset is before lastMidnight
        if (lastReset < lastMidnight)
        {
            DailyReset();
        }

        // Schedule daily reset and repeat every 24 hours
        TaskScheduler.Instance.ScheduleTask(0, 0, 24, 24 * 60, DailyReset);

        // Load Mob AI files
        string mobAiSchema = Path.Combine(Paths.AI_DIR, "mob-ai.xsd");
        MobAIManager.Load(Paths.AI_DIR, mobAiSchema);

        // Initialize all metadata.
        await MetadataHelper.InitializeAll();

        // Run global events
        GlobalEventManager.ScheduleEvents();

        IContainer loginContainer = LoginContainerConfig.Configure();
        using ILifetimeScope loginScope = loginContainer.BeginLifetimeScope();
        _loginServer = loginScope.Resolve<LoginServer>();
        _loginServer.Start();

        IContainer gameContainer = GameContainerConfig.Configure();
        using ILifetimeScope gameScope = gameContainer.BeginLifetimeScope();
        _gameServer = gameScope.Resolve<GameServer>();
        _gameServer.Start();

        Logger.Info("All Servers have been Started.".ColorGreen());

        // Input commands to the server
        while (true)
        {
            string[] input = (Console.ReadLine() ?? string.Empty).Split(" ", 2);
            switch (input[0])
            {
                case "exit":
                case "quit":
                    _gameServer.Stop();
                    _loginServer.Stop();
                    return;
                case "send":
                    if (input.Length <= 1)
                    {
                        break;
                    }

                    string packet = input[1];
                    PacketWriter pWriter = new();
                    pWriter.WriteBytes(packet.ToByteArray());
                    Logger.Info(pWriter);

                    foreach (Session session in GetSessions(_loginServer, _gameServer))
                    {
                        Logger.Info($"Sending packet to {session}: {pWriter}");
                        session.Send(pWriter);
                    }

                    break;
                case "resolve":
                    // How to use inside the PacketStructureResolver class
                    PacketStructureResolver resolver = PacketStructureResolver.Parse(input[1]);
                    if (resolver is null)
                    {
                        break;
                    }

                    GameSession first = _gameServer.GetSessions().FirstOrDefault();
                    if (first is null)
                    {
                        break;
                    }

                    resolver.Start(first);
                    break;
                default:
                    Logger.Info($"Unknown command:{input[0]} args:{(input.Length > 1 ? input[1] : "N/A")}");
                    break;
            }
        }
    }

    public static GameServer GetGameServer()
    {
        return _gameServer;
    }

    public static LoginServer GetLoginServer()
    {
        return _loginServer;
    }

    private static void DailyReset()
    {
        List<Player> players = GameServer.PlayerManager.GetAllPlayers();
        foreach (Player player in players)
        {
            player.GatheringCount = new();
            DatabaseManager.Characters.Update(player);

            List<GameEventUserValue> expiredValues = player.EventUserValues.Where(x => x.ExpirationTimestamp <= TimeInfo.Now()).ToList();
            foreach (GameEventUserValue userValue in expiredValues)
            {
                DatabaseManager.GameEventUserValue.Delete(userValue);
                player.EventUserValues.Remove(userValue);
            }
        }

        DatabaseManager.RunQuery("UPDATE `characters` SET gathering_count = '[]'");

        DatabaseManager.ServerInfo.SetLastDailyReset(TimeInfo.CurrentDate());
    }

    public static void BroadcastPacketAll(PacketWriter packet, GameSession sender = null)
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
        IEnumerable<GameSession> sessions = _gameServer.GetSessions();
        lock (sessions)
        {
            foreach (GameSession session in sessions)
            {
                action?.Invoke(session);
            }
        }
    }

    // Testing Stuff outside of a main arg
    public static IEnumerable<Session> GetSessions(LoginServer loginServer, GameServer gameServer)
    {
        List<Session> sessions = new();
        sessions.AddRange(loginServer.GetSessions());
        sessions.AddRange(gameServer.GetSessions());

        return sessions;
    }

    private static void UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs args)
    {
        SaveAll(sender, args);
        Exception e = (Exception) args.ExceptionObject;
        Logger.Fatal($"Exception Type: {e.GetType()}\nMessage: {e.Message}\nStack Trace: {e.StackTrace}\n");
    }

    private static void SaveAll(object sender, EventArgs e)
    {
        List<Player> players = GameServer.PlayerManager.GetAllPlayers();
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
