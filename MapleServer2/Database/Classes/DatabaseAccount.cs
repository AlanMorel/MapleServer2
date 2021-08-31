using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseAccount : DatabaseTable
    {
        public DatabaseAccount() : base("Accounts") { }

        public long Insert(Account account)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                account.Username,
                account.PasswordHash,
                account.CreationTime,
                account.LastLoginTime,
                account.CharacterSlots,
                Meret = account.Meret.Amount,
                GameMeret = account.GameMeret.Amount,
                EventMeret = account.EventMeret.Amount,
                MesoToken = account.MesoToken.Amount,
                BankInventoryId = account.BankInventory.Id,
                account.VIPExpiration
            });
        }

        public Account FindById(long id)
        {
            return ReadAccount(QueryFactory.Query(TableName).Where("Accounts.Id", id)
            .LeftJoin("Homes", "Homes.AccountId", "Accounts.Id")
            .Select("Accounts.{*}", "Homes.Id as HomeId")
            .FirstOrDefault());
        }

        public Account FindByUsername(string username) => ReadAccount(QueryFactory.Query(TableName).Where("Username", username).FirstOrDefault());

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

        public bool AccountExists(string username) => QueryFactory.Query(TableName).Where("Username", username).AsCount().FirstOrDefault().count > 0;

        public void Update(Account account)
        {
            QueryFactory.Query(TableName).Where("Id", account.Id).Update(new
            {
                account.LastLoginTime,
                account.CharacterSlots,
                Meret = account.Meret.Amount,
                GameMeret = account.GameMeret.Amount,
                EventMeret = account.EventMeret.Amount,
                MesoToken = account.MesoToken.Amount,
                account.VIPExpiration
            });
            DatabaseManager.BankInventories.Update(account.BankInventory);
        }

        public bool Delete(long id) => QueryFactory.Query(TableName).Where("Id", id).Delete() == 1;

        private static Account ReadAccount(dynamic data)
        {
            BankInventory bankInventory = DatabaseManager.BankInventories.FindById(data.BankInventoryId);

            return new Account(data.Id, data.Username, data.PasswordHash, data.CreationTime, data.LastLoginTime,
                data.CharacterSlots, data.Meret, data.GameMeret, data.EventMeret, data.MesoToken, data.HomeId ?? 0,
                data.VIPExpiration, bankInventory);
        }
    }
}
