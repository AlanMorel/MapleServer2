using System.Drawing;
using Maple2.Trigger.Enum;
using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Commands.Core;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Extensions;
using MapleServer2.Managers.Actors;
using MapleServer2.PacketHandlers.Game;
using MapleServer2.PacketHandlers.Game.Helpers;
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
        IFieldObject<Player> fieldPlayer = trigger.Session.Player.FieldPlayer;

        long activeSkillTabId = player.ActiveSkillTabId;
        SkillTab skillTab = player.SkillTabs.First(tab => tab.TabId == activeSkillTabId);

        if (string.IsNullOrEmpty(jobName))
        {
            string[] classes = Enum.GetNames(typeof(Job));

            player.Session.Send(NoticePacket.Notice(
                "You have to give a classname and specifiy awakening (1 or 0)\nAvailable classes:\n".Bold().Color(Color.DarkOrange) +
                $"{string.Join(", ", classes).Color(Color.Aquamarine)}", NoticeType.Chat));

            return;
        }

        if (!Enum.TryParse(jobName, true, out Job job))
        {
            player.Session.SendNotice($"{jobName} is not a valid class name");
            return;
        }

        if (job == Job.None)
        {
            player.Session.SendNotice("None is not a valid class");
            return;
        }

        player.Awakened = awakened == 1;

        if (job != player.Job)
        {
            player.Job = job;
            DatabaseManager.SkillTabs.Delete(skillTab.Uid);

            SkillTab newSkillTab = new(player.CharacterId, job, player.JobCode, skillTab.TabId, skillTab.Name);
            player.SkillTabs[player.SkillTabs.IndexOf(skillTab)] = newSkillTab;
        }

        trigger.Session.Send(JobPacket.SendJob(fieldPlayer));
    }
}

public class NoticeCommand : InGameCommand
{
    public NoticeCommand()
    {
        Aliases = new()
        {
            "testnotice"
        };
        Description = "Sends a notice to the player for testing purposes.";
        Parameters = new()
        {
            new Parameter<int>("noticeId", "The notice id")
        };
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        int noticeId = trigger.Get<int>("noticeId");

        if (!Enum.IsDefined(typeof(SystemNotice), noticeId))
        {
            trigger.Session.SendNotice("Invalid notice id.");
            return;
        }

        trigger.Session.Send(NoticePacket.Notice((SystemNotice) noticeId, NoticeType.Chat));
    }
}

public class AttributeCommand : InGameCommand
{
    public AttributeCommand()
    {
        Aliases = new()
        {
            "attribute"
        };
        Description = "Add attribute to selected item.";
        Parameters = new()
        {
            new Parameter<string>("equipSlot", "Equip slot, e.g.: RH (ItemSlot.cs)"),
            new Parameter<string>("newAttributeId", "New Attribute, e.g.: Dex (StatAttribute.cs)"),
            new Parameter<float>("value", "Value, e.g.: 10 / 0.002"),
            new Parameter<byte>("isPercentage", "Is percentage, e.g.: 1 / 0")
        };
        Usage = "/attribute [equipSlot] [attributeId] [value] [isPercentage]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        string equipSlot = trigger.Get<string>("equipSlot");
        string newAttributeId = trigger.Get<string>("newAttributeId");
        float value = trigger.Get<float>("value");
        byte isPercentage = trigger.Get<byte>("isPercentage");

        if (string.IsNullOrEmpty(equipSlot))
        {
            trigger.Session.SendNotice($"Type '/info {Aliases.First()}' for more details.");
            return;
        }

        if (!Enum.TryParse(equipSlot, out ItemSlot itemSlot) || itemSlot == ItemSlot.NONE)
        {
            trigger.Session.SendNotice($"{equipSlot} is not a valid equip slot.");
            string slots = "";
            foreach (object slot in Enum.GetValues(typeof(ItemSlot)))
            {
                slots += $"{slot} - {((ItemSlot) slot).GetEnumDescription()}, ";
            }

            trigger.Session.SendNotice($"Available slots: {slots.TrimEnd(',', ' ')}");
            return;
        }

        if (!Enum.TryParse(newAttributeId, out StatAttribute newAttribute))
        {
            trigger.Session.SendNotice($"{newAttributeId} is not a valid attribute. Check StatAttribute.cs");
            return;
        }

        if (value == 0)
        {
            trigger.Session.SendNotice("Value cannot be 0.");
            return;
        }

        Player player = trigger.Session.Player;
        if (!player.Inventory.Equips.TryGetValue(itemSlot, out Item item))
        {
            trigger.Session.SendNotice($"You don't have an item in slot {itemSlot}.");
            return;
        }

        ItemStat itemStat;
        StatAttributeType attributeType = isPercentage == 1 ? StatAttributeType.Rate : StatAttributeType.Flat;
        if ((int) newAttribute > 11000)
        {
            itemStat = new SpecialStat(newAttribute, value, attributeType);
        }
        else
        {
            itemStat = new BasicStat(newAttribute, value, attributeType);
        }

        player.DecreaseStats(item);
        item.Stats.Constants[newAttribute] = itemStat;

        trigger.Session.FieldManager.BroadcastPacket(EquipmentPacket.EquipItem(player.FieldPlayer, item, itemSlot));

        player.IncreaseStats(item);

        DatabaseManager.Items.Update(item);
    }
}

