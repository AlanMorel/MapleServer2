using Maple2Storage.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types
{
    public class Currency
    {
        private readonly CurrencyType Type;
        public long Amount { get; private set; }

        public Currency() { }

        public Currency(CurrencyType type, long input)
        {
            Type = type;
            Amount = input;
        }

        public bool Modify(GameSession session, long input)
        {
            if (Amount + input < 0)
            {
                return false;
            }
            Amount += input;
            UpdateWallet(session);
            return true;
        }

        public void SetAmount(GameSession session, long input)
        {
            if (input < 0)
            {
                return;
            }
            Amount = input;
            UpdateWallet(session);
        }

        private void UpdateWallet(GameSession session)
        {
            switch (Type)
            {
                case CurrencyType.Meso:
                    session.Send(MesosPacket.UpdateMesos(Amount));
                    break;
                case CurrencyType.Meret:
                case CurrencyType.GameMeret:
                case CurrencyType.EventMeret:
                    session.Send(MeretsPacket.UpdateMerets(session.Player.Account));
                    break;
                case CurrencyType.ValorToken:
                case CurrencyType.Treva:
                case CurrencyType.Rue:
                case CurrencyType.HaviFruit:
                    session.Send(WalletPacket.UpdateWallet(Type, Amount));
                    break;
                case CurrencyType.Bank:
                    session.Send(StorageInventoryPacket.UpdateMesos(Amount));
                    break;
                default:
                    break;
            }
        }
    }
}
