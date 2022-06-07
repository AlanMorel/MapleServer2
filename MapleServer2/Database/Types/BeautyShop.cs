using Maple2Storage.Enums;

namespace MapleServer2.Database.Types;

public class BeautyShop
{
    public int Id;
    public int UniqueId; // This needs to be correct to enable voucher use
    public BeautyCategory BeautyCategory;
    public BeautyShopType BeautyType;
    public int VoucherId;
    public ShopCurrencyType CurrencyType;
    public int CurrencyCost;
    public int RequiredItemId;
    public int SpecialCost;
    public List<BeautyShopItem> Items { get; set; }

    public BeautyShop() { }

    public BeautyShop(dynamic data, List<BeautyShopItem> items)
    {
        Id = data.id;
        UniqueId = data.unique_id;
        BeautyCategory = (BeautyCategory) data.beauty_category;
        BeautyType = (BeautyShopType) data.beauty_type;
        VoucherId = data.voucher_id;
        CurrencyType = (ShopCurrencyType) data.currency_type;
        CurrencyCost = data.currency_cost;
        RequiredItemId = data.required_item_id;
        SpecialCost = data.special_cost;
        Items = items;
    }
}
