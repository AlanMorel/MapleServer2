using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;

namespace MapleServer2.Types;

public class GameEventUserValue
{
    public long CharacterId;
    public GameEventUserValueType Type;
    public string EventValue;
    public int EventId;
    public long ExpirationTimestamp;

    public GameEventUserValue(long characterId, GameEventUserValueType type, string eventValue, int eventId, long expirationTimestamp)
    {
        CharacterId = characterId;
        Type = type;
        EventValue = eventValue;
        EventId = eventId;
        ExpirationTimestamp = expirationTimestamp;
        DatabaseManager.GameEventUserValue.Insert(this);
    }
}
