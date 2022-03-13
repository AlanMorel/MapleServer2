namespace MapleServer2.Enums;

public enum TransferType : byte
{
    Tradeable = 0,
    Untradeable = 1,
    BindOnLoot = 2,
    BindOnEquip = 3,
    BindOnUse = 4,
    BindOnTrade = 5,
    TradeableOnBlackMarket = 6, // ??
    BindOnSummonEnchantOrReroll = 7
}
