using Maple2Storage.Enums;
using MapleServer2.Database.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseEvent : DatabaseTable
{
    public DatabaseEvent() : base("events") { }

    public long Insert(GameEvent gameEvent)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            gameEvent.Id,
            begin_timestamp = gameEvent.BeginTimestamp,
            end_timestamp = gameEvent.EndTimestamp,
        });
    }

    public EventFieldPopup FindFieldPopupEvent()
    {
        dynamic result = QueryFactory.Query("event_field_popup").FirstOrDefault();
        return ReadFieldPopupEvent(result);
    }

    public BlueMarble FindMapleopolyEvent()
    {
        BlueMarble mapleopolyRewards = new();
        IEnumerable<dynamic> results = QueryFactory.Query("event_mapleopoly").Get();
        return ReadMapleopolyEvent(results);
    }

    public UgcMapContractSale FindUgcMapContractSaleEvent()
    {
        dynamic result = QueryFactory.Query("event_ugc_map_contract_sale").FirstOrDefault();
        return ReadUgcMapContractSaleEvent(result);
    }

    public UgcMapExtensionSale FindUgcMapExtensionSaleEvent()
    {
        dynamic result = QueryFactory.Query("event_ugc_map_extension_sale").FirstOrDefault();
        return ReadUgcMapExtensionSaleEvent(result);
    }

    public IEnumerable<StringBoard> FindAllStringBoardEvent()
    {
        List<StringBoard> stringBoardEvents = new();
        IEnumerable<dynamic> results = QueryFactory.Query("event_string_boards").Get();
        foreach (dynamic data in results)
        {
            stringBoardEvents.Add(ReadStringBoardEvent(data));
        }

        return stringBoardEvents;
    }

    public StringBoardLink FindStringBoardLinkEvent()
    {
        dynamic result = QueryFactory.Query("event_string_board_links").FirstOrDefault();
        return ReadStringBoardLinkEvent(result);
    }

    public MeratMarketNotice FindMeratMarketNotice()
    {
        dynamic result = QueryFactory.Query("event_meret_market_notices").FirstOrDefault();
        return ReadMeratMarketNoticeEvent(result);
    }

    public AttendGift FindAttendGiftEvent()
    {
        dynamic result = QueryFactory.Query("event_attend_gift").FirstOrDefault();
        return ReadAttendGiftEvent(result);
    }

    public RPS FindRockPaperScissorsEvent()
    {
        dynamic result = QueryFactory.Query("event_rockpaperscissors").FirstOrDefault();
        return ReadRockPaperScissorsEvent(result);
    }

    public SaleChat FindSaleChatEvent()
    {
        dynamic result = QueryFactory.Query("event_sale_chat").FirstOrDefault();
        return ReadSaleChatEvent(result);
    }

    public TrafficOptimizer FindTrafficOptimizer()
    {
        dynamic result = QueryFactory.Query("event_traffic_optimizer").FirstOrDefault();
        return ReadTrafficOptimizer(result);
    }

    public List<GameEvent> FindAll()
    {
        List<GameEvent> gameEvents = new();

        gameEvents.AddRange(FindAllStringBoardEvent());
        gameEvents.Add(FindStringBoardLinkEvent());
        gameEvents.Add(FindFieldPopupEvent());
        gameEvents.Add(FindMapleopolyEvent());
        gameEvents.Add(FindUgcMapExtensionSaleEvent());
        gameEvents.Add(FindUgcMapContractSaleEvent());
        gameEvents.Add(FindAttendGiftEvent());
        gameEvents.Add(FindRockPaperScissorsEvent());
        gameEvents.Add(FindMeratMarketNotice());
        gameEvents.Add(FindSaleChatEvent());
        gameEvents.Add(FindTrafficOptimizer());

        return gameEvents;
    }

    private static dynamic ReadBaseGameEvent(int eventId)
    {
        return QueryFactory.Query("events").Where("id", eventId).FirstOrDefault();
    }

    private static StringBoard ReadStringBoardEvent(dynamic data)
    {
        dynamic baseEvent = ReadBaseGameEvent((int) data.game_event_id);
        return new StringBoard(data.game_event_id, data.message_id, data.message, baseEvent.begin_timestamp, baseEvent.end_timestamp);
    }

    private static StringBoardLink ReadStringBoardLinkEvent(dynamic data)
    {
        dynamic baseEvent = ReadBaseGameEvent((int) data.game_event_id);
        return new StringBoardLink(data.game_event_id, data.link, baseEvent.begin_timestamp, baseEvent.end_timestamp);
    }

    private static MeratMarketNotice ReadMeratMarketNoticeEvent(dynamic data)
    {
        dynamic baseEvent = ReadBaseGameEvent((int) data.game_event_id);
        return new MeratMarketNotice(data.game_event_id, data.message, baseEvent.begin_timestamp, baseEvent.end_timestamp);
    }

    private static EventFieldPopup ReadFieldPopupEvent(dynamic data)
    {
        dynamic baseEvent = ReadBaseGameEvent((int) data.game_event_id);
        return new EventFieldPopup(data.game_event_id, data.map_id, baseEvent.begin_timestamp, baseEvent.end_timestamp);
    }

    private static BlueMarble ReadMapleopolyEvent(IEnumerable<dynamic> data)
    {
        List<BlueMarbleReward> rewards = new();
        IEnumerable<dynamic> dataList = data.ToList();
        dynamic baseEvent = ReadBaseGameEvent((int) dataList.First().game_event_id);
        foreach (dynamic item in dataList)
        {
            BlueMarbleReward reward = new(item.trip_amount, item.item_id, item.item_rarity, item.item_amount);
            rewards.Add(reward);
        }

        return new BlueMarble(baseEvent.id, rewards, baseEvent.begin_timestamp, baseEvent.end_timestamp);
    }

    private static UgcMapContractSale ReadUgcMapContractSaleEvent(dynamic data)
    {
        dynamic baseEvent = ReadBaseGameEvent((int) data.game_event_id);
        return new UgcMapContractSale(data.game_event_id, data.discount_amount, baseEvent.begin_timestamp, baseEvent.end_timestamp);
    }

    private static UgcMapExtensionSale ReadUgcMapExtensionSaleEvent(dynamic data)
    {
        dynamic baseEvent = ReadBaseGameEvent((int) data.game_event_id);
        return new UgcMapExtensionSale(data.game_event_id, data.discount_amount, baseEvent.begin_timestamp, baseEvent.end_timestamp);
    }

    private static AttendGift ReadAttendGiftEvent(dynamic data)
    {
        dynamic baseEvent = ReadBaseGameEvent((int) data.game_event_id);
        return new AttendGift(data.game_event_id, baseEvent.begin_timestamp, baseEvent.end_timestamp, data.name, data.url, data.disable_claim_button,
            data.time_required,
            (GameEventCurrencyType) data.skip_day_currency_type, data.skip_days_allowed, data.skip_day_cost,
            JsonConvert.DeserializeObject<List<AttendGiftDay>>(data.days));
    }

    private static RPS ReadRockPaperScissorsEvent(dynamic data)
    {
        dynamic baseEvent = ReadBaseGameEvent((int) data.game_event_id);
        return new RPS(data.game_event_id, data.voucher_id, baseEvent.begin_timestamp, baseEvent.end_timestamp,
            JsonConvert.DeserializeObject<List<RPSTier>>(data.rewards));
    }

    private static SaleChat ReadSaleChatEvent(dynamic data)
    {
        dynamic baseEvent = ReadBaseGameEvent((int) data.game_event_id);
        return new SaleChat(data.game_event_id, baseEvent.begin_timestamp, baseEvent.end_timestamp, data.world_chat_discount_amount,
            data.channel_chat_discount_amount);
    }

    private static TrafficOptimizer ReadTrafficOptimizer(dynamic data)
    {
        dynamic baseEvent = ReadBaseGameEvent((int) data.game_event_id);
        return new TrafficOptimizer(data.game_event_id, baseEvent.begin_timestamp, baseEvent.end_timestamp, data.guide_object_sync_interval_ms,
            data.ride_sync_interval_ms, data.linear_movement_interval_ms, data.user_sync_interval_ms);
    }
}
