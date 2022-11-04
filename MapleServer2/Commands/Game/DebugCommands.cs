using MapleServer2.Commands.Core;
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
            trigger.Session.SendNotice($"party DS {trigger.Session.Player.Party?.DungeonSessionId}");
            return;
        }

        trigger.Session.SendNotice($"No party for {trigger.Session.Player.Name}");
    }
}
