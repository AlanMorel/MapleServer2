using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseAccount : DatabaseTable
{
    public DatabaseAccount() : base("accounts") { }

    public long Insert(Account account)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            username = account.Username,
            password_hash = account.PasswordHash,
            creation_time = account.CreationTime,
            last_log_time = account.LastLogTime,
            character_slots = account.CharacterSlots,
            meret = account.Meret.Amount,
            game_meret = account.GameMeret.Amount,
            event_meret = account.EventMeret.Amount,
            meso_token = account.MesoToken.Amount,
            bank_inventory_id = account.BankInventory.Id,
            mushking_royale_id = account.MushkingRoyaleStats.Id,
            vip_expiration = account.VIPExpiration,
            meso_market_daily_listings = account.MesoMarketDailyListings,
            meso_market_monthly_purchases = account.MesoMarketMonthlyPurchases
        });
    }

    public Account FindById(long id)
    {
        return ReadAccount(QueryFactory.Query(TableName).Where("accounts.id", id)
            .LeftJoin("homes", "homes.account_id", "accounts.id")
            .Select("accounts.{*}", "homes.id as home_id")
            .FirstOrDefault());
    }

    public Account FindByUsername(string username)
    {
        return ReadAccount(QueryFactory.Query(TableName).Where("username", username).FirstOrDefault());
    }

    public bool Authenticate(string username, string password, out Account? account)
    {
        account = null;
        Account dbAccount = FindByUsername(username);

        if (BCrypt.Net.BCrypt.Verify(password, dbAccount.PasswordHash))
        {
            account = dbAccount;
            return true;
        }

        return false;
    }

    public bool AccountExists(string username)
    {
        return QueryFactory.Query(TableName).Where("username", username).AsCount().FirstOrDefault().count > 0;
    }

    public void Update(Account account)
    {
        QueryFactory.Query(TableName).Where("id", account.Id).Update(new
        {
            last_log_time = account.LastLogTime,
            character_slots = account.CharacterSlots,
            meret = account.Meret.Amount,
            game_meret = account.GameMeret.Amount,
            event_meret = account.EventMeret.Amount,
            meso_token = account.MesoToken.Amount,
            vip_expiration = account.VIPExpiration,
            meso_market_daily_listings = account.MesoMarketDailyListings,
            meso_market_monthly_purchases = account.MesoMarketMonthlyPurchases
        });
        DatabaseManager.BankInventories.Update(account.BankInventory);
        DatabaseManager.MushkingRoyaleStats.Update(account.MushkingRoyaleStats);
    }

    public bool Delete(long id)
    {
        return QueryFactory.Query(TableName).Where("id", id).Delete() == 1;
    }

    private static Account ReadAccount(dynamic data)
    {
        BankInventory bankInventory = DatabaseManager.BankInventories.FindById(data.bank_inventory_id);
        MushkingRoyaleStats royaleStats = DatabaseManager.MushkingRoyaleStats.FindById(data.mushking_royale_id);
        List<Medal> medals = DatabaseManager.MushkingRoyaleMedals.FindAllByAccountId(data.id);

        return new(data.id, data, bankInventory, royaleStats, medals, null, null);
    }
}
