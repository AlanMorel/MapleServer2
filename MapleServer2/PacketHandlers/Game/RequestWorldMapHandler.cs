using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game;

public class RequestWorldMapHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.RequestWorldMap;

    private enum WorldMapMode : byte
    {
        Open = 0x00
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        WorldMapMode mode = (WorldMapMode) packet.ReadByte();
        switch (mode)
        {
            case WorldMapMode.Open: // open
                HandleOpen(session);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }

        packet.ReadByte(); // always 0?
        int tab = packet.ReadInt();
    }

    private static void HandleOpen(GameSession session)
    {
        session.Send(WorldMapPacket.Open());
    }
}
