using Maple2Storage.Enums;

namespace MapleServer2.Types
{
    public class Wallet
    {
        public readonly long Id;
        public Currency Meso { get; private set; }
        public Currency Meret { get; private set; }
        public Currency GameMeret { get; private set; }
        public Currency EventMeret { get; private set; }
        public Currency ValorToken { get; private set; }
        public Currency Treva { get; private set; }
        public Currency Rue { get; private set; }
        public Currency HaviFruit { get; private set; }
        public Currency MesoToken { get; private set; }
        public Currency Bank { get; private set; }

        public Wallet() { }

        public Wallet(Player player, long meso, long meret, long gameMeret, long eventMeret, long valorToken, long treva,
                    long rue, long haviFruit, long mesoToken, long bank)
        {
            Meso = new Currency(player, CurrencyType.Meso, meso);
            Meret = new Currency(player, CurrencyType.Meret, meret);
            GameMeret = new Currency(player, CurrencyType.GameMeret, gameMeret);
            EventMeret = new Currency(player, CurrencyType.EventMeret, eventMeret);
            ValorToken = new Currency(player, CurrencyType.ValorToken, valorToken);
            Treva = new Currency(player, CurrencyType.Treva, treva);
            Rue = new Currency(player, CurrencyType.Rue, rue);
            HaviFruit = new Currency(player, CurrencyType.HaviFruit, haviFruit);
            MesoToken = new Currency(player, CurrencyType.MesoToken, mesoToken);
            Bank = new Currency(player, CurrencyType.Bank, bank);
        }

        public bool RemoveMerets(long amount)
        {
            if (Meret.Modify(-amount))
            {
                return true;
            }
            if (GameMeret.Modify(-amount))
            {
                return true;
            }
            if (EventMeret.Modify(-amount))
            {
                return true;
            }
            if (Meret.Amount + GameMeret.Amount + EventMeret.Amount >= amount)
            {
                long rest = Meret.Amount + GameMeret.Amount + EventMeret.Amount - amount;
                Meret.SetAmount(rest);
                GameMeret.SetAmount(0);
                EventMeret.SetAmount(0);
                return true;
            }

            return false;
        }
    }
}
