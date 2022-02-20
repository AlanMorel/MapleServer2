﻿using MapleServer2.Enums;
using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseGameEventUserValue : DatabaseTable
{
    public DatabaseGameEventUserValue() : base("game_event_user_values") { }

    public long Insert(GameEventUserValue value)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            character_id = value.CharacterId,
            type = value.Type,
            event_value = value.EventValue,
            event_id = value.EventId,
            expiration_timestamp = value.ExpirationTimestamp,
        });
    }

    public List<GameEventUserValue> FindAllUserValuesByCharacterId(long characterId)
    {
        IEnumerable<dynamic> result = QueryFactory.Query(TableName).Where("character_id", characterId).Get();
        List<GameEventUserValue> values = new();
        foreach (dynamic entry in result)
        {
            values.Add(ReadGameEventUserValue(entry));
        }
        return values;
    }

    public bool Delete(long characterId, int eventId)
    {
        return QueryFactory.Query(TableName).Where(new
        {
            character_id = characterId,
            event_id = eventId
        }).Delete() == 1;
    }

    private static GameEventUserValue ReadGameEventUserValue(dynamic data)
    {
        return new GameEventUserValue(data.character_id, (GameEventUserValueType) data.type, data.event_value, data.event_id, data.expiration_timestamp);
    }
}
