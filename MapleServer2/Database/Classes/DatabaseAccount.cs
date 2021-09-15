using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
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
                last_login_time = account.LastLoginTime,
                character_slots = account.CharacterSlots,
                meret = account.Meret.Amount,
                game_meret = account.GameMeret.Amount,
                event_meret = account.EventMeret.Amount,
                meso_token = account.MesoToken.Amount,
                bank_inventory_id = account.BankInventory.Id,
                vip_expiration = account.VIPExpiration
            });
        }

        public Account FindById(long id)
        {
            return ReadAccount(QueryFactory.Query(TableName).Where("accounts.id", id)
            .LeftJoin("homes", "homes.account_id", "accounts.id")
            .Select("accounts.{*}", "homes.id as home_id")
            .FirstOrDefault());
        }

        public Account FindByUsername(string username) => ReadAccount(QueryFactory.Query(TableName).Where("username", username).FirstOrDefault());

        public bool Authenticate(string username, string password, out Account account)
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

        public bool AccountExists(string username) => QueryFactory.Query(TableName).Where("username", username).AsCount().FirstOrDefault().count > 0;

        public void Update(Account account)
        {
            QueryFactory.Query(TableName).Where("id", account.Id).Update(new
            {
                last_login_time = account.LastLoginTime,
                character_slots = account.CharacterSlots,
                meret = account.Meret.Amount,
                game_meret = account.GameMeret.Amount,
                event_meret = account.EventMeret.Amount,
                meso_token = account.MesoToken.Amount,
                vip_expiration = account.VIPExpiration
            });
            DatabaseManager.BankInventories.Update(account.BankInventory);
        }

        public bool Delete(long id) => QueryFactory.Query(TableName).Where("id", id).Delete() == 1;

        private static Account ReadAccount(dynamic data)
        {
            BankInventory bankInventory = DatabaseManager.BankInventories.FindById(data.bank_inventory_id);

            return new Account(data.id, data.username, data.password_hash, data.creation_time, data.last_login_time,
                data.character_slots, data.meret, data.game_meret, data.event_meret, data.meso_token, data.home_id ?? 0,
                data.vip_expiration, bankInventory);
        }
    }
}
