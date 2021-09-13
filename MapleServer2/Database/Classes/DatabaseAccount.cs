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
                account.Username,
                account.PasswordHash,
                account.CreationTime,
                account.LastLoginTime,
                account.CharacterSlots,
                meret = account.Meret.Amount,
                gamemeret = account.GameMeret.Amount,
                eventmeret = account.EventMeret.Amount,
                mesotoken = account.MesoToken.Amount,
                bankinventoryid = account.BankInventory.Id,
                account.VIPExpiration
            });
        }

        public Account FindById(long id)
        {
            return ReadAccount(QueryFactory.Query(TableName).Where("accounts.id", id)
            .LeftJoin("homes", "homes.accountid", "accounts.id")
            .Select("accounts.{*}", "homes.id as homeid")
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
                account.LastLoginTime,
                account.CharacterSlots,
                meret = account.Meret.Amount,
                gamemeret = account.GameMeret.Amount,
                eventmeret = account.EventMeret.Amount,
                mesotoken = account.MesoToken.Amount,
                account.VIPExpiration
            });
            DatabaseManager.BankInventories.Update(account.BankInventory);
        }

        public bool Delete(long id) => QueryFactory.Query(TableName).Where("id", id).Delete() == 1;

        private static Account ReadAccount(dynamic data)
        {
            BankInventory bankInventory = DatabaseManager.BankInventories.FindById(data.bankinventoryid);

            return new Account(data.id, data.username, data.passwordhash, data.creationtime, data.lastlogintime,
                data.characterslots, data.meret, data.gamemeret, data.eventmeret, data.mesotoken, data.homeid ?? 0,
                data.vipexpiration, bankInventory);
        }
    }
}
