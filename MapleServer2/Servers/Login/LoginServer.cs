using System;
using Autofac;
using MapleServer2.Config;
using MapleServer2.Network;
using Microsoft.Extensions.Logging;

namespace MapleServer2.Servers.Login
{
    public class LoginServer : Server<LoginSession>
    {
        public LoginServer(PacketRouter<LoginSession> router, ILogger<LoginServer> logger, IComponentContext context)
            : base(router, logger, context)
        {
        }

        public void Start()
        {
            base.Start((ushort) Convert.ToInt16(ConfigHandler.Data["Server"]["LoginPort"]));
        }
    }
}
