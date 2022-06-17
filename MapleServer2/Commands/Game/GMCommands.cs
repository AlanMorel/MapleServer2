using System.Drawing;
using Maple2Storage.Enums;
using MapleServer2.Commands.Core;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Extensions;
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
            new Parameter<byte>("isPercentage", "Is percentage, e.g.: 1 / 0"),
            new Parameter<byte>("category", "Stat Category, e.g.: 1 - 4"),
        };
        Usage = "/attribute [equipSlot] [attributeId] [value] [isPercentage] [category]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        string equipSlot = trigger.Get<string>("equipSlot");
        string newAttributeId = trigger.Get<string>("newAttributeId");
        float value = trigger.Get<float>("value");
        byte isPercentage = trigger.Get<byte>("isPercentage");
        byte category = trigger.Get<byte>("category");

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
            //trigger.Session.SendNotice("Value cannot be 0.");
            //return;
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

        if (category == 0)
        {
            if (value == 0)
                item.Stats.Constants.Remove(newAttribute);
            else
                item.Stats.Constants[newAttribute] = itemStat;

        }
        else if (category == 1)
        {
            if (value == 0)
                item.Stats.Statics.Remove(newAttribute);
            else
                item.Stats.Statics[newAttribute] = itemStat;
        }
        else if (category == 2)
        {
            if (value == 0)
                item.Stats.Randoms.Remove(newAttribute);
            else
                item.Stats.Randoms[newAttribute] = itemStat;
        }

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
