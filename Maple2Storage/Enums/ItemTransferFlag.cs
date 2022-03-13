namespace Maple2Storage.Enums;

[Flags]
public enum ItemTransferFlag : byte
{
    Untradeable = 0,
    Unknown1 = 1,
    Splitable = 2,
    Tradeable = 4,
    Binds = 8,
    LimitedTradeCount = 16,
    Unknown3 = 32
}
