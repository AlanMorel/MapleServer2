using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Login;
using Serilog;

namespace MapleServer2.PacketHandlers;

public abstract class LoginPacketHandler<T> : IPacketHandler<LoginSession>
{
    public abstract RecvOp OpCode { get; }

    protected readonly ILogger Logger = Log.Logger.ForContext<T>();

    public abstract void Handle(LoginSession session, PacketReader packet);

    public override string ToString()
    {
        return $"[{OpCode}] Login.{GetType().Name}";
    }

    protected void LogUnknownMode(Enum mode)
    {
        Logger.Warning("New Unknown {0}: 0x{1}", mode.GetType().Name, mode.ToString("X"));
    }
}
