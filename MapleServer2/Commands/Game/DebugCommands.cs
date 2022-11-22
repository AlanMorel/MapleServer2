using MapleServer2.Commands.Core;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Commands.Game;

public class DungeonTests : InGameCommand
{
    public DungeonTests()
    {
        Aliases = new()
        {
            "dt"
        };
        Description = "dungeon test";
        Parameters = new()
        {
        };
        Usage = "";
    }

    public override void Execute(GameCommandTrigger trigger)
    {

        trigger.Session.SendNotice($"player DS {trigger.Session.Player.DungeonSessionId} InstanceId: {trigger.Session.Player.InstanceId}");

        if (trigger.Session.Player.Party is not null)
        {
            DungeonSession dungeonSession =
                GameServer.DungeonManager.GetBySessionId(trigger.Session.Player.Party.DungeonSessionId);

            trigger.Session.SendNotice($"party DS {trigger.Session.Player.Party?.DungeonSessionId} IsCompleted {dungeonSession.IsCompleted}");

            dungeonSession.IsCompleted = true;

            trigger.Session.SendNotice($"party DS {trigger.Session.Player.Party?.DungeonSessionId} IsCompleted {dungeonSession.IsCompleted} IsReset {dungeonSession.IsReset}");
            return;
        }

        trigger.Session.SendNotice($"No party for {trigger.Session.Player.Name}");
    }
}
