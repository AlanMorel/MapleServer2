using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class HomeBankHandler : GamePacketHandler<HomeBankHandler>
{
    public override RecvOp OpCode => RecvOp.RequestHomeBank;

    private enum Mode : byte
    {
        House = 0x01,
        Inventory = 0x02
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();
        switch (mode)
        {
            case Mode.House:
                HandleOpen(session, TimeInfo.Now());
                break;
            case Mode.Inventory:
                HandleOpen(session);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleOpen(GameSession session, long date = 0)
    {
        session.Send(HomeBank.OpenBank(date));
    }
}
