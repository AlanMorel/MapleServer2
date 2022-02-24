using Maple2Storage.Enums;

namespace MapleServer2.Database.Types;

public abstract class GameEvent
{
    public int Id;
    public long BeginTimestamp;
    public long EndTimestamp;

    public GameEvent() { }
    public GameEvent(int id, long beginTimestamp, long endTimestamp)
    {
        Id = id;
        BeginTimestamp = beginTimestamp;
        EndTimestamp = endTimestamp;
    }
}

/* LIST OF KNOWN EVENTS
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
*/

public class StringBoard : GameEvent
{
    public int StringId { get; set; }
    public string String { get; set; }

    // if stringId = 0, a string is required to display custom text. Otherwise the id needs to match one in /table/stringboardtext.xml
    public StringBoard() { }

    public StringBoard(int id, int stringId, string text, long beginTimestamp, long endTimestamp) : base(id, beginTimestamp, endTimestamp)
    {
        StringId = stringId;
        String = text;
    }
}

public class EventFieldPopup : GameEvent
{
    public int MapId { get; set; }

    public EventFieldPopup() { }

    public EventFieldPopup(int id, int mapId, long beginTimestamp, long endTimestamp) : base(id, beginTimestamp, endTimestamp)
    {
        Id = id;
        MapId = mapId;
    }
}

public class BlueMarble : GameEvent
{
    public List<BlueMarbleReward> Rewards { get; set; }

    public BlueMarble() { }

    public BlueMarble(int id, List<BlueMarbleReward> rewards, long beginTimestamp, long endTimestamp) : base(id, beginTimestamp, endTimestamp)
    {
        Rewards = rewards;
    }
}

public class BlueMarbleReward
{
    public int TripAmount { get; set; }
    public int ItemId { get; set; }
    public byte ItemRarity { get; set; }
    public int ItemAmount { get; set; }

    public BlueMarbleReward(int tripAmount, int itemId, byte itemRarity, int itemAmount)
    {
        TripAmount = tripAmount;
        ItemId = itemId;
        ItemRarity = itemRarity;
        ItemAmount = itemAmount;
    }
}

public class UgcMapContractSale : GameEvent
{
    public int DiscountAmount { get; set; }

    public UgcMapContractSale() { }

    public UgcMapContractSale(int id, int discountAmount, long beginTimestamp, long endTimestamp) : base(id, beginTimestamp, endTimestamp)
    {
        DiscountAmount = discountAmount;
    }
}

public class UgcMapExtensionSale : GameEvent
{
    public int DiscountAmount { get; set; }

    public UgcMapExtensionSale() { }

    public UgcMapExtensionSale(int id, int discountAmount, long beginTimestamp, long endTimestamp) : base(id, beginTimestamp, endTimestamp)
    {
        DiscountAmount = discountAmount;
    }
}

public class AttendGift : GameEvent
{
    public string Name;
    public string Url;
    public bool DisableClaimButton;
    public int TimeRequired;
    public GameEventCurrencyType SkipDayCurrencyType;
    public int SkipDaysAllowed;
    public long SkipDayCost;
    public List<AttendGiftDay> Days;

    public AttendGift(int id, long beginTimestamp, long endTimestamp, string name, string url, bool disableClaimButton, int timeRequired, GameEventCurrencyType skipdayCurrencyType,
        int skipDaysAllowed, long skipDayCost, List<AttendGiftDay> giftDays) : base(id, beginTimestamp, endTimestamp)
    {
        Name = name;
        Url = url;
        DisableClaimButton = disableClaimButton;
        TimeRequired = timeRequired;
        SkipDayCurrencyType = skipdayCurrencyType;
        SkipDaysAllowed = skipDaysAllowed;
        SkipDayCost = skipDayCost;
        Days = giftDays;
    }
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
