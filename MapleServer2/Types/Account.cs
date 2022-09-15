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
    public long LastLogTime { get; set; }
    public int CharacterSlots { get; set; }
    public long VIPExpiration { get; set; }

    public Currency Meret { get; set; }
    public Currency GameMeret { get; set; }
    public Currency EventMeret { get; set; }
    public Currency MesoToken { get; private set; }

    public int MesoMarketDailyListings { get; set; }
    public int MesoMarketMonthlyPurchases { get; set; }

    public long HomeId;
    public Home? Home;
    public BankInventory BankInventory;
    public Dictionary<MedalSlot, Medal?> EquippedMedals;
    public List<Medal> Medals;
    public MushkingRoyaleStats MushkingRoyaleStats;

    public AuthData? AuthData;

    public Account() { }

    public Account(long accountId, dynamic data, BankInventory bankInventory, MushkingRoyaleStats royaleStats, List<Medal> medals, AuthData? authData, GameSession? gameSession)
    {
        Id = accountId;
        Username = data.username;
        PasswordHash = data.password_hash;
        CreationTime = data.creation_time;
        LastLogTime = data.last_log_time;
        CharacterSlots = data.character_slots;
        Meret = new(CurrencyType.Meret, data.meret, gameSession);
        GameMeret = new(CurrencyType.GameMeret, data.game_meret, gameSession);
        EventMeret = new(CurrencyType.EventMeret, data.event_meret, gameSession);
        MesoToken = new(CurrencyType.MesoToken, data.meso_token, gameSession);
        BankInventory = bankInventory;
        MushkingRoyaleStats = royaleStats;
        VIPExpiration = data.vip_expiration;
        HomeId = data.home_id ?? 0;
        MesoMarketDailyListings = data.meso_market_daily_listings;
        MesoMarketMonthlyPurchases = data.meso_market_monthly_purchases;
        AuthData = authData;
        EquippedMedals = new()
        {
            {
                MedalSlot.Tail,
                null
            },
            {
                MedalSlot.GroundMount,
                null
            },
            {
                MedalSlot.Glider,
                null
            }
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
        LastLogTime = TimeInfo.Now();
        CharacterSlots = 7;
        Meret = new(CurrencyType.Meret, 0);
        GameMeret = new(CurrencyType.GameMeret, 0);
        EventMeret = new(CurrencyType.EventMeret, 0);
        MesoToken = new(CurrencyType.MesoToken, 0);
        BankInventory = new();
        EquippedMedals = new()
        {
            {
                MedalSlot.Tail,
                null
            },
            {
                MedalSlot.GroundMount,
                null
            },
            {
                MedalSlot.Glider,
                null
            }
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

    public void UnequipMedal(MedalSlot slot)
    {
        Medal? oldMedal = EquippedMedals[slot];
        if (oldMedal is not null)
        {
            oldMedal.IsEquipped = false;
            DatabaseManager.MushkingRoyaleMedals.Update(oldMedal);
        }
    }
}
