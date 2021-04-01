namespace Maple2Storage.Types
{
    public enum MeretMarketCategory : int
    {
        Promo = 10,
        Functional = 40300,
        Lifestyle = 40600
    }

    public enum MeretMarketJobRequirement : int // not all requirements are listed. Individual job values can be summed to make a specific job list
    {
        Knight = 1,
        Berserker = 4,
        KnightBerserker = 5,
        Wizard = 8,
        Priest = 16,
        Archer = 32,
        HeavyGunner = 64,
        Thief = 128,
        Assassin = 256,
        Runeblader = 512,
        Striker = 1024,
        SoulBinder = 2048,
        All = 4095
    }

    public enum MeretMarketItemFlag : byte
    {
        New = 1,
        Hot = 2,
        Event = 3,
        Sale = 4,
        Special = 5
    }

    public enum MeretMarketCurrencyType : byte
    {
        Meso = 0,
        Meret = 1,
        RedMeret = 2
    }

    public enum MeretMarketPromoFlag : int
    {
        PinkGift = 1,
        BlueGift = 2
    }
}
