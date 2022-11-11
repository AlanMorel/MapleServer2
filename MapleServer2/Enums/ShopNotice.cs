namespace MapleServer2.Enums;

public enum ShopNotice
{
    NotEnoughSupplies = 2, // s_err_lack_shopitem
    ItemNotFound = 4, // s_err_invalid_item
    CannotBeSold = 9, // s_msg_cant_sell
    NotEnoughMesos = 10,
    NotEnoughMerets = 11, // s_err_lack_merat
    FullInventory = 14, // s_err_inventory
    NotEnoughGuildTrophies = 15, // s_err_lack_guild_trophy
    InsufficentItems = 16, // s_err_lack_payment_item
    MustBeInGuild7Days = 19, // s_err_lack_guild_require_date
    ActiveFatigue = 22, // s_anti_addiction_cannot_receive
    CantSellItemsInThisShop = 23, // s_msg_cant_sell_to_only_sell_shop
    MustWait60Seconds = 24, // s_system_property_protection_time
    GuildLeaderCanOnlyPurchase = 25, // s_guild_err_buy_no_master
    InsufficientGuildFunds = 26, // 
    CannotPurchaseAtThisTime = 27, // s_err_invalid_item_cannot_buy_by_period
    CanOnlyBeUsedDuringEvent = 28, // s_shop_no_star_point_event
    RestrictedCountryPurchase = 31, // s_meratmarket_error_country_limit
    CannotSellSummonedPet = 32, // s_err_cannot_sell_petitem_summon
}
