using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseAccount
    {
        private readonly string TableName = "Accounts";

        public long CreateAccount(Account account)
        {
            return DatabaseManager.QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                account.Username,
                account.PasswordHash,
                account.CreationTime,
                account.LastLoginTime,
                account.CharacterSlots,
                Meret = account.Meret.Amount,
                GameMeret = account.GameMeret.Amount,
                EventMeret = account.EventMeret.Amount,
            });
        }

        public Account FindById(long id)
        {
            return ReadAccount(DatabaseManager.QueryFactory.Query(TableName).Where("Accounts.Id", id)
            .LeftJoin("Homes", "Homes.AccountId", "Accounts.Id")
            .Select("Accounts.{*}", "Homes.Id as HomeId")
            .FirstOrDefault());
        }

        public Account FindByUsername(string username) => ReadAccount(DatabaseManager.QueryFactory.Query(TableName).Where("Username", username).FirstOrDefault());

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

        public bool AccountExists(string username) => DatabaseManager.QueryFactory.Query(TableName).Where("Username", username).AsCount().FirstOrDefault().count > 0;

        public void Update(Account account)
        {
            DatabaseManager.QueryFactory.Query(TableName).Where("Id", account.Id).Update(new
            {
                account.LastLoginTime,
                account.CharacterSlots,
                Meret = account.Meret.Amount,
                GameMeret = account.GameMeret.Amount,
                EventMeret = account.EventMeret.Amount,
            });
        }

        public bool Delete(long id) => DatabaseManager.QueryFactory.Query(TableName).Where("Id", id).Delete() == 1;

        private static Account ReadAccount(dynamic data) => new Account(data.Username, data.PasswordHash, data.CreationTime, data.LastLoginTime, data.CharacterSlots, data.Meret, data.GameMeret, data.EventMeret, data.Id, data.HomeId ?? 0);
    }
}
