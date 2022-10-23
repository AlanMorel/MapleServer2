using MapleServer2.Database.Types;
using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabasePlayerShopLog : DatabaseTable
{
    public DatabasePlayerShopLog() : base("player_shop_logs") { }

    public long Insert(PlayerShopLog shopLog)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            shop_id = shopLog.ShopId,
            character_id = shopLog.CharacterId,
            account_id = shopLog.AccountId,
            restock_time = shopLog.RestockTime,
            is_persistant = shopLog.IsPersistant
        });
    }
    
    public Dictionary<int, PlayerShopLog> FindAllByCharacterId(long id)
    {
        Dictionary<int, PlayerShopLog> shopLogs = new();
        IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("character_id", id).Get();
        foreach (dynamic result in results)
        {
            PlayerShopLog shopLog = ReadShopLog(result);
            shopLogs.Add(shopLog.ShopId, shopLog);
        }

        return shopLogs;
    }
    
    public void Update(PlayerShopLog shop)
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

    private static PlayerShopLog ReadShopLog(dynamic data)
    {
        return new(data);
    }
}
