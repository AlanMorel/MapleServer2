using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Login;

namespace MapleServer2.PacketHandlers.Common;

public abstract class HeartbeatHandler : CommonPacketHandler
{
    public override RecvOp OpCode => RecvOp.ResponseHeartbeat;

    public override void Handle(LoginSession session, PacketReader packet)
    {
        session.ServerTick = packet.ReadInt();
        session.ClientTick = packet.ReadInt();
    }
}
