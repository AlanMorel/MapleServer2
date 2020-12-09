using MaplePacketLib2.Tools;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public abstract class GamePacketHandler : IPacketHandler<GameSession>
    {
        public abstract ushort OpCode { get; }

        protected readonly ILogger logger;

        protected GamePacketHandler(ILogger<GamePacketHandler> logger)
        {
            this.logger = logger;
        }

        public abstract void Handle(GameSession session, PacketReader packet);

        public override string ToString() => $"[0x{OpCode:X4}] Game.{GetType().Name}";
    }
}