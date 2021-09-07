using MapleServer2.Commands.Core;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Types;

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
            Aliases = new[]
            {
                "setJob",
                "sj"
            };
            Description = "Sets job";
            AddParameter<string>("job", "Classname, e.g.: striker");
            AddParameter<byte>("awakened", "Awakened = 1, Unawakened = 0");
        }

        public override void Execute(GameCommandTrigger trigger)
        {
            Player player = trigger.Session.Player;
            string jobName = trigger.Get<string>("job");

            player.Awakened = trigger.Get<byte>("awakened") == 1;

            IFieldObject<Player> fieldPlayer = trigger.Session.FieldPlayer;
            long activeSkillTabId = player.ActiveSkillTabId;
            SkillTab skillTab = player.SkillTabs.First(tab => tab.TabId == activeSkillTabId);

            Job jobValue = Job.None;
            if (Enum.TryParse(jobName, out jobValue))
            {
                if (Enum.IsDefined(typeof(Job), jobValue) | jobValue.ToString().Contains(","))
                {
                    Console.WriteLine("Converted {0} to {1}", jobName, jobValue.ToString());

                    //if (jobName.ToLower().Contains("gamemaster"))
                    //{
                    //    jobCode = JobCode.GameMaster;
                    //}
                }
                else 
                { }
                    Console.WriteLine("{0} is not an underlying value of the Job enumeration.", jobName);
            }
            else
                Console.WriteLine("{0} is not a member of the Job enumeration", jobName);

            if (jobValue != Job.None)
            {
                DatabaseManager.SkillTabs.Delete(skillTab.Uid);
                SkillTab newSkillTab = new SkillTab(player.CharacterId, jobValue, skillTab.TabId, skillTab.Name);
                
                player.SkillTabs[player.SkillTabs.IndexOf(skillTab)] = newSkillTab;
                player.Job = jobValue;
            }

            trigger.Session.Send(Packets.JobPacket.SendJob(fieldPlayer));
        }

    }
}
