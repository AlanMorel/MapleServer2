using MapleServer2.Enums;
using MapleServer2.Packets;

namespace MapleServer2.Types
{
    public class Currency
    {
        private readonly Player Player;
        private readonly CurrencyType Type;
        public long Amount { get; private set; }

        public Currency(Player player, CurrencyType type, long input)
        {
            Player = player;
            Type = type;
            Amount = input;
        }

        public bool Modify(long input)
        {
            if (Amount + input < 0)
            {
                return false;
            }
            Amount += input;
            UpdateWallet();
            return true;
        }

        public void SetAmount(long input)
        {
            if (input < 0)
            {
                return;
            }
            Amount = input;
            UpdateWallet();
        }

        private void UpdateWallet()
        {
            switch (Type)
            {
                case CurrencyType.Meso:
                    Player.Session.Send(MesosPacket.UpdateMesos(Player.Session));
                    break;
                case CurrencyType.Meret:
                case CurrencyType.GameMeret:
                case CurrencyType.EventMeret:
                    Player.Session.Send(MeretsPacket.UpdateMerets(Player.Session));
                    break;
                case CurrencyType.ValorToken:
                case CurrencyType.Treva:
                case CurrencyType.Rue:
                case CurrencyType.HaviFruit:
                    Player.Session.Send(WalletPacket.UpdateWallet(Type, Amount));
                    break;
                case CurrencyType.MesoToken:
                    break;
                default:
                    break;
            }
        }
    }
}
