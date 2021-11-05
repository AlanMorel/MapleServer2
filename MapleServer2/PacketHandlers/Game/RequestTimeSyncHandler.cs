using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game;

public class RequestTimeSyncHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.REQUEST_TIME_SYNC;

    public RequestTimeSyncHandler() : base() { }

    public override void Handle(GameSession session, PacketReader packet)
    {
        int key = packet.ReadInt();

        session.Send(TimeSyncPacket.SetSessionServerTick(key));
    }
}
