using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseEvent
    {
        private readonly string TableName = "events";

        public long Insert(GameEvent gameEvent)
        {
            return DatabaseManager.QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                gameEvent.Id,
                gameEvent.Active,
                gameEvent.Type
            });
        }

        public FieldPopupEvent FindFieldPopupEvent()
        {
            GameEvent gameEvent = DatabaseManager.QueryFactory.Query(TableName).Where(new
            {
                Type = GameEventType.EventFieldPopup,
                Active = true
            }).Get<GameEvent>().FirstOrDefault();
            if (gameEvent == null)
            {
                return null;
            }
            return DatabaseManager.QueryFactory.Query("event_fieldpopup").Where("GameEventId", gameEvent.Id).Get<FieldPopupEvent>().FirstOrDefault();
        }

        public List<MapleopolyEvent> FindAllMapleopolyEvents()
        {
            GameEvent gameEvent = DatabaseManager.QueryFactory.Query(TableName).Where(new
            {
                Type = GameEventType.BlueMarble,
                Active = true
            }).Get<GameEvent>().FirstOrDefault();
            if (gameEvent == null)
            {
                return null;
            }
            return DatabaseManager.QueryFactory.Query("event_mapleopoly").Where("GameEventId", gameEvent.Id).Get<MapleopolyEvent>().ToList();
        }

        public UGCMapContractSaleEvent FindUGCMapContractSaleEvent()
        {
            GameEvent gameEvent = DatabaseManager.QueryFactory.Query(TableName).Where(new
            {
                Type = GameEventType.UGCMapContractSale,
                Active = true
            }).Get<GameEvent>().FirstOrDefault();
            if (gameEvent == null)
            {
                return null;
            }
            return DatabaseManager.QueryFactory.Query("event_ugcmapcontractsale").Where("GameEventId", gameEvent.Id).Get<UGCMapContractSaleEvent>().FirstOrDefault();
        }

        public UGCMapExtensionSaleEvent FindUGCMapExtensionSaleEvent()
        {
            GameEvent gameEvent = DatabaseManager.QueryFactory.Query(TableName).Where(new
            {
                Type = GameEventType.UGCMapExtensionSale,
                Active = true
            }).Get<GameEvent>().FirstOrDefault();
            if (gameEvent == null)
            {
                return null;
            }
            return DatabaseManager.QueryFactory.Query("event_ugcmapextensionsale").Where("GameEventId", gameEvent.Id).Get<UGCMapExtensionSaleEvent>().FirstOrDefault();
        }

        public List<StringBoardEvent> FindAllStringBoardEvent()
        {
            GameEvent gameEvent = DatabaseManager.QueryFactory.Query(TableName).Where(new
            {
                Type = GameEventType.StringBoard,
                Active = true
            }).Get<GameEvent>().FirstOrDefault();
            if (gameEvent == null)
            {
                return null;
            }
            return DatabaseManager.QueryFactory.Query("event_stringboards").Where("GameEventId", gameEvent.Id).Get<StringBoardEvent>().ToList();
        }

        public List<GameEvent> FindAll()
        {
            List<GameEvent> gameEvents = DatabaseManager.QueryFactory.Query(TableName).Where("Active", true).Get<GameEvent>().ToList();
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
                    case GameEventType.UGCMapContractSale:
                        gameEvent.UGCMapContractSale = FindUGCMapContractSaleEvent();
                        break;
                    case GameEventType.UGCMapExtensionSale:
                        gameEvent.UGCMapExtensionSale = FindUGCMapExtensionSaleEvent();
                        break;
                    case GameEventType.EventFieldPopup:
                        gameEvent.FieldPopupEvent = FindFieldPopupEvent();
                        break;
                }
            }
            return gameEvents;
        }
    }
}
