using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game;

public class BonusGameHandler : GamePacketHandler<BonusGameHandler>
{
    public override RecvOp OpCode => RecvOp.BonusGame;

    private enum BonusGameType : byte
    {
        Open = 0x00,
        Spin = 0x02,
        Close = 0x03
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        BonusGameType mode = (BonusGameType) packet.ReadByte();
        switch (mode)
        {
            case BonusGameType.Open:
                HandleOpen(session, packet);
                break;
            case BonusGameType.Spin:
                HandleSpin(session);
                break;
            case BonusGameType.Close:
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleOpen(GameSession session, PacketReader packet)
    {
        int gameId = packet.ReadInt();

        // Static data for now
        // Tuple<item id, rarity, quantity>
        List<Tuple<int, byte, int>> items = new()
        {
            new(20000527, 1, 1),
            new(20000528, 1, 1),
            new(20000529, 1, 1),
            new(12100073, 3, 1),
            new(12088889, 3, 1),
            new(11200069, 3, 1),
            new(11900089, 3, 1),
            new(11800087, 3, 1),
            new(40220121, 4, 1),
            new(11050011, 1, 1),
            new(11050020, 1, 1),
            new(20300041, 1, 1)
        };
        session.Send(BonusGamePacket.OpenWheel(items));
    }

    private static void HandleSpin(GameSession session)
    {
        List<Tuple<int, byte, int>> items = new()
        {
            new(20000527, 1, 1),
            new(20000528, 1, 1),
            new(20000529, 1, 1),
            new(12100073, 3, 1),
            new(12088889, 3, 1),
            new(11200069, 3, 1),
            new(11900089, 3, 1),
            new(11800087, 3, 1),
            new(40220121, 4, 1),
            new(11050011, 1, 1),
            new(11050020, 1, 1),
            new(20300041, 1, 1)
        };
        int randomIndex = Random.Shared.Next(0, items.Count);
        session.Send(BonusGamePacket.SpinWheel(randomIndex, items[randomIndex]));
    }
}
