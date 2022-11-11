using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabasePlayerShopInventory : DatabaseTable
{
    public DatabasePlayerShopInventory() : base("player_shop_inventories") { }

    public long Insert(PlayerShopInventory inventory)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            shop_item_uid = inventory.ShopItemUid,
            shop_id = inventory.ShopId,
            character_id = inventory.CharacterId,
            account_id = inventory.AccountId,
            item_uid = inventory.ItemUid,
            purchase_count = inventory.StockPurchased,
            is_persistant = inventory.IsPersistant
        });
    }

    public Dictionary<int, PlayerShopInventory> FindAllByCharacterId(long characterId, long accountId)
    {
        Dictionary<int, PlayerShopInventory> shopInventories = new();
        IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("character_id", characterId).Get();
        foreach (dynamic result in results)
        {
            PlayerShopInventory shopInventory = ReadShopInventory(result);
            shopInventories.Add(shopInventory.ShopItemUid, shopInventory);
        }

        // find account wide inventories
        IEnumerable<dynamic> accountResults = QueryFactory.Query(TableName).Where(new
        {
            account_id = accountId,
            is_persistant = true,
        }).Get();

        foreach (dynamic result in accountResults)
        {
            if (result.character_id == characterId)
            {
                continue;
            }
            PlayerShopInventory shopInventory = ReadShopInventory(result);
            shopInventories.Add(shopInventory.ShopItemUid, shopInventory);
        }

        return shopInventories;
    }

    public bool Delete(long uid)
    {
        return QueryFactory.Query(TableName).Where("uid", uid).Delete() == 1;
    }

    public void Update(PlayerShopInventory inventory)
    {
        QueryFactory.Query(TableName).Where("uid", inventory.Uid).Update(new
        {
            purchase_count = inventory.StockPurchased
        });
    }

    private static PlayerShopInventory ReadShopInventory(dynamic data)
    {
        return new(data);
    }
}
