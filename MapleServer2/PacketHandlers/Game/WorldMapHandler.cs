using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game;

public class WorldMapHandler : GamePacketHandler<WorldMapHandler>
{
    public override RecvOp OpCode => RecvOp.RequestWorldMap;

    private enum Mode : byte
    {
        Open = 0x00
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();
        switch (mode)
        {
            case Mode.Open: // open
                HandleOpen(session);
                break;
            default:
                LogUnknownMode(mode);
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
