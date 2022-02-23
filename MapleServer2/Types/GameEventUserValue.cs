using MapleServer2.Database;
using MapleServer2.Enums;

namespace MapleServer2.Types;

public class GameEventUserValue
{
    public long Id;
    public long CharacterId;
    public GameEventUserValueType EventType;
    public string EventValue;
    public int EventId;
    public long ExpirationTimestamp;

    public GameEventUserValue(long characterId, GameEventUserValueType type, string eventValue, int eventId, long expirationTimestamp)
    {
        CharacterId = characterId;
        EventType = type;
        EventValue = eventValue;
        EventId = eventId;
        ExpirationTimestamp = expirationTimestamp;
        Id = DatabaseManager.GameEventUserValue.Insert(this);
    }

    public GameEventUserValue(long id, long characterId, GameEventUserValueType type, string eventValue, int eventId, long expirationTimestamp)
    {
        Id = id;
        CharacterId = characterId;
        EventType = type;
        EventValue = eventValue;
        EventId = eventId;
        ExpirationTimestamp = expirationTimestamp;
    }
}
