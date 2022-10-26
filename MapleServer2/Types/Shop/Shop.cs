using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Database;
using MapleServer2.Enums;

namespace MapleServer2.Types;

public class Shop : IPacketSerializable
{
    public readonly int Id;
    public int NpcId;
    public readonly int Category;
    public readonly string Name;
    public readonly ShopType ShopType;
    public readonly bool HideUnuseable;
    public readonly bool HideStats;
    public readonly bool DisableBuyback;
    public readonly bool OpenWallet;
    public readonly bool DisplayNew;
    public readonly bool RandomizeOrder;
    public readonly bool CanRestock;
    public long RestockTime;
    public readonly int RestockMinInterval;
    public readonly ShopRestockInterval RestockInterval;
    public readonly ShopCurrencyType RestockCurrencyType;
    public readonly ShopCurrencyType ExcessRestockCurrencyType;
    public readonly int RestockCost;
    public readonly bool EnableRestockCostMultiplier;
    public readonly int TotalRestockCount;
    public readonly bool DisableInstantRestock;
    public readonly bool PersistantInventory;
    public readonly int PullCount;
    public List<ShopItem> Items;

    public Shop() { }

    public Shop(dynamic data)
    {
        Id = data.id;
        Category = data.category;
        Name = data.name;
        ShopType = (ShopType) data.shop_type;
        HideUnuseable = data.hide_unuseable;
        HideStats = data.hide_stats;
        DisableBuyback = data.disable_buyback;
        OpenWallet = data.open_wallet;
        DisplayNew = data.display_new;
        RandomizeOrder = data.randomize_order;
        CanRestock = data.can_restock;
        RestockTime = data.next_restock_timestamp;
        RestockMinInterval = data.restock_min_interval;
        RestockInterval = (ShopRestockInterval) data.restock_interval;
        RestockCurrencyType = (ShopCurrencyType) data.restock_currency_type;
        ExcessRestockCurrencyType = (ShopCurrencyType) data.excess_restock_currency_type;
        RestockCost = data.restock_cost;
        EnableRestockCostMultiplier = data.enable_restock_cost_multiplier;
        DisableInstantRestock = data.disable_instant_restock;
        PersistantInventory = data.persistant_inventory;
        PullCount = data.pull_count;
    }

    public Shop(Shop serverShop, PlayerShopInfo info)
    {
        Id = serverShop.Id;
        Category = serverShop.Category;
        Name = serverShop.Name;
        NpcId = serverShop.NpcId;
        ShopType = serverShop.ShopType;
        HideUnuseable = serverShop.HideUnuseable;
        DisableBuyback = serverShop.DisableBuyback;
        OpenWallet = serverShop.OpenWallet;
        DisplayNew = serverShop.DisplayNew;
        RandomizeOrder = serverShop.RandomizeOrder;
        CanRestock = serverShop.CanRestock;
        RestockTime = info.RestockTime;
        RestockMinInterval = serverShop.RestockMinInterval;
        RestockInterval = serverShop.RestockInterval;
        RestockCurrencyType = serverShop.RestockCurrencyType;
        ExcessRestockCurrencyType = serverShop.ExcessRestockCurrencyType;
        RestockCost = serverShop.RestockCost;
        EnableRestockCostMultiplier = serverShop.EnableRestockCostMultiplier;
        TotalRestockCount = info.TotalRestockCount;
        DisableInstantRestock = serverShop.DisableInstantRestock;
        PersistantInventory = serverShop.PersistantInventory;
        PullCount = serverShop.PullCount;
        Items = new();
    }

    public void Update() => UpdateRestockTime();

    private async Task UpdateRestockTime()
    {
        while (TimeInfo.Now() >= RestockTime)
        {
            RestockTime = TimeInfo.Now() + RestockMinInterval * 60; // convert to seconds
        }

        DatabaseManager.Shops.UpdateRestockTime(this);
    }

    public void WriteTo(PacketWriter pWriter)
    {
        pWriter.WriteInt(NpcId);
        pWriter.WriteInt(Id);
        pWriter.WriteLong(RestockTime);
        pWriter.WriteInt();
        pWriter.WriteShort((short) Items.Count);
        pWriter.WriteInt(Category);
        pWriter.WriteBool(OpenWallet);
        pWriter.WriteBool(DisableBuyback);
        pWriter.WriteBool(CanRestock);
        pWriter.WriteBool(RandomizeOrder);
        pWriter.Write(ShopType);
        pWriter.WriteBool(HideUnuseable);
        pWriter.WriteBool(HideStats);
        pWriter.WriteBool(false);
        pWriter.WriteBool(DisplayNew);
        pWriter.WriteString(Name);
        if (CanRestock)
        {
            pWriter.Write(RestockCurrencyType);
            pWriter.Write(ExcessRestockCurrencyType);
            pWriter.WriteInt(); // currency item id ?
            pWriter.WriteInt(RestockCost);
            pWriter.WriteBool(EnableRestockCostMultiplier);
            pWriter.WriteInt(TotalRestockCount);
            pWriter.Write(RestockInterval);
            pWriter.WriteBool(DisableInstantRestock);
            pWriter.WriteBool(PersistantInventory);
        }
    }
}
