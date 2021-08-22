using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseWallet
    {
        private readonly string TableName = "wallets";

        public long CreateWallet(Wallet wallet)
        {
            return DatabaseManager.QueryFactory.Query(TableName).InsertGetId<long>(new
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

        public void Update(Wallet wallet)
        {
            DatabaseManager.QueryFactory.Query(TableName).Where("Id", wallet.Id).Update(new
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

        public bool Delete(long id) => DatabaseManager.QueryFactory.Query(TableName).Where("Id", id).Delete() == 1;
    }
}
