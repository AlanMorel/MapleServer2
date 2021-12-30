using Maple2Storage.Enums;
using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseMedal : DatabaseTable
{
    public DatabaseMedal() : base("medals") { }

    public long Insert(Medal medal)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            effect_id = medal.EffectId,
            is_equipped = medal.IsEquipped,
            medal_slot = medal.Slot,
            expiration_time = medal.ExpirationTimeStamp,
            owner_account_id = medal.AccountId,
            item_uid = medal.Item.Uid
        });
    }

    public void Update(Medal medal)
    {
        QueryFactory.Query(TableName).Where("uid", medal.Uid).Update(new
        {
            is_equipped = medal.IsEquipped
        });
    }

    public List<Medal> FindAllByAccountId(long accountId)
    {
        List<Medal> medals = new();
        IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("owner_account_id", accountId).Get();
        foreach (dynamic data in results)
        {
            medals.Add(ReadMedal(data));
        }
        return medals;
    }

    public bool Delete(long uid)
    {
        return QueryFactory.Query(TableName).Where("id", uid).Delete() == 1;
    }

    private static Medal ReadMedal(dynamic data)
    {
        return new Medal()
        {
            Uid = data.uid,
            EffectId = data.effect_id,
            IsEquipped = data.is_equipped,
            ExpirationTimeStamp = data.expiration_time,
            Item = DatabaseManager.Items.FindByUid(data.item_uid),
            Slot = (MedalSlot) data.medal_slot,
            AccountId = data.owner_account_id
        };
    }
}
