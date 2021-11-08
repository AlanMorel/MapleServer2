using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class RequestHomeBankHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.REQUEST_HOME_BANK;

    public RequestHomeBankHandler() : base() { }

    private enum BankMode : byte
    {
        House = 0x01,
        Inventory = 0x02
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        BankMode mode = (BankMode) packet.ReadByte();
        switch (mode)
        {
            case BankMode.House:
                HandleOpen(session, TimeInfo.Now());
                break;
            case BankMode.Inventory:
                HandleOpen(session);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleOpen(GameSession session, long date = 0)
    {
        session.Send(HomeBank.OpenBank(date));
    }
}
