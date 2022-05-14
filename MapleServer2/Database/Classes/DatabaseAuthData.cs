using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseAuthData : DatabaseTable
{
    public DatabaseAuthData() : base("auth_data") { }

    public int Insert(AuthData authData)
    {
        return QueryFactory.Query(TableName).Insert(new
        {
            account_id = authData.AccountId,
            token_a = authData.TokenA,
            token_b = authData.TokenB,
            online_character_id = authData.OnlineCharacterId == 0 ? (long?) null : authData.OnlineCharacterId
        });
    }

    public void UpdateOnlineCharacterId(AuthData authData)
    {
        QueryFactory.Query(TableName).Where("account_id", authData.AccountId).Update(new
        {
            online_character_id = authData.OnlineCharacterId == 0 ? (long?) null : authData.OnlineCharacterId
        });
    }

    public AuthData GetByAccountId(long accountId)
    {
        dynamic result = QueryFactory.Query(TableName).Where("account_id", accountId).FirstOrDefault();
        if (result is null)
        {
            return null;
        }

        return new(result.token_a, result.token_b, result.account_id, result.online_character_id ?? 0);
    }
}
