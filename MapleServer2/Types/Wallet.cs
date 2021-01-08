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
        public Currency BlueStar { get; private set; }
        public Currency RedStar { get; private set; }
        public Currency EventDungeonCoin { get; private set; }
        public Currency GuildCoins { get; private set; }
        public Currency KayCoin { get; private set; }
        public Currency MapleCoin { get; private set; }
        public Currency PremiumCoin { get; private set; }

        public Wallet(Player player)
        {
            Meso = new Currency(CurrencyType.Meso, 2000, player);
            Meret = new Currency(CurrencyType.Meret, 2000, player);
            GameMeret = new Currency(CurrencyType.GameMeret, 2000, player);
            EventMeret = new Currency(CurrencyType.EventMeret, 2000, player);
            ValorToken = new Currency(CurrencyType.ValorToken, 2000, player);
            Treva = new Currency(CurrencyType.Treva, 2000, player);
            Rue = new Currency(CurrencyType.Rue, 2000, player);
            HaviFruit = new Currency(CurrencyType.HaviFruit, 2000, player);
            MesoToken = new Currency(CurrencyType.MesoToken, 2000, player);
            BlueStar = new Currency(CurrencyType.BlueStar, 2000, player);
            RedStar = new Currency(CurrencyType.RedStar, 2000, player);
            EventDungeonCoin = new Currency(CurrencyType.EventDungeonCoin, 2000, player);
            GuildCoins = new Currency(CurrencyType.GuildCoins, 2000, player);
            KayCoin = new Currency(CurrencyType.KayCoin, 2000, player);
            MapleCoin = new Currency(CurrencyType.MapleCoin, 2000, player);
            PremiumCoin = new Currency(CurrencyType.PremiumCoin, 2000, player);
        }
    }
}
