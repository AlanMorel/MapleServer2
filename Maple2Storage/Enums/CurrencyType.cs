namespace Maple2Storage.Enums;

public enum CurrencyType : byte
{
    Meso,
    ValorToken = 3,
    Treva = 4,
    Rue = 5,
    HaviFruit = 6,
    Meret = 7,
    GameMeret = 8,
    EventMeret = 9,
    // ReverseCoin = 9, also 9?
    MentorPoints = 10,
    MenteePoints = 11,
    MesoToken = 13,
    BankMesos
}

public enum GameEventCurrencyType : byte
{
    None = 0,
    Meso = 1,
    Meret = 2
}
