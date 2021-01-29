using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Extensions;
using MapleServer2.Network;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Common
{
    public class SystemInfoHandler : CommonPacketHandler
    {
        public override RecvOp OpCode => RecvOp.SYSTEM_INFO;

        public SystemInfoHandler(ILogger<SystemInfoHandler> logger) : base(logger) { }

        protected override void HandleCommon(Session session, PacketReader packet)
        {
            string info = packet.ReadUnicodeString();
            Logger.Debug($"System Info: {info}");
        }
    }
}
