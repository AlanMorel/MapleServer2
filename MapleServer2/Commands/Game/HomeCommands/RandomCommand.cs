using MapleServer2.Commands.Core;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Managers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Commands.Game.HomeCommands;

public class RandomUserCommand : InGameCommand
{
    public RandomUserCommand()
    {
        Aliases = new()
        {
            "randomuser"
        };
        Description = "Randomly selects a user in the map.";
        Usage = "randomuser";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        Player player = trigger.Session.Player;
        bool mapIsHome = player.MapId == (int) Map.PrivateResidence;

        if (!mapIsHome)
        {
            return;
        }

        string randomPlayer = trigger.Session.FieldManager.State.Players.Values.ToList().OrderBy(_ => Random.Shared.Next()).FirstOrDefault()?.Value.Name;
        if (randomPlayer is null)
        {
            return;
        }

        trigger.Session.FieldManager.BroadcastPacket(NoticePacket.Notice(SystemNotice.PickRandomCharacterName, NoticeType.Chat, new()
        {
            trigger.Session.Player.Name,
            randomPlayer
        }));
    }
}

public class RandomNumberCommand : InGameCommand
{
    public RandomNumberCommand()
    {
        Aliases = new()
        {
            "roll"
        };
        Description = "Rolls a random number between 1 and 100.";
        Usage = "roll";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        Player player = trigger.Session.Player;
        bool mapIsHome = player.MapId == (int) Map.PrivateResidence;

        DungeonSession? dungeonSession = GameServer.DungeonManager.GetBySessionId(player.DungeonSessionId);
        if (mapIsHome || dungeonSession is not null || trigger.ChatType is ChatType.Party)
        {
            trigger.Session.FieldManager.BroadcastPacket(HomeActionPacket.Roll(trigger.Session.Player, Random.Shared.Next(100)));
            return;
        }

        trigger.Session.FieldManager.BroadcastPacket(NoticePacket.Notice(SystemNotice.UgcMapFunRollError, NoticeType.Chat));
    }
}