public class ClearStatsCommand : InGameCommand
{
    public ClearStatsCommand()
    {
        Aliases = new()
        {
            "clearstats"
        };
        Description = "Removes all stats from selected item.";
        Parameters = new()
        {
            new Parameter<string>("equipSlot", "Equip slot, e.g.: RH (ItemSlot.cs)")
        };
        Usage = "/clearstats [equipSlot]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        string equipSlot = trigger.Get<string>("equipSlot");

        if (string.IsNullOrEmpty(equipSlot))
        {
            trigger.Session.SendNotice($"Type '/info {Aliases.First()}' for more details.");
            return;
        }

        if (!Enum.TryParse(equipSlot, out ItemSlot itemSlot) || itemSlot == ItemSlot.NONE)
        {
            trigger.Session.SendNotice($"{equipSlot} is not a valid equip slot.");
            string slots = "";
            foreach (object slot in Enum.GetValues(typeof(ItemSlot)))
            {
                slots += $"{slot} - {((ItemSlot) slot).GetEnumDescription()}, ";
            }

            trigger.Session.SendNotice($"Available slots: {slots.TrimEnd(',', ' ')}");
            return;
        }

        Player player = trigger.Session.Player;
        if (!player.Inventory.Equips.TryGetValue(itemSlot, out Item item))
        {
            trigger.Session.SendNotice($"You don't have an item in slot {itemSlot}.");
            return;
        }

        player.DecreaseStats(item);

        item.Stats.Constants.Clear();
        item.Stats.Randoms.Clear();
        item.Stats.Statics.Clear();

        trigger.Session.FieldManager.BroadcastPacket(EquipmentPacket.EquipItem(player.FieldPlayer, item, itemSlot));

        DatabaseManager.Items.Update(item);
    }
}

public class GMShopCommand : InGameCommand
{
    public GMShopCommand()
    {
        Aliases = new()
        {
            "gmshop"
        };
        Description = "Opens the GM shop.";
        Usage = "/gmshop";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        ShopHelper.OpenSystemShop(trigger.Session, 999999, 29000307);
    }
}

public class TimeScaleCommand : InGameCommand
{
    public TimeScaleCommand()
    {
        Aliases = new()
        {
            "timescale"
        };
        Description = "Sets the time scale.";
        Parameters = new()
        {
            new Parameter<byte>("enable", "Enabled, e.g.: 1 (true) / 0 (false)"),
            new Parameter<float>("startScale", "Start scale, e.g.: 0.0 ~ 1.0"),
            new Parameter<float>("endScale", "End scale, e.g.: 0.0 ~ 1.0"),
            new Parameter<float>("duration", "Duration, e.g.: 10.0 (seconds)"),
            new Parameter<byte>("interpolator", "Enable fade between start and end scale, e.g.: 1 (true) / 0 (false)"),
            new Parameter<byte>("broadcast", "Broadcast to all players in the field, e.g.: 1 (true) / 0 (false)")
        };
        Usage = "/timescale [enable] [startScale] [endScale] [duration] [interpolator] [broadcast]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        bool enable = trigger.Get<byte>("enable") == 1;
        if (!enable)
        {
            trigger.Session.Send(TimeScalePacket.SetTimeScale(false, 0, 0, 0, 0));
            return;
        }

        float startScale = trigger.Get<float>("startScale");
        float endScale = trigger.Get<float>("endScale");
        float duration = trigger.Get<float>("duration");
        byte interpolator = trigger.Get<byte>("interpolator");
        bool broadcast = trigger.Get<byte>("broadcast") == 1;
        PacketWriter packet = TimeScalePacket.SetTimeScale(true, startScale, endScale, duration, interpolator);
        if (broadcast)
        {
            trigger.Session.FieldManager.BroadcastPacket(packet);
            return;
        }

        trigger.Session.Send(packet);
    }
}

