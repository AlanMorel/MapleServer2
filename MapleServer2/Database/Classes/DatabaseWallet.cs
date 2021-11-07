using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseWallet : DatabaseTable
{
    public DatabaseWallet() : base("wallets") { }

    public long Insert(Wallet wallet)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            meso = wallet.Meso.Amount,
            valor_token = wallet.ValorToken.Amount,
            treva = wallet.Treva.Amount,
            rue = wallet.Rue.Amount,
            havi_fruit = wallet.HaviFruit.Amount
        });
    }

    public void Update(Wallet wallet)
    {
        QueryFactory.Query(TableName).Where("id", wallet.Id).Update(new
        {
            meso = wallet.Meso.Amount,
            valor_token = wallet.ValorToken.Amount,
            treva = wallet.Treva.Amount,
            rue = wallet.Rue.Amount,
            havi_fruit = wallet.HaviFruit.Amount
        });
    }

    public bool Delete(long id)
    {
        return QueryFactory.Query(TableName).Where("id", id).Delete() == 1;
    }
}
