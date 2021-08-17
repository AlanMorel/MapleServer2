using MapleServer2.Commands.Core;
using MapleServer2.Enums;

namespace MapleServer2.Commands.Game
{
    public class HandicraftCommand : InGameCommand
    {
        public HandicraftCommand()
        {
            Aliases = new string[]
            {
                "sethandicraft"
            };
            Description = "Set Exp for handicraft.";
            AddParameter<int>("exp", "Amount of exp for handicraft.");
        }

        public override void Execute(GameCommandTrigger trigger)
        {
            int exp = trigger.Get<int>("exp");

            if (exp > 0)
            {
                trigger.Session.Player.Levels.GainMasteryExp(MasteryType.Handicraft, exp);
                return;
            }
            trigger.Session.SendNotice("Exp must be a number or more than 0.");
        }
    }

    public class PrestigeExpCommand : InGameCommand
    {
        public PrestigeExpCommand()
        {
            Aliases = new[]
            {
                "gainprestigeexp"
            };
            Description = "Set Exp for prestige.";
            AddParameter<long>("exp", "Amount of exp for prestige");
        }

        public override void Execute(GameCommandTrigger trigger)
        {
            long exp = trigger.Get<long>("exp");

            if (exp > 0)
            {
                trigger.Session.Player.Levels.GainPrestigeExp(exp);
                return;
            }
            trigger.Session.SendNotice("Exp must be a number or more than 0.");
        }
    }

    public class GainExpCommand : InGameCommand
    {
        public GainExpCommand()
        {
            Aliases = new[]
            {
                "gainexp"
            };
            Description = "Give Exp for the Player";
            AddParameter<int>("exp", "Amount of exp for player");
        }

        public override void Execute(GameCommandTrigger trigger)
        {
            int exp = trigger.Get<int>("exp");

            if (exp > 0)
            {
                trigger.Session.Player.Levels.GainExp(exp);
                return;
            }
            trigger.Session.SendNotice("Exp must be a number or more than 0.");
        }
    }

    public class PrestigeLevelCommand : InGameCommand
    {
        public PrestigeLevelCommand()
        {
            Aliases = new[]
            {
                "setprestigelevel"
            };
            Description = "Set the level for prestige";
            AddParameter<int>("level", "The number of level for prestige");
        }

        public override void Execute(GameCommandTrigger trigger)
        {
            int level = trigger.Get<int>("level");

            if (level > 0)
            {
                trigger.Session.Player.Levels.SetPrestigeLevel(level);
                return;
            }
            trigger.Session.SendNotice("Level must be a number or more than 0.");
        }
    }

    public class LevelCommand : InGameCommand
    {
        public LevelCommand()
        {
            Aliases = new[]
            {
                "setlevel"
            };
            Description = "Set the Player level.";
            AddParameter<short>("level", "The number of the level to get.");
        }

        public override void Execute(GameCommandTrigger trigger)
        {
            short level = trigger.Get<short>("level");

            if (level > 0)
            {
                trigger.Session.Player.Levels.SetLevel(level);
                return;
            }
            trigger.Session.SendNotice("Level must be a number or more than 0.");
        }
    }
}
