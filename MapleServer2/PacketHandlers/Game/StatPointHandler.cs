using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game;

public class StatPointHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.StatPoint;

    private enum StatPointMode : byte
    {
        Increment = 0x2,
        Reset = 0x3
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        StatPointMode mode = (StatPointMode) packet.ReadByte();

        switch (mode)
        {
            case StatPointMode.Increment:
                HandleStatIncrement(session, packet);
                break;
            case StatPointMode.Reset:
                HandleResetStatDistribution(session);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleStatIncrement(GameSession session, PacketReader packet)
    {
        StatId statTypeIndex = (StatId) packet.ReadByte();

        session.Player.StatPointDistribution.AddPoint(statTypeIndex);
        session.Player.Stats.Allocate(statTypeIndex);
        session.Send(StatPointPacket.WriteStatPointDistribution(session.Player));
        session.Send(StatPacket.SetStats(session.Player.FieldPlayer));
    }

    private static void HandleResetStatDistribution(GameSession session)
    {
        session.Player.Stats.ResetAllocations(session.Player.StatPointDistribution);
        session.Send(StatPointPacket.WriteStatPointDistribution(session.Player));
        session.Send(StatPacket.SetStats(session.Player.FieldPlayer));
    }
}
