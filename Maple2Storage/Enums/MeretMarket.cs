namespace Maple2Storage.Enums;

public enum MeretMarketSection
{
    All = 0,
    PremiumMarket = 100000,
    RedMeretMarket = 100001,
    UgcMarket = 110000
}

public enum MeretMarketCategory
{
    None = 0,
    Promo = 10,
    Functional = 40300,
    Lifestyle = 40600
}

public enum MeretMarketItemFlag : byte
{
    None = 0,
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

public enum MeretMarketPromoFlag
{
    None = 0,
    PinkGift = 1,
    BlueGift = 2
}

public enum MeretMarketSort : short
{
    None = 0,
    MostPopularUgc = 1,
    PriceLowest = 2,
    PriceHighest = 3,
    MostRecent = 4,
    MostPopularPremium = 5,
    TopSeller = 6
}
