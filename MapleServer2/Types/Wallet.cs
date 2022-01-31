using Maple2Storage.Enums;
using MapleServer2.Database;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class Wallet
{
    public readonly long Id;

    public Currency Meso { get; private set; }
    public Currency ValorToken { get; private set; }
    public Currency Treva { get; private set; }
    public Currency Rue { get; private set; }
    public Currency HaviFruit { get; private set; }

    public Wallet(long meso, long valorToken, long treva, long rue, long haviFruit, GameSession gameSession, long id = 0)
    {
        Meso = new(CurrencyType.Meso, meso, gameSession);
        ValorToken = new(CurrencyType.ValorToken, valorToken, gameSession);
        Treva = new(CurrencyType.Treva, treva, gameSession);
        Rue = new(CurrencyType.Rue, rue, gameSession);
        HaviFruit = new(CurrencyType.HaviFruit, haviFruit, gameSession);

        if (id == 0)
        {
            Id = DatabaseManager.Wallets.Insert(this);
            return;
        }
        Id = id;
    }
}
