using Maple2Storage.Enums;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class Account
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public long CreationTime { get; set; }
    public long LastLoginTime { get; set; }
    public int CharacterSlots { get; set; }
    public long VIPExpiration { get; set; }

    public Currency Meret { get; set; }
    public Currency GameMeret { get; set; }
    public Currency EventMeret { get; set; }
    public Currency MesoToken { get; private set; }

    public int MesoMarketDailyListings { get; set; }
    public int MesoMarketMonthlyPurchases { get; set; }

    public long HomeId;
    public Home Home;
    public BankInventory BankInventory;
    public Dictionary<MedalSlot, Medal> EquippedMedals;
    public List<Medal> Medals;
    public MushkingRoyaleStats MushkingRoyaleStats;

    public AuthData AuthData;

    public Account() { }

    public Account(long accountId, string username, string passwordHash,
        long creationTime, long lastLoginTime, int characterSlots, long meretAmount,
        long gameMeretAmount, long eventMeretAmount, long mesoTokens, long homeId, long vipExpiration, int mesoMarketDailyListings, int mesoMarketMonthlyPurchases,
        BankInventory bankInventory, MushkingRoyaleStats royaleStats, List<Medal> medals, AuthData authData)
    {
        Id = accountId;
        Username = username;
        PasswordHash = passwordHash;
        CreationTime = creationTime;
        LastLoginTime = lastLoginTime;
        CharacterSlots = characterSlots;
        Meret = new(CurrencyType.Meret, meretAmount);
        GameMeret = new(CurrencyType.GameMeret, gameMeretAmount);
        EventMeret = new(CurrencyType.EventMeret, eventMeretAmount);
        MesoToken = new(CurrencyType.MesoToken, mesoTokens);
        BankInventory = bankInventory;
        MushkingRoyaleStats = royaleStats;
        VIPExpiration = vipExpiration;
        HomeId = homeId;
        MesoMarketDailyListings = mesoMarketDailyListings;
        MesoMarketMonthlyPurchases = mesoMarketMonthlyPurchases;
        AuthData = authData;
        EquippedMedals = new()
        {
            { MedalSlot.Tail, null },
            { MedalSlot.GroundMount, null },
            { MedalSlot.Glider, null }
        };
        Medals = medals;
        foreach (Medal medal in medals)
        {
            if (medal.IsEquipped)
            {
                EquippedMedals[medal.Slot] = medal;
            }
        }
    }

    public Account(string username, string passwordHash)
    {
        Username = username;
        PasswordHash = passwordHash;
        CreationTime = TimeInfo.Now() + Environment.TickCount;
        LastLoginTime = TimeInfo.Now();
        CharacterSlots = 7;
        Meret = new(CurrencyType.Meret, 0);
        GameMeret = new(CurrencyType.GameMeret, 0);
        EventMeret = new(CurrencyType.EventMeret, 0);
        MesoToken = new(CurrencyType.MesoToken, 0);
        BankInventory = new();
        EquippedMedals = new()
        {
            { MedalSlot.Tail, null },
            { MedalSlot.GroundMount, null },
            { MedalSlot.Glider, null }
        };
        Medals = new();
        MushkingRoyaleStats = new();
        Id = DatabaseManager.Accounts.Insert(this);
        AuthData = new(Id);
    }

    public bool RemoveMerets(long amount)
    {
        if (Meret.Modify(-amount) || GameMeret.Modify(-amount) || EventMeret.Modify(-amount))
        {
            return true;
        }

        if (Meret.Amount + GameMeret.Amount + EventMeret.Amount < amount)
        {
            return false;
        }

        long rest = Meret.Amount + GameMeret.Amount + EventMeret.Amount - amount;
        Meret.SetAmount(rest);
        GameMeret.SetAmount(0);
        EventMeret.SetAmount(0);

        return true;
    }

    public bool IsVip() => VIPExpiration > TimeInfo.Now();

    public void AddMedal(GameSession session, Item item)
    {
        Medal medal = new(item.Function.SurvivalSkin.Id, item, session.Player.AccountId, item.Function.SurvivalSkin.Slot);
        Medals.Add(medal);

        session.Send(MushkingRoyaleSystemPacket.LoadMedals(this));
    }
}
