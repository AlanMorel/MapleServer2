using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using Serilog;

namespace MapleServer2.PacketHandlers;

public abstract class GamePacketHandler<T> : IPacketHandler<GameSession>
{
    public abstract RecvOp OpCode { get; }

    protected static readonly ILogger Logger = Log.Logger.ForContext<T>();

    public abstract void Handle(GameSession session, PacketReader packet);

    public override string ToString()
    {
        return $"[{OpCode}] Game.{GetType().Name}";
    }

    protected void LogUnknownMode(Enum mode)
    {
        Logger.Warning("New Unknown {0}: 0x{1}", mode.GetType().Name, mode.ToString("X"));
    }
}
