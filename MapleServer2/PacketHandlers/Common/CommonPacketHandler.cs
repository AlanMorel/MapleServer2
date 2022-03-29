using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Network;
using MapleServer2.Servers.Game;
using MapleServer2.Servers.Login;
using Serilog;

namespace MapleServer2.PacketHandlers.Common;

public abstract class CommonPacketHandler : IPacketHandler<LoginSession>, IPacketHandler<GameSession>
{
    public abstract RecvOp OpCode { get; }

    protected readonly ILogger Logger = Log.Logger.ForContext<CommonPacketHandler>();

    public virtual void Handle(GameSession session, PacketReader packet)
    {
        HandleCommon(session, packet);
    }

    public virtual void Handle(LoginSession session, PacketReader packet)
    {
        HandleCommon(session, packet);
    }

    protected abstract void HandleCommon(Session session, PacketReader packet);

    public override string ToString()
    {
        return $"[{OpCode}] Common.{GetType().Name}";
    }
}
