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
            new Parameter<bool>("isCompleted", "if true is given for this parameter, the dungeon session will be set to be completed")
        };
        Usage = "/dt [setIsCompleted?]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {

        trigger.Session.SendNotice($"player DS {trigger.Session.Player.DungeonSessionId} InstanceId: {trigger.Session.Player.InstanceId}");

        if (trigger.Session.Player.Party is not null)
        {
            bool setIsCompleted = trigger.Get<bool>("isCompleted");
            DungeonSession? dungeonSession = GameServer.DungeonManager.GetBySessionId(trigger.Session.Player.Party.DungeonSessionId);

            if (dungeonSession == null)
            {
                return;
            }

            trigger.Session.SendNotice($"party DS {trigger.Session.Player.Party?.DungeonSessionId} IsCompleted {dungeonSession.IsCompleted} IsReset {dungeonSession.IsReset}");

            if (!setIsCompleted)
            {
                return;
            }

            dungeonSession.IsCompleted = true;
            trigger.Session.SendNotice($"Set IsCompleted: IsCompleted {dungeonSession.IsCompleted}");
            return;
        }
        trigger.Session.SendNotice($"No party for {trigger.Session.Player.Name}");
    }
}
