using Autofac;
using Maple2Storage.Extensions;
using MapleServer2.Network;
using Serilog;

namespace MapleServer2.Servers.Login;

public class LoginServer : Server<LoginSession>
{
    private List<LoginSession> Sessions;
    
    private readonly ILogger Logger = Log.Logger.ForContext<LoginServer>();

    public LoginServer(PacketRouter<LoginSession> router, IComponentContext context) : base(router, context) { }

    public void Start()
    {
        ushort port = ushort.Parse(Environment.GetEnvironmentVariable("LOGIN_PORT"));
        Start(port);
        Sessions = new();
        Logger.Information("Login Server started.");
    }

    public override void AddSession(LoginSession session)
    {
        Sessions.Add(session);
        Logger.Information("Login client connected: {session}", session);
        session.Start();
    }

    public override void RemoveSession(LoginSession session)
    {
        Sessions.Remove(session);
        Logger.Information("Login client disconnected: {session}", session);
    }

    public IEnumerable<LoginSession> GetSessions()
    {
        return Sessions;
    }
}
