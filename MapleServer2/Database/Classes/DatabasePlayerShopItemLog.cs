using MapleServer2.Database.Types;
using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabasePlayerShopItemLog : DatabaseTable
{
    public DatabasePlayerShopItemLog() : base("player_shop_item_logs") { }

    public long Insert(PlayerShopItemLog itemLog)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            shop_item_uid = itemLog.ShopItemUid,
            shop_id = itemLog.ShopId,
            character_id = itemLog.CharacterId,
            account_id = itemLog.AccountId,
            item_uid = itemLog.ItemUid,
            purchase_count = itemLog.StockPurchased,
            is_persistant = itemLog.IsPersistant
        });
    }
    
    public Dictionary<int, PlayerShopItemLog> FindAllByCharacterId(long characterId, long accountId)
    {
        Dictionary<int, PlayerShopItemLog> shopItemLogs = new();
        IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("character_id", characterId).Get();
        foreach (dynamic result in results)
        {
            PlayerShopItemLog shopItemLog = ReadShopItemLog(result);
            shopItemLogs.Add(shopItemLog.ShopItemUid, shopItemLog);
        }
        
        // find account wide logs
        IEnumerable<dynamic> accountResults = QueryFactory.Query(TableName).Where(new {
            account_id = accountId,
            is_persistant = true,
        }).Get();
        
        foreach (dynamic result in accountResults)
        {
            if (result.character_id == characterId)
            {
                continue;
            }
            PlayerShopItemLog shopItemLog = ReadShopItemLog(result);
            shopItemLogs.Add(shopItemLog.ShopItemUid, shopItemLog);
        }

        return shopItemLogs;
    }
    
    public bool Delete(long uid)
    {
        return QueryFactory.Query(TableName).Where("uid", uid).Delete() == 1;
    }
    
    public void Update(PlayerShopItemLog itemLog)
    {
        QueryFactory.Query(TableName).Where("uid", itemLog.Uid).Update(new
        {
            purchase_count = itemLog.StockPurchased
        });
    }

    private static PlayerShopItemLog ReadShopItemLog(dynamic data)
    {
        return new(data);
    }
}
