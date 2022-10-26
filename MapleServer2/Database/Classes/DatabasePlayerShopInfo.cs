using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabasePlayerShopInfo : DatabaseTable
{
    public DatabasePlayerShopInfo() : base("player_shop_infos") { }

    public long Insert(PlayerShopInfo shopInfo)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            shop_id = shopInfo.ShopId,
            character_id = shopInfo.CharacterId,
            account_id = shopInfo.AccountId,
            restock_time = shopInfo.RestockTime,
            is_persistant = shopInfo.IsPersistant
        });
    }

    public Dictionary<int, PlayerShopInfo> FindAllByCharacterId(long id)
    {
        Dictionary<int, PlayerShopInfo> shopLogs = new();
        IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("character_id", id).Get();
        foreach (dynamic result in results)
        {
            PlayerShopInfo shopInfo = ReadShopLog(result);
            shopLogs.Add(shopInfo.ShopId, shopInfo);
        }

        return shopLogs;
    }

    public void Update(PlayerShopInfo shop)
    {
        QueryFactory.Query(TableName).Where("uid", shop.Uid).Update(new
        {
            restock_time = shop.RestockTime,
            total_restock_count = shop.TotalRestockCount
        });
    }

    public bool Delete(long uid)
    {
        return QueryFactory.Query(TableName).Where("uid", uid).Delete() == 1;
    }

    private static PlayerShopInfo ReadShopLog(dynamic data)
    {
        return new(data);
    }
}
