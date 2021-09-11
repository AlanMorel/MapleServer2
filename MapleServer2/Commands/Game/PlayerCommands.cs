using MapleServer2.Data.Static;
using MapleServer2.Commands.Core;
using MapleServer2.Enums;
using MapleServer2.Types;
using MapleServer2.Tools;
using MapleServer2.Packets;
using Maple2Storage.Types;

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

    public class SkillCommand : InGameCommand
    {
        public SkillCommand()
        {
            Aliases = new[]
{
                "skill"
            };
            Description = "Cast an specific skill.";
            AddParameter<int>("id", "ID of the skill.");
            AddParameter<short>("level", "The level of the skill.");
        }

        public override void Execute(GameCommandTrigger trigger)
        {
            int id = trigger.Get<int>("id");
            short level = trigger.Get<short>("level") > 0 ? trigger.Get<short>("level") : (short) 1;
            if (SkillMetadataStorage.GetSkill(id) == null)
            {
                trigger.Session.SendNotice($"Skill with id: {id} is not defined.");
                return;
            }

            SkillCast skillCast = new SkillCast(id, level, GuidGenerator.Long(), trigger.Session.ServerTick);
            CoordF empty = CoordF.From(0, 0, 0);
            IFieldObject<Player> player = trigger.Session.FieldPlayer;

            trigger.Session.FieldManager.BroadcastPacket(SkillUsePacket.SkillUse(skillCast, player.Coord, empty, empty, player.ObjectId));
        }
    }

    public class BuffCommand : InGameCommand
    {
        public BuffCommand()
        {
            Aliases = new[]
{
                "buff"
            };
            Description = "Level up all the skills available.";
            AddParameter<int>("id", "ID of the status.");
            AddParameter<short>("level", "The level of the status. (OPTIONAL)");
            AddParameter<int>("duration", "Duration for the status in seconds. (OPTIONAL)");
            AddParameter<int>("stacks", "Stacks for the status. (OPTIONAL)");
        }

        public override void Execute(GameCommandTrigger trigger)
        {
            int id = trigger.Get<int>("id");
            if (SkillMetadataStorage.GetSkill(id) == null)
            {
                trigger.Session.SendNotice($"No skill found with id: {id}");
                return;
            }

            short level = trigger.Get<short>("level") <= 10 && trigger.Get<short>("level") != 0 ? trigger.Get<short>("level") : (short) 1;
            // The Status packet needs this in miliseconds, we are converting them here for the user to just input the actual seconds.
            int duration = trigger.Get<int>("duration") <= 3600 && trigger.Get<int>("duration") != 0 ? trigger.Get<int>("duration") * 1000 : 10000;
            int stacks = trigger.Get<int>("stacks") == 0 ? 1 : trigger.Get<int>("stacks");

            SkillCast skillCast = new SkillCast(id, level);
            if (skillCast.IsBuffToOwner() || skillCast.IsBuffToEntity() || skillCast.IsBuffShield() || skillCast.IsGM() || skillCast.IsGlobal() || skillCast.IsHealFromBuff())
            {
                Status status = new Status(skillCast, trigger.Session.FieldPlayer.ObjectId, trigger.Session.FieldPlayer.ObjectId, stacks);
                StatusHandler.Handle(trigger.Session, status);
                return;
            }

            trigger.Session.SendNotice($"Skill with id: {id} is not a buff to the owner.");
        }
    }
}