public class WeatherCommand : InGameCommand
{
    private readonly string WeatherTypes = "";

    public WeatherCommand()
    {
        foreach (object slot in Enum.GetValues(typeof(WeatherType)))
        {
            WeatherTypes += $"{slot}, ";
        }

        WeatherTypes = WeatherTypes.TrimEnd(',', ' ');

        Aliases = new()
        {
            "weather"
        };
        Description = "Sets the weather.";
        Parameters = new()
        {
            new Parameter<string>("weatherType", $"Weather type, e.g.: Rain ({WeatherTypes})"),
        };
        Usage = "/weather [weatherType]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        string weather = trigger.Get<string>("weatherType");

        if (!Enum.TryParse(weather, out WeatherType weatherType))
        {
            trigger.Session.SendNotice($"Available weathers: {WeatherTypes}");
            return;
        }

        trigger.Session.Send(FieldPropertyPacket.ChangeWeather(weatherType));
    }
}

public class FreeCamCommand : InGameCommand
{
    public FreeCamCommand()
    {
        Aliases = new()
        {
            "freecam"
        };
        Description = "Enables free camera mode.";
        Usage = "/freecam";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        trigger.Session.Send(FieldPropertyPacket.FreeCam(true));
    }
}

public class LightCommand : InGameCommand
{
    public LightCommand()
    {
        Aliases = new()
        {
            "light"
        };
        Description = "Changes light settings.";
        Parameters = new()
        {
            new Parameter<byte>("option", "0 (Ambient) / 1 (Directional)"),
            new Parameter<byte>("red", "Red, e.g.: 0 ~ 255"),
            new Parameter<byte>("green", "Green, e.g.: 0 ~ 255"),
            new Parameter<byte>("blue", "Blue, e.g.: 0 ~ 255"),
        };
        Usage = "/light [option] [red] [green] [blue]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        byte option = trigger.Get<byte>("option");
        byte red = trigger.Get<byte>("red");
        byte green = trigger.Get<byte>("green");
        byte blue = trigger.Get<byte>("blue");

        if (red is 0 && green is 0 && blue is 0)
        {
            trigger.Session.SendNotice(Usage);
            return;
        }

        switch (option)
        {
            case 0:
                trigger.Session.Send(FieldPropertyPacket.ChangeAmbientLight(red, green, blue));
                break;
            case 1:
                trigger.Session.Send(FieldPropertyPacket.ChangeDirectionalLight(red, green, blue));
                break;
            default:
                trigger.Session.SendNotice($"Invalid option: {option}");
                break;
        }
    }
}

public class ChangeBackgroundCommand : InGameCommand
{
    public ChangeBackgroundCommand()
    {
        Aliases = new()
        {
            "background"
        };
        Description = "Changes the background. Invalid background will turn background pure black.";
        Parameters = new()
        {
            new Parameter<string>("background", "Background dds")
        };
        Usage = "/background [background]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        string background = trigger.Get<string>("background");
        trigger.Session.FieldManager.BroadcastPacket(ChangeBackgroundPacket.ChangeBackground(background));
    }
}

public class MountCommand : InGameCommand
{
    public MountCommand()
    {
        Aliases = new()
        {
            "mount"
        };
        Description = "Mounts on an specific ID.";
        Parameters = new()
        {
            new Parameter<int>("mountId", "Mount ID"),
            new Parameter<byte>("enable", "Enter or leave mount, e.g.: 1 (enter) / 0 (leave)")
        };
        Usage = "/mount [mountId] [enable]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        Player player = trigger.Session.Player;
        Character fieldPlayer = player.FieldPlayer;

        int mountId = trigger.Get<int>("mountId");
        byte enable = trigger.Get<byte>("enable");

        if (enable is 0)
        {
            player.Mount = null; // Remove mount from player
            trigger.Session.FieldManager.BroadcastPacket(MountPacket.StopRide(fieldPlayer, true));
            trigger.Session.Send(KeyTablePacket.SendHotbars(player.GameOptions));
            return;
        }

        IFieldObject<Mount> fieldMount = trigger.Session.FieldManager.RequestFieldObject(new Mount
        {
            Type = RideType.Default,
            Id = mountId,
            Uid = 0
        });

        fieldMount.Value.Players[0] = fieldPlayer;
        player.Mount = fieldMount;

        trigger.Session.FieldManager.BroadcastPacket(MountPacket.StartRide(fieldPlayer));
    }
}
