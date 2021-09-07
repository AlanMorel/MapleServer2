using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Network;
using MapleServer2.Packets;

namespace MapleServer2.PacketHandlers.Common
{
    public class RequestTimeSyncHandler : CommonPacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_TIME_SYNC;

        public RequestTimeSyncHandler() : base() { }

        protected override void HandleCommon(Session session, PacketReader packet)
        {
            int key = packet.ReadInt();

            session.Send(TimeSyncPacket.SetSessionServerTick(key));
        }
    }
}
