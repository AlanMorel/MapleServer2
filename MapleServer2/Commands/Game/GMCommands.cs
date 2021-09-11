﻿using System.Drawing;
using MapleServer2.Commands.Core;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Types;

namespace MapleServer2.Commands.Game
{
    public class OneShotCommand : InGameCommand
    {
        public OneShotCommand()
        {
            Aliases = new()
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
            Aliases = new()
            {
                "setJob",
                "sj"
            };
            Description = "Sets character's job, e.g. /setJob assassin 1";
            AddParameter<string>("job", "Classname, e.g.: striker");
            AddParameter<byte>("awakened", "Awakened = 1, Unawakened = 0");
        }

        public override void Execute(GameCommandTrigger trigger)
        {

            string jobName = trigger.Get<string>("job");
            byte awakened = trigger.Get<byte>("awakened");

            Player player = trigger.Session.Player;
            IFieldObject<Player> fieldPlayer = trigger.Session.FieldPlayer;

            long activeSkillTabId = player.ActiveSkillTabId;
            SkillTab skillTab = player.SkillTabs.First(tab => tab.TabId == activeSkillTabId);

            Job job = Job.None;


            if (!Enum.TryParse(jobName, true, out job))
            {
                player.Session.SendNotice($"{jobName} is not a valid class name");
                return;
            }

            if (Job.None == job)
            {
                string[] classes = Enum.GetNames(typeof(Job));

                player.Session.Send(NoticePacket.Notice($"{CommandHelpers.Color(CommandHelpers.Bold("You have to give a classname and specifiy awakening (1 or 0)\nAvailable classes:\n"), Color.DarkOrange)}" +
                    $"{CommandHelpers.Color(string.Join(", ", classes), Color.Aquamarine)}", NoticeType.Chat));

                return;
            }

            JobCode jobCode = (JobCode) ((int) job * 10 + awakened);
            player.JobCode = jobCode;

            if (job != Job.None && job != player.Job)
            {
                DatabaseManager.SkillTabs.Delete(skillTab.Uid);
                SkillTab newSkillTab = new SkillTab(player.CharacterId, job, skillTab.TabId, skillTab.Name);

                player.SkillTabs[player.SkillTabs.IndexOf(skillTab)] = newSkillTab;
                player.Job = job;
            }
            trigger.Session.Send(Packets.JobPacket.SendJob(fieldPlayer));
        }

    }
}
