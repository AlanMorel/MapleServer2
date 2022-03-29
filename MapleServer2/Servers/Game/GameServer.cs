using System.Reflection;
using Autofac;
using Maple2Storage.Extensions;
using MapleServer2.Commands.Core;
using MapleServer2.Database;
using MapleServer2.Managers;
using MapleServer2.Network;
using MapleServer2.Types;
using Serilog;

namespace MapleServer2.Servers.Game;

public class GameServer : Server<GameSession>
{
    public static readonly PlayerManager PlayerManager = new();
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
    public static readonly MailManager MailManager = new();
    public static readonly BlackMarketManager BlackMarketManager = new();
    public static readonly MesoMarketManager MesoMarketManager = new();
    public static readonly UgcMarketManager UgcMarketManager = new();

    private List<GameSession> Sessions;

    private readonly ILogger Logger = Log.Logger.ForContext<GameServer>();

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
        Sessions = new();
        Logger.Information("Game Server Started.");
    }

    public override void AddSession(GameSession session)
    {
        Sessions.Add(session);
        Logger.Information("Game client connected: {session}", session);
        session.Start();
    }

    public override void RemoveSession(GameSession session)
    {
        Sessions.Remove(session);
        Logger.Information("Game client disconnected: {session}", session);
    }

    public IEnumerable<GameSession> GetSessions()
    {
        return Sessions;
    }
}
