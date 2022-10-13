using Maple2Storage.Enums;

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
