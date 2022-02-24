using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game.Helpers;

public static class GameEventHelper
{
    public static void LoadEvents(Player player)
    {
        List<GameEventUserValue> userValues = DatabaseManager.GameEventUserValue.FindAllUserValuesByCharacterId(player.CharacterId);

        player.EventUserValues = userValues;
        player.Session.Send(GameEventUserValuePacket.LoadValues(userValues));
    }

    public static GameEventUserValue GetUserValue(Player player, int eventId, long expirationTimestamp, GameEventUserValueType type)
    {
        GameEventUserValue userValue = player.EventUserValues.FirstOrDefault(x => x.EventId == eventId && x.EventType == type);
        if (userValue is null)
        {
            string defaultValue = type switch
            {
                GameEventUserValueType.AccumulatedTime or
                GameEventUserValueType.RewardsClaimed or
                GameEventUserValueType.EarlyParticipationRemaining or
                GameEventUserValueType.CompletedTimestamp => "0",
                GameEventUserValueType.Active => "True",
                _ => "0"
            };

            userValue = new(player.CharacterId, type, defaultValue, eventId, expirationTimestamp);
            player.EventUserValues.Add(userValue);
        }
        return userValue;
    }
}
