using Maple2Storage.Enums;

namespace MapleServer2.Database.Types;

public class GameEvent
{
    public int Id;
    public bool Active;
    public GameEventType Type;
    public long BeginTimestamp;
    public long EndTimestamp;
    public List<MapleopolyEvent> Mapleopoly;
    public List<StringBoardEvent> StringBoard;
    public UgcMapContractSaleEvent UgcMapContractSale;
    public UgcMapExtensionSaleEvent UgcMapExtensionSale;
    public FieldPopupEvent FieldPopupEvent;

    public GameEvent() { }
    public GameEvent(int id, bool active, GameEventType type, long beginTimestamp, long endTimestamp)
    {
        Id = id;
        Active = active;
        Type = type;
        BeginTimestamp = beginTimestamp;
        EndTimestamp = endTimestamp;
    }
}
public enum GameEventType
{
    StringBoard, // Marquee text
    BlueMarble, // Mapleopoly
    ExchangeScrollSale,
    TrafficOptimizer,
    PetComposeSale,
    SaleRemakes,
    SaleEnchant,
    ItemTradeRestriction,
    HotTime,
    UgcMapContractSale,
    UgcMapExtensionSale,
    DungeonBonusReward,
    EventFieldPopup,
    AttendGift // attendance
}

public class AttendGift : GameEvent
{
    public string Name;
    public string Url;
    public bool DisableClaimButton;
    public int TimeRequired;
    public EventCurrencyType SkipDayCurrencyType;
    public int SkipDaysAllowed;
    public long SkipDayCost;
    public List<AttendGiftDay> Days;

    public AttendGift(int id, bool active, GameEventType type, long beginTimestamp, long endTimestamp, List<AttendGiftDay> giftDays)
        : base(id, active, type, beginTimestamp, endTimestamp)
    {
        Days = giftDays;
    }

    //remove this
    public AttendGift() { }
}

public class AttendGiftDay
{
    public int Day;
    public int ItemId;
    public short ItemRarity;
    public int ItemAmount;

    public AttendGiftDay(int day, int itemId, short itemRarity, int itemAmount)
    {
        Day = day;
        ItemId = itemId;
        ItemRarity = itemRarity;
        ItemAmount = itemAmount;
    }
}
