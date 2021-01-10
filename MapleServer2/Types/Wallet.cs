using MapleServer2.Enums;

namespace MapleServer2.Types
{
    public class Wallet
    {
        public Currency Meso { get; private set; }
        public Currency Meret { get; private set; }
        public Currency GameMeret { get; private set; }
        public Currency EventMeret { get; private set; }
        public Currency ValorToken { get; private set; }
        public Currency Treva { get; private set; }
        public Currency Rue { get; private set; }
        public Currency HaviFruit { get; private set; }
        public Currency MesoToken { get; private set; }

        public Wallet(Player player)
        {
            Meso = new Currency(player, CurrencyType.Meso, 2000);
            Meret = new Currency(player, CurrencyType.Meret, 2000);
            GameMeret = new Currency(player, CurrencyType.GameMeret, 2000);
            EventMeret = new Currency(player, CurrencyType.EventMeret, 2000);
            ValorToken = new Currency(player, CurrencyType.ValorToken, 2000);
            Treva = new Currency(player, CurrencyType.Treva, 2000);
            Rue = new Currency(player, CurrencyType.Rue, 2000);
            HaviFruit = new Currency(player, CurrencyType.HaviFruit, 2000);
            MesoToken = new Currency(player, CurrencyType.MesoToken, 2000);
        }
    }
}
