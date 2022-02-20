using Maple2Storage.Enums;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game.Helpers;

public static class GameEventHelper
{
    public static void LoadEvents(Player player, List<GameEvent> gameEvents)
    {
        List<GameEventUserValue> userValues = DatabaseManager.GameEventUserValue.FindAllUserValuesByCharacterId(player.CharacterId);
        // will check if values are in player yet
        // if not, create them
        // create => make for attendance event for now.
        foreach(GameEvent gameEvent in gameEvents)
        {
            switch (gameEvent.Type)
            {
                case GameEventType.AttendGift:
                    if (!userValues.Any(x => x.EventId == gameEvent.Id && x.Type == GameEventUserValueType.AccumulatedTime))
                    {
                        GameEventUserValue timeAccumulation = new GameEventUserValue(player.CharacterId, GameEventUserValueType.AccumulatedTime,
                            "0", gameEvent.Id, gameEvent.EndTimestamp);
                        userValues.Add(timeAccumulation);
                    }
                    break;


            }
        }
    }
}
