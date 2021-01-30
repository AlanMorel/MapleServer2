using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public abstract class GamePacketHandler : IPacketHandler<GameSession>
    {
        public abstract RecvOp OpCode { get; }

        protected readonly ILogger Logger;

        protected GamePacketHandler(ILogger<GamePacketHandler> logger)
        {
            Logger = logger;
        }

        public abstract void Handle(GameSession session, PacketReader packet);

        public override string ToString() => $"[{OpCode}] Game.{GetType().Name}";
    }
}
