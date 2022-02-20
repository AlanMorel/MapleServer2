using MapleServer2.Database.Types;
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
            gameEvent.Active,
            gameEvent.Type,
            begin_timestamp = gameEvent.BeginTimestamp,
            end_timestamp = gameEvent.EndTimestamp,
        });
    }

    public FieldPopupEvent FindFieldPopupEvent()
    {
        GameEvent gameEvent = QueryFactory.Query(TableName).Where(new
        {
            type = GameEventType.EventFieldPopup,
            active = true
        }).Get<GameEvent>().FirstOrDefault();
        if (gameEvent == null)
        {
            return null;
        }
        return ReadFieldPopupEvent(QueryFactory.Query("event_field_popup").Where("game_event_id", gameEvent.Id).Get().FirstOrDefault());
    }

    public List<MapleopolyEvent> FindAllMapleopolyEvents()
    {
        List<MapleopolyEvent> mapleopolyEvents = new();
        GameEvent gameEvent = QueryFactory.Query(TableName).Where(new
        {
            type = GameEventType.BlueMarble,
            active = true
        }).Get<GameEvent>().FirstOrDefault();
        if (gameEvent == null)
        {
            return null;
        }
        IEnumerable<dynamic> results = QueryFactory.Query("event_mapleopoly").Where("game_event_id", gameEvent.Id).Get();
        foreach (dynamic result in results)
        {
            mapleopolyEvents.Add(ReadMapleopolyEvent(result));
        }
        return mapleopolyEvents;
    }

    public UgcMapContractSaleEvent FindUgcMapContractSaleEvent()
    {
        GameEvent gameEvent = QueryFactory.Query(TableName).Where(new
        {
            type = GameEventType.UgcMapContractSale,
            active = true
        }).Get<GameEvent>().FirstOrDefault();
        if (gameEvent == null)
        {
            return null;
        }
        return ReadUgcMapContractSaleEvent(QueryFactory.Query("event_ugc_map_contract_sale").Where("game_event_id", gameEvent.Id).Get().FirstOrDefault());
    }

    public UgcMapExtensionSaleEvent FindUgcMapExtensionSaleEvent()
    {
        GameEvent gameEvent = QueryFactory.Query(TableName).Where(new
        {
            type = GameEventType.UgcMapExtensionSale,
            active = true
        }).Get<GameEvent>().FirstOrDefault();
        if (gameEvent == null)
        {
            return null;
        }
        return ReadUgcMapExtensionSaleEvent(QueryFactory.Query("event_ugc_map_extension_sale").Where("game_event_id", gameEvent.Id).Get().FirstOrDefault());
    }

    public List<StringBoardEvent> FindAllStringBoardEvent()
    {
        List<StringBoardEvent> stringBoardEvents = new();
        GameEvent gameEvent = QueryFactory.Query(TableName).Where(new
        {
            type = GameEventType.StringBoard,
            active = true
        }).Get<GameEvent>().FirstOrDefault();
        if (gameEvent == null)
        {
            return null;
        }
        IEnumerable<dynamic> results = QueryFactory.Query("event_string_boards").Where("game_event_id", gameEvent.Id).Get();
        foreach (dynamic data in results)
        {
            stringBoardEvents.Add(ReadStringBoardEvent(data));
        }
        return stringBoardEvents;
    }

    public List<GameEvent> FindAll()
    {
        List<GameEvent> gameEvents = QueryFactory.Query(TableName).Where("active", true).Get<GameEvent>().ToList();
        foreach (GameEvent gameEvent in gameEvents)
        {
            switch (gameEvent.Type)
            {
                case GameEventType.StringBoard:
                    gameEvent.StringBoard = FindAllStringBoardEvent();
                    break;
                case GameEventType.BlueMarble:
                    gameEvent.Mapleopoly = FindAllMapleopolyEvents();
                    break;
                case GameEventType.UgcMapContractSale:
                    gameEvent.UgcMapContractSale = FindUgcMapContractSaleEvent();
                    break;
                case GameEventType.UgcMapExtensionSale:
                    gameEvent.UgcMapExtensionSale = FindUgcMapExtensionSaleEvent();
                    break;
                case GameEventType.EventFieldPopup:
                    gameEvent.FieldPopupEvent = FindFieldPopupEvent();
                    break;
            }
        }
        return gameEvents;
    }

    private static StringBoardEvent ReadStringBoardEvent(dynamic data)
    {
        return new StringBoardEvent(data.id, data.message_id, data.message);
    }

    private static FieldPopupEvent ReadFieldPopupEvent(dynamic data)
    {
        return new FieldPopupEvent(data.id, data.map_id);
    }

    private static MapleopolyEvent ReadMapleopolyEvent(dynamic data)
    {
        return new MapleopolyEvent(data.id, data.trip_amount, data.item_id, data.item_rarity, data.item_amount);
    }

    private static UgcMapContractSaleEvent ReadUgcMapContractSaleEvent(dynamic data)
    {
        return new UgcMapContractSaleEvent(data.id, data.discount_amount);
    }

    private static UgcMapExtensionSaleEvent ReadUgcMapExtensionSaleEvent(dynamic data)
    {
        return new UgcMapExtensionSaleEvent(data.id, data.discount_amount);
    }


}
