using System.Globalization;
using Autofac;
using Maple2.PathEngine;
using Maple2.PathEngine.Utils;
using Maple2Storage.Tools;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Managers;
using MapleServer2.Network;
using MapleServer2.Servers.Game;
using MapleServer2.Servers.Login;
using MapleServer2.Tools;
using MapleServer2.Types;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Serilog.Templates;
using Serilog.Templates.Themes;
using Path = System.IO.Path;
using TaskScheduler = MapleServer2.Tools.TaskScheduler;

namespace MapleServer2;

public static class MapleServer
{
    public static readonly PathEngine PathEngine = new(new PrintErrorHandler(Console.Out));

    private static GameServer _gameServer;
    private static LoginServer _loginServer;
    private static ILogger _logger;

    public static async Task Main()
    {
        // Setup Serilog
        const string ConsoleOutputTemplate = "[{@t:HH:mm:ss}] [{@l:u3}]" +
                                             "{#if SourceContext is not null} {Substring(SourceContext, LastIndexOf(SourceContext, '.') + 1)}:{#end} {@m}\n{@x}";
        const string FileOutputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level}] {SourceContext:l}: {Message:lj}{NewLine}{Exception}";

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(new ExpressionTemplate(ConsoleOutputTemplate, theme: TemplateTheme.Code))
            .WriteTo.File($"{Paths.SOLUTION_DIR}/Logs/MapleServer2/LOG-.txt",
                rollingInterval: RollingInterval.Day, outputTemplate: FileOutputTemplate, restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();

        _logger = Log.Logger.ForContext(typeof(MapleServer));

        AppDomain currentDomain = AppDomain.CurrentDomain;
        currentDomain.UnhandledException += UnhandledExceptionEventHandler;
        currentDomain.ProcessExit += Shutdown;

        // Force Globalization to en-US because we use periods instead of commas for decimals
        CultureInfo.CurrentCulture = new("en-US");

        // Load .env file
        string dotenv = Path.Combine(Paths.SOLUTION_DIR, ".env");

        if (!File.Exists(dotenv))
        {
            throw new FileNotFoundException(".env file not found!");
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
        TaskScheduler.Instance.ScheduleTask(0, 0, 24 * 60, DailyReset);

        // Load Mob AI files
        string mobAiSchema = Path.Combine(Paths.AI_DIR, "mob-ai.xsd");
        MobAIManager.Load(Paths.AI_DIR, mobAiSchema);

        // Initialize all metadata.
        await MetadataHelper.InitializeAll();

        // Run global events
        GlobalEventManager.ScheduleEvents();

        IContainer loginContainer = LoginContainerConfig.Configure();
        await using ILifetimeScope loginScope = loginContainer.BeginLifetimeScope();
        _loginServer = loginScope.Resolve<LoginServer>();
        _loginServer.Start();

        IContainer gameContainer = GameContainerConfig.Configure();
        await using ILifetimeScope gameScope = gameContainer.BeginLifetimeScope();
        _gameServer = gameScope.Resolve<GameServer>();
        _gameServer.Start();

        _logger.Information("All Servers have been Started.");

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
                    _logger.Information(pWriter.ToString());

                    foreach (Session session in GetSessions(_loginServer, _gameServer))
                    {
                        _logger.Information("Sending packet to {session}: {pWriter}", session, pWriter);
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
                    _logger.Information("Unknown command:{input[0]} args:{input}", input[0], input.Length > 1 ? input[1] : "N/A");
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

        // Weekly reset
        if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
        {
            WeeklyReset(players);
        }

        DatabaseManager.RunQuery("UPDATE `characters` SET gathering_count = '[]'");

        DatabaseManager.ServerInfo.SetLastDailyReset(TimeInfo.CurrentDate());
    }

    private static void WeeklyReset(List<Player> players)
    {
        foreach (Player player in players)
        {
            player.PrestigeMissions = PrestigeLevelMissionMetadataStorage.GetPrestigeMissions;
        }

        string missions = JsonConvert.SerializeObject(PrestigeLevelMissionMetadataStorage.GetPrestigeMissions);
        DatabaseManager.RunQuery($"UPDATE `characters` SET prestige_missions = '{missions}'");
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
        SaveAll();
        Exception e = (Exception) args.ExceptionObject;
        _logger.Fatal("Exception Type: {type}\nMessage: {message}\nStack Trace: {stackTrace}\n",
            e.GetType(), e.Message, e.StackTrace);
    }

    private static void SaveAll()
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

    private static void Shutdown(object sender, EventArgs e)
    {
        SaveAll();
        Log.CloseAndFlush();
    }
}
