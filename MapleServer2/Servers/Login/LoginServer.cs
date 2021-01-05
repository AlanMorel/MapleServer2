using Autofac;
using MapleServer2.Network;
using Microsoft.Extensions.Logging;

namespace MapleServer2.Servers.Login {
    public class LoginServer : Server<LoginSession> {
        public const int PORT = 20001;

        public LoginServer(PacketRouter<LoginSession> router, ILogger<LoginServer> logger, IComponentContext context)
            : base(router, logger, context) {
        }

        public void Start() {
            base.Start(PORT);
        }
    }
}