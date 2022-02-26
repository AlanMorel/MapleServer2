using Maple2Storage.Enums;

namespace MapleServer2.Database.Types;

public abstract class GameEvent
{
    public int Id;
    public readonly long BeginTimestamp;
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
    StringBoardLink // Clickable marquee
    MeratMarketNotice // Changes the Meret Market marquee
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
    RPS // Rock Paper Scissors
    Snowman // Snowman event
*/

public class StringBoard : GameEvent
{
    public int StringId { get; set; }
    public string String { get; set; }

    // if stringId = 0, a string is required to display custom text. Otherwise the id needs to match one in /table/stringboardtext.xml
    public StringBoard() { }

    public StringBoard(int id, int stringId, string text, long beginTimestamp, long endTimestamp) : base(id,
        beginTimestamp, endTimestamp)
    {
        StringId = stringId;
        String = text;
    }
}

public class StringBoardLink : GameEvent
{
    public string Link;

    public StringBoardLink(int id, string link, long beginTimestamp, long endTimestamp) : base(id,
        beginTimestamp, endTimestamp)
    {
        Link = link;
    }
}

public class MeratMarketNotice : GameEvent
{
    public string Message;

    public MeratMarketNotice(int id, string message, long beginTimestamp, long endTimestamp) : base(id,
        beginTimestamp, endTimestamp)
    {
        Message = message;
    }
}

public class EventFieldPopup : GameEvent
{
    public int MapId { get; set; }

    public EventFieldPopup() { }

    public EventFieldPopup(int id, int mapId, long beginTimestamp, long endTimestamp) : base(id, beginTimestamp,
        endTimestamp)
    {
        Id = id;
        MapId = mapId;
    }
}

public class BlueMarble : GameEvent
{
    public List<BlueMarbleReward> Rewards { get; set; }

    public BlueMarble() { }

    public BlueMarble(int id, List<BlueMarbleReward> rewards, long beginTimestamp, long endTimestamp) : base(id,
        beginTimestamp, endTimestamp)
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

    public UgcMapContractSale(int id, int discountAmount, long beginTimestamp, long endTimestamp) : base(id,
        beginTimestamp, endTimestamp)
    {
        DiscountAmount = discountAmount;
    }
}

public class UgcMapExtensionSale : GameEvent
{
    public int DiscountAmount { get; set; }

    public UgcMapExtensionSale(int id, int discountAmount, long beginTimestamp, long endTimestamp) : base(id,
        beginTimestamp, endTimestamp)
    {
        DiscountAmount = discountAmount;
    }
}

public class AttendGift : GameEvent
{
    public readonly string Name;
    public readonly string Url;
    public readonly bool DisableClaimButton;
    public readonly int TimeRequired;
    public readonly GameEventCurrencyType SkipDayCurrencyType;
    public readonly int SkipDaysAllowed;
    public readonly long SkipDayCost;
    public readonly List<AttendGiftDay> Days;

    public AttendGift(int id, long beginTimestamp, long endTimestamp, string name, string url, bool disableClaimButton,
        int timeRequired, GameEventCurrencyType skipdayCurrencyType,
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
    public readonly int Day;
    public readonly int ItemId;
    public readonly short ItemRarity;
    public readonly int ItemAmount;

    public AttendGiftDay(int day, int itemId, short itemRarity, int itemAmount)
    {
        Day = day;
        ItemId = itemId;
        ItemRarity = itemRarity;
        ItemAmount = itemAmount;
    }
}

public class RPS : GameEvent
{
    public readonly int VoucherId;
    public readonly List<RPSTier> Tiers = new();

    public RPS(int id, int voucherId, long beginTimestamp, long endTimestamp, List<RPSTier> tiers) : base(id,
        beginTimestamp, endTimestamp)
    {
        Id = id;
        VoucherId = voucherId;
        EndTimestamp = endTimestamp;
        Tiers = tiers;
    }
}

public class RPSTier
{
    public readonly int PlayAmount;
    public readonly List<RPSReward> Rewards = new();

    public RPSTier(int playAmount, List<RPSReward> rewards)
    {
        PlayAmount = playAmount;
        Rewards = rewards;
    }
}

public class RPSReward
{
    public readonly int ItemId;
    public readonly int ItemAmount;
    public readonly short ItemRarity;

    public RPSReward(int itemId, int amount, short rarity)
    {
        ItemId = itemId;
        ItemAmount = amount;
        ItemRarity = rarity;
    }
}

public class SaleChat : GameEvent
{
    public readonly int WorldChatDiscountAmount; // ex 1000 = 10% discount
    public readonly int ChannelChatDiscountAmount;

    public SaleChat(int id, long beginTimestamp, long endTimestamp, int worldChatDiscountAmount, int channelChatDiscountAmount) : base(id,
        beginTimestamp, endTimestamp)
    {
        WorldChatDiscountAmount = worldChatDiscountAmount;
        ChannelChatDiscountAmount = channelChatDiscountAmount;
    }
}
