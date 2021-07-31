using MapleServer2.Commands.Core;

namespace MapleServer2.Commands.Game
{
    public class OneShotCommand : InGameCommand
    {
        public OneShotCommand()
        {
            Aliases = new[]
            {
                "oneshot"
            };
            Description = "Enables oneshot mode.";
        }

        public override void Execute(GameCommandTrigger trigger)
        {
            if (trigger.Session.Player.GmFlags.Contains("oneshot"))
            {
                trigger.Session.Player.GmFlags.Remove("oneshot");
                trigger.Session.SendNotice("Oneshot mode disabled.");
            }
            else
            {
                trigger.Session.Player.GmFlags.Add("oneshot");
                trigger.Session.SendNotice("Oneshot mode enabled.");
            }
        }
    }
}
