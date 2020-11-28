using MaplePacketLib2.Tools;
using MapleServer2.Servers.Login;
using Microsoft.Extensions.Logging;
using Maple2.Data.Storage;

namespace MapleServer2.PacketHandlers.Login {
    public abstract class LoginPacketHandler : IPacketHandler<LoginSession> {
        public abstract ushort OpCode { get; }

        protected readonly ILogger logger;


        protected LoginPacketHandler(ILogger<LoginPacketHandler> logger) {
            this.logger = logger;
        }

        public abstract void Handle(LoginSession session, PacketReader packet);

        public override string ToString() => $"[0x{OpCode:X4}] Login.{GetType().Name}";
    }
}