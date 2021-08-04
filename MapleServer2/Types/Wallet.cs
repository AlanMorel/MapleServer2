using Maple2Storage.Enums;

namespace MapleServer2.Types
{
    public class Wallet
    {
        public readonly long Id;

        public Currency Meso { get; private set; }
        public Currency ValorToken { get; private set; }
        public Currency Treva { get; private set; }
        public Currency Rue { get; private set; }
        public Currency HaviFruit { get; private set; }
        public Currency MesoToken { get; private set; }
        public Currency Bank { get; private set; }

        public Wallet() { }

        public Wallet(long meso, long valorToken, long treva, long rue, long haviFruit, long mesoToken, long bank)
        {
            Meso = new Currency(CurrencyType.Meso, meso);
            ValorToken = new Currency(CurrencyType.ValorToken, valorToken);
            Treva = new Currency(CurrencyType.Treva, treva);
            Rue = new Currency(CurrencyType.Rue, rue);
            HaviFruit = new Currency(CurrencyType.HaviFruit, haviFruit);
            MesoToken = new Currency(CurrencyType.MesoToken, mesoToken);
            Bank = new Currency(CurrencyType.Bank, bank);
        }
    }
}
