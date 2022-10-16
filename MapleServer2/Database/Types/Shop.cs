using Maple2Storage.Enums;
using MapleServer2.Types;

namespace MapleServer2.Database.Types;

public class Shop
{
    public readonly int Id;
    public readonly int Category;
    public readonly string Name;
    public readonly ShopType ShopType;
    public readonly bool HideUnuseable;
    public readonly bool DisableBuyback;
    public readonly bool OpenWallet;
    public readonly bool DisplayNew;
    public bool CanRestock;
    public long RestockTime;
    public int RestockInterval;
    public readonly ShopCurrencyType RestockCurrencyType;
    public readonly ShopCurrencyType ExcessRestockCurrencyType;
    public readonly int RestockCost;
    public readonly bool EnableRestockCostMultiplier;
    public readonly int TotalRestockCount;
    public readonly bool EnableInstantRestock;
    public readonly bool PersistantInventory;
    public List<ShopItem> Items;

    public Shop() { }

    public Shop(dynamic data)
    {
        Id = data.id;
        Category = data.category;
        Name = data.name;
        ShopType = (ShopType) data.shop_type;
        HideUnuseable = data.hide_unuseable;
        DisableBuyback = data.disable_buyback;
        OpenWallet = data.open_wallet;
        DisplayNew = data.display_new;
        CanRestock = data.can_restock;
        RestockTime = data.next_restock_timestamp;
        RestockInterval = data.restock_interval;
        RestockCurrencyType = (ShopCurrencyType) data.restock_currency_type;
        ExcessRestockCurrencyType = (ShopCurrencyType) data.excess_restock_currency_type;
        RestockCost = data.restock_cost;
        EnableRestockCostMultiplier = data.enable_restock_cost_multiplier;
        TotalRestockCount = data.total_restock_count;
        EnableInstantRestock = data.enable_instant_restock;
        PersistantInventory = data.persistant_inventory;
    }

    public void Update()
    {
        _ = UpdateRestockTime();
    }

    private async Task UpdateRestockTime()
    {
        while (TimeInfo.Now() >= RestockTime)
        {
            RestockTime += RestockInterval * 60; // convert to seconds
        }
        
        DatabaseManager.Shops.Update(this);
    }
}
