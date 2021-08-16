using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseWallet
    {
        public static long CreateWallet(Wallet wallet)
        {
            return DatabaseManager.QueryFactory.Query("wallets").InsertGetId<long>(new
            {
                Meso = wallet.Meso.Amount,
                ValorToken = wallet.ValorToken.Amount,
                Treva = wallet.Treva.Amount,
                Rue = wallet.Rue.Amount,
                HaviFruit = wallet.HaviFruit.Amount,
                MesoToken = wallet.MesoToken.Amount,
                Bank = wallet.Bank.Amount,
            });
        }

        public static void Update(Wallet wallet)
        {
            DatabaseManager.QueryFactory.Query("wallets").Where("Id", wallet.Id).Update(new
            {
                Meso = wallet.Meso.Amount,
                ValorToken = wallet.ValorToken.Amount,
                Treva = wallet.Treva.Amount,
                Rue = wallet.Rue.Amount,
                HaviFruit = wallet.HaviFruit.Amount,
                MesoToken = wallet.MesoToken.Amount,
                Bank = wallet.Bank.Amount,
            });
        }

        public static bool Delete(long id) => DatabaseManager.QueryFactory.Query("wallets").Where("Id", id).Delete() == 1;
    }
}
