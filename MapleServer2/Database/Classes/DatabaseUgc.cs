using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseUgc : DatabaseTable
{
    public DatabaseUgc() : base("ugc") { }

    public long Insert(Ugc ugc)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            uid = ugc.Uid,
            guid = ugc.Guid.ToString(),
            name = ugc.Name,
            url = ugc.Url,
            character_id = ugc.CharacterId,
            character_name = ugc.CharacterName,
            account_id = ugc.AccountId,
            creation_time = ugc.CreationTime,
            sale_price = ugc.SalePrice
        });
    }

    public void Update(Ugc ugc)
    {
        QueryFactory.Query(TableName).Where("uid", ugc.Uid).Update(new
        {
            name = ugc.Name,
            url = ugc.Url,
            character_id = ugc.CharacterId,
            character_name = ugc.CharacterName,
            account_id = ugc.AccountId,
            creation_time = ugc.CreationTime
        });
    }

    public Ugc FindByUid(long uid)
    {
        dynamic result = QueryFactory.Query(TableName).Where("uid", uid).FirstOrDefault();
        if (result == null)
        {
            return null;
        }

        return new()
        {
            Uid = result.uid,
            Guid = new Guid(result.guid),
            Name = result.name,
            Url = result.url,
            CharacterId = result.character_id,
            CharacterName = result.character_name,
            AccountId = result.account_id,
            CreationTime = result.creation_time,
            SalePrice = result.sale_price
        };
    }

    public bool Delete(long uid) => QueryFactory.Query(TableName).Where("uid", uid).Delete() == 1;
}
