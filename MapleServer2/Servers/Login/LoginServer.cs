using Autofac;
using MapleServer2.Network;

namespace MapleServer2.Servers.Login
{
    public class LoginServer : Server<LoginSession>
    {
        private List<LoginSession> Sessions;

        public LoginServer(PacketRouter<LoginSession> router, IComponentContext context) : base(router, context) { }

        public void Start()
        {
            ushort port = ushort.Parse(Environment.GetEnvironmentVariable("LOGIN_PORT"));
            Start(port);
            Sessions = new List<LoginSession>();
        }

        public override void AddSession(LoginSession session)
        {
            Sessions.Add(session);
            Logger.Info($"Login client connected: {session}");
            session.Start();
        }

        public override void RemoveSession(LoginSession session)
        {
            Sessions.Remove(session);
            Logger.Info($"Login client disconnected: {session}");
        }

        public List<LoginSession> GetSessions()
        {
            return Sessions;
        }
    }
}
