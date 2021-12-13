using System.Text;
using MapleServer2.Commands.Core;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
namespace MapleServer2.Commands.Game.HomeCommands;

public class SurveyCommand : InGameCommand
{
    public SurveyCommand()
    {
        Aliases = new()
        {
            "survey"
        };
        Description = "Create a survey";
        Parameters = new()
        {
            new Parameter<string[]>("options", "Options")
        };
        Usage = "/survey";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        Player player = trigger.Session.Player;
        bool mapIsHome = player.MapId == (int) Map.PrivateResidence;

        if (!mapIsHome)
        {
            return;
        }

        Home home = GameServer.HomeManager.GetHomeById(player.VisitingHomeId);
        if (home.AccountId != player.AccountId)
        {
            return;
        }

        string[] args = trigger.Get<string[]>("options");

        if (args is null)
        {
            trigger.Session.Send(HomeActionPacket.SurveyMessage());
            return;
        }

        string[] options = args[1..];
        if (options.Length == 0)
        {
            trigger.Session.Send(HomeActionPacket.SurveyMessage());
            return;
        }

        string firstOption = options[0].ToLower();
        switch (firstOption)
        {
            // Init survey as open (show who voted) or secret (no one can see who voted)
            case "open" or "secret":
                {
                    StringBuilder sb = new();
                    foreach (string str in options[1..])
                    {
                        sb.Append($"{str} ");
                    }

                    string question = sb.ToString().Trim();
                    if (string.IsNullOrEmpty(question))
                    {
                        return;
                    }

                    home.Survey = new(question, firstOption is "open");

                    trigger.Session.Send(HomeActionPacket.SurveyQuestion(home.Survey));
                    return;
                }
            case "add":
                {
                    if (home.Survey.Question is null || home.Survey.Ended)
                    {
                        return;
                    }

                    StringBuilder sb = new();
                    foreach (string str in options[1..])
                    {
                        sb.Append($"{str} ");
                    }

                    string option = sb.ToString().Trim();
                    if (string.IsNullOrEmpty(option))
                    {
                        return;
                    }

                    home.Survey.Options.Add(option, new());

                    trigger.Session.Send(HomeActionPacket.SurveyAddOption(home.Survey));
                    return;
                }
            case "start":
                if (home.Survey.Started || home.Survey.Ended)
                {
                    return;
                }

                home.Survey.Start(player.CharacterId, trigger.Session.FieldManager.State.Players.Values.ToList());
                trigger.Session.FieldManager.BroadcastPacket(HomeActionPacket.SurveyStart(home.Survey));
                return;
            case "end":
                if (!home.Survey.Started || home.Survey.Ended)
                {
                    return;
                }

                trigger.Session.FieldManager.BroadcastPacket(HomeActionPacket.SurveyEnd(home.Survey));
                home.Survey.End();
                return;
        }
    }
}
