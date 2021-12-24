using MapleServer2.Commands.Core;
using MapleServer2.Enums;
using MapleServer2.Packets;

namespace MapleServer2.Commands.Game.HomeCommands;

public class GravityCommand : InGameCommand
{
    public GravityCommand()
    {
        Aliases = new()
        {
            "hostgravity"
        };
        Usage = "hostgravity [value]";
        Description = "Changes the gravity of the map.";
        Parameters = new()
        {
            new Parameter<float>("gravity", "The new gravity value.")
        };
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        int gravity = (int) Math.Round(trigger.Get<float>("gravity"));

        gravity = Math.Min(gravity * 40, 400);
        if (gravity < 0)
        {
            gravity = 0;
        }

        trigger.Session.FieldManager.BroadcastPacket(FieldPropertyPacket.ChangeGravity(gravity * -1));
        trigger.Session.FieldManager.BroadcastPacket(NoticePacket.Notice(SystemNotice.GravityChanged, NoticeType.Chat));
    }
}
