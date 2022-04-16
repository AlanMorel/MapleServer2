using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Network;

namespace MapleServer2.PacketHandlers.Common;

public class SystemInfoHandler : CommonPacketHandler<SystemInfoHandler>
{
    public override RecvOp OpCode => RecvOp.SystemInfo;

    protected override void HandleCommon(Session session, PacketReader packet)
    {
        string info = packet.ReadUnicodeString();
        Logger.Debug("System Info: {info}", info);
    }
}
