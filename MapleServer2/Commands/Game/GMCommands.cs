using System.Drawing;
using MapleServer2.Commands.Core;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Types;

namespace MapleServer2.Commands.Game;

public class OneShotCommand : InGameCommand
{
    public OneShotCommand()
    {
        Aliases = new()
        {
            "oneshot"
        };
        Description = "Enables oneshot mode.";
        Usage = "/oneshot";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        if (trigger.Session.Player.GmFlags.Contains("oneshot"))
        {
            trigger.Session.Player.GmFlags.Remove("oneshot");
            trigger.Session.SendNotice("Oneshot mode disabled.");
            return;
        }

        trigger.Session.Player.GmFlags.Add("oneshot");
        trigger.Session.SendNotice("Oneshot mode enabled.");
    }
}
public class SetJobCommand : InGameCommand
{
    public SetJobCommand()
    {
        Aliases = new()
        {
            "setJob",
            "sj"
        };
        Description = "Sets character's job.";
        Parameters = new()
        {
            new Parameter<string>("job", "Classname, e.g.: striker"),
            new Parameter<byte>("awakened", "Awakened = 1, Unawakened = 0")
        };
        Usage = "/setJob [job] [awakened]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        string jobName = trigger.Get<string>("job");
        byte awakened = trigger.Get<byte>("awakened");

        Player player = trigger.Session.Player;
        IFieldObject<Player> fieldPlayer = trigger.Session.FieldPlayer;

        long activeSkillTabId = player.ActiveSkillTabId;
        SkillTab skillTab = player.SkillTabs.First(tab => tab.TabId == activeSkillTabId);

        if (string.IsNullOrEmpty(jobName))
        {
            string[] classes = Enum.GetNames(typeof(Job));

            player.Session.Send(NoticePacket.Notice($"You have to give a classname and specifiy awakening (1 or 0)\nAvailable classes:\n".Bold().Color(Color.DarkOrange) +
                                                    $"{string.Join(", ", classes).Color(Color.Aquamarine)}", NoticeType.Chat));

            return;
        }

        Job job = Job.None;
        if (!Enum.TryParse(jobName, true, out job))
        {

            player.Session.SendNotice($"{jobName} is not a valid class name");
            return;
        }

        if (job == Job.None)
        {
            player.Session.SendNotice("None is not a valid class");
            return;
        }

        if (job != player.Job)
        {
            DatabaseManager.SkillTabs.Delete(skillTab.Uid);
            SkillTab newSkillTab = new(player.CharacterId, job, skillTab.TabId, skillTab.Name);

            player.SkillTabs[player.SkillTabs.IndexOf(skillTab)] = newSkillTab;
            player.Job = job;
        }

        player.Awakened = awakened == 1;

        trigger.Session.Send(JobPacket.SendJob(fieldPlayer));
    }
}
