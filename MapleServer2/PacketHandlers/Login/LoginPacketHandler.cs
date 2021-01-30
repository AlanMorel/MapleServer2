using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Login;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Login
{
    public abstract class LoginPacketHandler : IPacketHandler<LoginSession>
    {
        public abstract RecvOp OpCode { get; }

        protected readonly ILogger Logger;

        protected LoginPacketHandler(ILogger<LoginPacketHandler> logger)
        {
            Logger = logger;
        }

        public abstract void Handle(LoginSession session, PacketReader packet);

        public override string ToString() => $"[{OpCode}] Login.{GetType().Name}";
    }
}
