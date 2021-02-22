using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class RequestHomeBankHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_HOME_BANK;

        public RequestHomeBankHandler(ILogger<RequestHomeBankHandler> logger) : base(logger) { }

        private enum BankMode : byte
        {
            Open = 0x02,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            BankMode mode = (BankMode) packet.ReadByte();
            switch (mode)
            {
                case BankMode.Open:
                    HandleOpen(session);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleOpen(GameSession session)
        {
            session.Send(HomeBank.OpenBank());
        }
    }
}
