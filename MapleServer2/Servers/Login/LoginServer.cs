using Autofac;
using MapleServer2.Network;

namespace MapleServer2.Servers.Login
{
    public class LoginServer : Server<LoginSession>
    {
        public LoginServer(PacketRouter<LoginSession> router, IComponentContext context) : base(router, context) { }

        public void Start()
        {
            ushort port = ushort.Parse(Environment.GetEnvironmentVariable("LOGIN_PORT"));
            Start(port);
        }
    }
}
