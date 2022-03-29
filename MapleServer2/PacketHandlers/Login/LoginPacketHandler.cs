using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Login;
using Serilog;

namespace MapleServer2.PacketHandlers.Login;

public abstract class LoginPacketHandler : IPacketHandler<LoginSession>
{
    public abstract RecvOp OpCode { get; }

    protected readonly ILogger Logger = Log.Logger.ForContext<LoginPacketHandler>();

    public abstract void Handle(LoginSession session, PacketReader packet);

    public override string ToString()
    {
        return $"[{OpCode}] Login.{GetType().Name}";
    }
}
