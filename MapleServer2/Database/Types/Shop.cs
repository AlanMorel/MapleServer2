using Maple2Storage.Enums;
using MapleServer2.Types;

namespace MapleServer2.Database.Types;

public class Shop
{
    public readonly int Id;
    public readonly int Category;
    public readonly string Name;
    public readonly ShopType ShopType;
    public readonly bool RestrictSales;
    public readonly bool CanRestock;
    public readonly long NextRestock;
    public readonly bool AllowBuyback;
    public readonly bool OpenWallet;
    public readonly bool DisplayNew;
    public readonly ShopCurrencyType RestockCurrencyType;
    public readonly ShopCurrencyType ExcessRestockCurrencyType;
    public readonly int RestockCost;
    public readonly bool EnableRestockCostMultiplier;
    public readonly int TotalRestockCount;
    public readonly bool EnableInstantRestock;
    public readonly bool PersistantInventory;
    public List<ShopItem> Items;

    public Shop() { }

    public Shop(int id, int category, string name, byte shopType, bool restrictSales, bool canRestock, long nextRestock, bool allowBuyback)
    {
        Id = id;
        Category = category;
        Name = name;
        ShopType = (ShopType) shopType;
        RestrictSales = restrictSales;
        CanRestock = canRestock;
        NextRestock = nextRestock;
        AllowBuyback = allowBuyback;
    }
}
