using MapleServer2.Commands.Core;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;

namespace MapleServer2.Commands.Game;

public class HandicraftCommand : InGameCommand
{
    public HandicraftCommand()
    {
        Aliases = new()
        {
            "sethandicraft"
        };
        Description = "Set Exp for handicraft.";
        Parameters = new()
        {
            new Parameter<int>("exp", "Amount of exp for handicraft.")
        };
        Usage = "/sethandicraft [exp]";
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
        Aliases = new()
        {
            "gainprestigeexp"
        };
        Description = "Set Exp for prestige.";
        Parameters = new()
        {
            new Parameter<long>("exp", "Amount of exp for prestige")
        };
        Usage = "/gainprestigeexp [exp]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        long exp = trigger.Get<long>("exp");

        if (exp > 0)
        {
            trigger.Session.Player.Account.Prestige.GainExp(trigger.Session, exp);
            return;
        }

        trigger.Session.SendNotice("Exp must be a number or more than 0.");
    }
}

public class GainExpCommand : InGameCommand
{
    public GainExpCommand()
    {
        Aliases = new()
        {
            "gainexp"
        };
        Description = "Give Exp for the Player";
        Parameters = new()
        {
            new Parameter<int>("exp", "Amount of exp for player")
        };
        Usage = "/gainexp [exp]";
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
        Aliases = new()
        {
            "setprestigelevel"
        };
        Description = "Set the level for prestige";
        Parameters = new()
        {
            new Parameter<int>("level", "The number of level for prestige")
        };
        Usage = "/setprestigelevel [level]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        int level = trigger.Get<int>("level");

        if (level > 0)
        {
            trigger.Session.Player.Account.Prestige.SetLevel(trigger.Session, level);
            return;
        }

        trigger.Session.SendNotice("Level must be a number or more than 0.");
    }
}

public class LevelCommand : InGameCommand
{
    public LevelCommand()
    {
        Aliases = new()
        {
            "setlevel"
        };
        Description = "Set the Player level.";
        Parameters = new()
        {
            new Parameter<short>("level", "The number of the level to get.")
        };
        Usage = "/setlevel [level]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        short level = trigger.Get<short>("level");

        if (level <= 0)
        {
            trigger.Session.SendNotice("Level must be a number or more than 0.");
            return;
        }

        Player player = trigger.Session.Player;

        // Reset stats to default
        player.Stats = new(player.JobCode);
        player.Stats.AddBaseStats(player, level);

        trigger.Session.Send(StatPacket.SetStats(player.FieldPlayer));
        trigger.Session.FieldManager.BroadcastPacket(StatPacket.SetStats(player.FieldPlayer), trigger.Session);

        player.Levels.SetLevel(level);
    }
}

public class LevelUpCommand : InGameCommand
{
    public LevelUpCommand()
    {
        Aliases = new()
        {
            "levelup"
        };
        Description = "Level up.";
        Parameters = new();
        Usage = "/levelup";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        Levels levels = trigger.Session.Player.Levels;
        levels.GainExp(ExpMetadataStorage.GetExpToLevel(levels.Level) - levels.Exp);
    }
}

public class SkillCommand : InGameCommand
{
    public SkillCommand()
    {
        Aliases = new()
        {
            "skill"
        };
        Description = "Cast an specific skill.";
        Parameters = new()
        {
            new Parameter<int>("id", "ID of the skill."),
            new Parameter<short>("level", "The level of the skill.")
        };
        Usage = "/skill <id> [level]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        GameSession gameSession = trigger.Session;
        IFieldActor<Player> player = gameSession.Player.FieldPlayer;

        int id = trigger.Get<int>("id");
        short level = trigger.Get<short>("level") > 0 ? trigger.Get<short>("level") : (short) 1;

        if (SkillMetadataStorage.GetSkill(id) is null)
        {
            gameSession.SendNotice($"Skill with id: {id} is not defined.");
            return;
        }

        SkillCast skillCast = new(id, level, GuidGenerator.Long(), gameSession.ServerTick, player, gameSession.ClientTick)
        {
            Position = player.Coord,
            Direction = default,
            Rotation = default
        };

        gameSession.FieldManager.BroadcastPacket(SkillUsePacket.SkillUse(skillCast));
    }
}

public class BuffCommand : InGameCommand
{
    public BuffCommand()
    {
        Aliases = new()
        {
            "buff"
        };
        Description = "Level up all the skills available.";
        Parameters = new()
        {
            new Parameter<int>("id", "ID of the status."),
            new Parameter<short>("level", "The level of the status. (OPTIONAL)"),
            new Parameter<int>("duration", "Duration for the status in seconds. (OPTIONAL)"),
            new Parameter<int>("stacks", "Stacks for the status. (OPTIONAL)")
        };
        Usage = "/buff [id] [level] [duration] [stacks]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        int id = trigger.Get<int>("id");
        short level = trigger.Get<short>("level") <= 10 && trigger.Get<short>("level") != 0 ? trigger.Get<short>("level") : (short) 1;
        if (AdditionalEffectMetadataStorage.GetLevelMetadata(id, level) == null)
        {
            trigger.Session.SendNotice($"No skill found with id: {id}");
            return;
        }

        // The Status packet needs this in miliseconds, we are converting them here for the user to just input the actual seconds.
        int duration = trigger.Get<int>("duration") <= 3600 && trigger.Get<int>("duration") != 0 ? trigger.Get<int>("duration") * 1000 : 10000;
        int stacks = trigger.Get<int>("stacks") == 0 ? 1 : trigger.Get<int>("stacks");

        trigger.Session.Player.AdditionalEffects.AddEffect(new(id, level)
        {
            Stacks = stacks,
        });
    }
}
