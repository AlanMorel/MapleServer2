using MapleServer2.Enums;
using MapleServer2.Packets;

namespace MapleServer2.Types
{
    public class Currency
    {

        public long Amount { get; private set; }
        private readonly CurrencyType Type;
        private readonly Player Player;

        public Currency(CurrencyType type, long input, Player player)
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
            UpdateUi();
            return true;
        }

        public void SetAmount(long input)
        {
            if (input < 0)
            {
                return;
            }
            Amount = input;
            UpdateUi();
        }

        private void UpdateUi()
        {
            switch (Type)
            {
                case CurrencyType.Meso:
                    Player.Session.Send(MesosPacket.UpdateMesos(Player.Session));
                    break;
                case CurrencyType.Meret:
                    Player.Session.Send(MeretsPacket.UpdateMerets(Player.Session));
                    break;
                case CurrencyType.GameMeret:
                    Player.Session.Send(MeretsPacket.UpdateMerets(Player.Session));
                    break;
                case CurrencyType.EventMeret:
                    Player.Session.Send(MeretsPacket.UpdateMerets(Player.Session));
                    break;
                case CurrencyType.ValorToken:
                    break;
                case CurrencyType.Treva:
                    break;
                case CurrencyType.Rue:
                    break;
                case CurrencyType.HaviFruit:
                    break;
                case CurrencyType.MesoToken:
                    break;
                case CurrencyType.BlueStar:
                    break;
                case CurrencyType.RedStar:
                    break;
                case CurrencyType.EventDungeonCoin:
                    break;
                case CurrencyType.GuildCoins:
                    break;
                case CurrencyType.KayCoin:
                    break;
                case CurrencyType.MapleCoin:
                    break;
                case CurrencyType.PremiumCoin:
                    break;
                default:
                    break;
            }
        }
    }
}
