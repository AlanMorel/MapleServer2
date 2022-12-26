using System.Drawing;
using Maple2.Trigger.Enum;
using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Commands.Core;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Extensions;
using MapleServer2.Managers.Actors;
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
            string[] classes = Enum.GetNames(typeof(JobCode));

            player.Session.Send(NoticePacket.Notice(
                "You have to give a classname and specify awakening (1 or 0)\nAvailable classes:\n".Bold().Color(Color.DarkOrange) +
                $"{string.Join(", ", classes).Color(Color.Aquamarine)}", NoticeType.Chat));

            return;
        }

        if (!Enum.TryParse(jobName, true, out JobCode job))
        {
            player.Session.SendNotice($"{jobName} is not a valid class name");
            return;
        }

        if (job == JobCode.None)
        {
            player.Session.SendNotice("None is not a valid class");
            return;
        }

        player.Awakened = awakened == 1;

        if (job != player.JobCode)
        {
            player.JobCode = job;
            DatabaseManager.SkillTabs.Delete(skillTab.Uid);

            SkillTab newSkillTab = new(player.CharacterId, job, player.SubJobCode, skillTab.TabId, skillTab.Name);
            player.SkillTabs[player.SkillTabs.IndexOf(skillTab)] = newSkillTab;
        }

        player.SkillTabs.FirstOrDefault(x => x.TabId == activeSkillTabId)?.LearnDefaultSkills(player.JobCode, player.SubJobCode);
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
            new Parameter<string>("equipSlot", "Equip slot, e.g.: RH (ItemSlot.cs) or PET (Active pet)"),
            new Parameter<string>("newAttributeId", "New Attribute, e.g.: Dex (StatAttribute.cs)"),
            new Parameter<float>("value", "Value, e.g.: 10 / 0.002"),
            new Parameter<byte>("isPercentage", "Is percentage, e.g.: 1 / 0"),
            new Parameter<byte>("category", "Stat Category: 0 = constant, 1 = static, 2 = random"),
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
        bool isPet = equipSlot == "PET";

        if (string.IsNullOrEmpty(equipSlot))
        {
            trigger.Session.SendNotice($"Type '/info {Aliases.First()}' for more details.");
            return;
        }

        if ((!Enum.TryParse(equipSlot, ignoreCase: true, out ItemSlot itemSlot) || itemSlot == ItemSlot.NONE) && !isPet)
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

        if (!Enum.TryParse(newAttributeId, ignoreCase: true, out StatAttribute newAttribute))
        {
            trigger.Session.SendNotice($"{newAttributeId} is not a valid attribute. Check StatAttribute.cs");
            return;
        }

        Item item = null;
        Player player = trigger.Session.Player;
        if (!isPet && !player.Inventory.Equips.TryGetValue(itemSlot, out item))
        {
            trigger.Session.SendNotice($"You don't have an item in slot {itemSlot}.");
            return;
        }

        if (isPet)
        {
            item = player.ActivePet;
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

        switch (category)
        {
            case 0 when value == 0:
                item.Stats.Constants.Remove(newAttribute);
                break;
            case 0:
                item.Stats.Constants[newAttribute] = itemStat;
                break;
            case 1 when value == 0:
                item.Stats.Statics.Remove(newAttribute);
                break;
            case 1:
                item.Stats.Statics[newAttribute] = itemStat;
                break;
            case 2 when value == 0:
                item.Stats.Randoms.Remove(newAttribute);
                break;
            case 2:
                item.Stats.Randoms[newAttribute] = itemStat;
                break;
        }

        if (!isPet)
        {
            trigger.Session.FieldManager.BroadcastPacket(EquipmentPacket.EquipItem(player.FieldPlayer, item, itemSlot));
        }
        else
        {
            player.Inventory.RemoveItem(player.Session, item.Uid, out Item _);
            player.Inventory.AddItem(player.Session, item, true);

            player.Session.FieldManager.RemovePet(player.FieldPlayer.ActivePet);

            player.Session.FieldManager.AddPet(player.Session, item.Uid);
        }

        player.FieldPlayer.ComputeStats();

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

        if (!Enum.TryParse(equipSlot, ignoreCase: true, out ItemSlot itemSlot) || itemSlot == ItemSlot.NONE)
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

        item.Stats.Constants.Clear();
        item.Stats.Randoms.Clear();
        item.Stats.Statics.Clear();

        player.FieldPlayer.ComputeStats();

        trigger.Session.FieldManager.BroadcastPacket(EquipmentPacket.EquipItem(player.FieldPlayer, item, itemSlot));

        DatabaseManager.Items.Update(item);
    }
}

public class PetLevelCommand : InGameCommand
{
    public PetLevelCommand()
    {
        Aliases = new()
        {
            "petlevel"
        };
        Description = "Sets the level of your active pet.";
        Usage = "/petlevel level";
        Parameters = new()
        {
            new Parameter<int>("level", "The pet level to set the pet to (1 - 50). Not the same as item level.")
        };
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        int level = trigger.Get<int>("level");

        Player player = trigger.Session.Player;
        Item pet = player.ActivePet;

        if (pet is null)
        {
            trigger.Session.SendNotice("No pet currently equipped.");

            return;
        }

        pet.PetInfo.Level = (short) Math.Min(50, Math.Max(1, level));

        player.Inventory.RemoveItem(player.Session, pet.Uid, out Item _);
        player.Inventory.AddItem(player.Session, pet, true);

        player.Session.FieldManager.RemovePet(player.FieldPlayer.ActivePet);
        player.Session.FieldManager.AddPet(player.Session, pet.Uid);
    }
}

public class BonusPointsCommand : InGameCommand
{
    public BonusPointsCommand()
    {
        Aliases = new()
        {
            "bonuspoints"
        };
        Description = "Sets the bonus attribute points ands skill points. Leave out 'mode' and 'amount' to display point counts instead.";
        Usage = "/bonuspoints [type] [source] [mode] [amount]";
        Parameters = new()
        {
            new Parameter<string>("type", "Type of bonus points. e.g.: Attrib/Attribute, Rank1, Rank2"),
            new Parameter<string>("source", "Source of the bonus points. Attribute sources: Trophy, Quest, Exploration, Prestige. Skill point sources: Trophy, Chapter."),
            new Parameter<string>("mode", "Specifies whether to add or set points. e.g.: Add/Set"),
            new Parameter<int>("amount", "Amount to add/set")
        };
    }

    private enum BonusPointType
    {
        Unknown,
        Attribute,
        Rank1,
        Rank2,
        Attrib = Attribute,
        SkillRank1 = Rank1,
        SkillRank2 = Rank2
    }

    private enum PointModifyMode
    {
        Add,
        Set
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        Player player = trigger.Session.Player;

        string type = trigger.Get<string>("type");
        string source = trigger.Get<string>("source");
        string mode = trigger.Get<string>("mode");

        bool printSkills = true;
        bool printAtributes = true;
        short skillRank = 0;
        BonusPointType pointType = BonusPointType.Unknown;
        SkillPointSource skillSource = SkillPointSource.Unknown;
        OtherStatsIndex attribSource = OtherStatsIndex.Unknown;

        if (!string.IsNullOrEmpty(type))
        {
            if (!Enum.TryParse(type, ignoreCase: true, out pointType) || pointType == BonusPointType.Unknown)
            {
                trigger.Session.SendNotice($"'{source}' is not a valid point source!");

                return;
            }

            printSkills = pointType == BonusPointType.Rank1 || pointType == BonusPointType.Rank2;
            printAtributes = pointType == BonusPointType.Attribute;
            skillRank = (short) (pointType == BonusPointType.Rank1 ? 0 : 1);
        }

        if (!string.IsNullOrEmpty(source))
        {
            bool parsedSkillSource = pointType != BonusPointType.Attribute && Enum.TryParse(source, ignoreCase: true, out skillSource);
            bool parsedAttributeSource = pointType == BonusPointType.Attribute && Enum.TryParse(source, ignoreCase: true, out attribSource);

            if (!parsedSkillSource && !parsedAttributeSource)
            {
                trigger.Session.SendNotice($"'{source}' is not a valid {pointType} point source!");

                return;
            }
        }

        if (string.IsNullOrEmpty(mode))
        {
            if (printSkills)
            {
                int pointsFound = 0;

                foreach ((SkillPointSource pointSource, ExtraSkillPoints skillPoints) in player.StatPointDistribution.ExtraSkillPoints)
                {
                    if (pointSource != skillSource && skillSource != SkillPointSource.Unknown)
                    {
                        continue;
                    }

                    foreach ((short rank, int points) in skillPoints.ExtraPoints)
                    {
                        if (points != 0 && (rank == skillRank || skillSource == SkillPointSource.Unknown))
                        {
                            trigger.Session.SendNotice($"{points} rank {rank + 1} skill points from {pointSource}");

                            pointsFound += points;
                        }
                    }
                }

                trigger.Session.SendNotice($"Found {pointsFound} skill points from the specified source");
            }

            if (printAtributes)
            {
                int pointsFound = 0;

                foreach ((OtherStatsIndex pointSource, int attribPoints) in player.StatPointDistribution.OtherStats)
                {
                    if (pointSource != attribSource && attribSource != OtherStatsIndex.Unknown)
                    {
                        continue;
                    }

                    trigger.Session.SendNotice($"{attribPoints} attribute points from {pointSource}");

                    pointsFound += attribPoints;
                }

                trigger.Session.SendNotice($"Found {pointsFound} attribute points from the specified source");
            }

            return;
        }

        if (!Enum.TryParse(mode, ignoreCase: true, out PointModifyMode modifyMode))
        {
            trigger.Session.SendNotice($"Invalid point modification mode '{mode}'");

            return;
        }

        int amount = trigger.Get<int>("amount");

        if (pointType == BonusPointType.Attribute)
        {
            if (modifyMode == PointModifyMode.Set && player.StatPointDistribution.OtherStats.TryGetValue(attribSource, out int currentAmount))
            {
                amount -= currentAmount;
            }

            player.StatPointDistribution.AddTotalStatPoints(amount, attribSource);

            player.Session.Send(StatPointPacket.WriteTotalStatPoints(player));
        }

        if (pointType != BonusPointType.Attribute)
        {
            if (modifyMode == PointModifyMode.Set)
            {
                amount -= player.StatPointDistribution.ExtraSkillPoints[skillSource].ExtraPoints[skillRank];
            }

            player.StatPointDistribution.AddTotalSkillPoints(amount, skillRank, skillSource);
            player.Session.Send(SkillPointPacket.ExtraSkillPoints(player));
        }
    }
}

public class DebugPrintCommand : InGameCommand
{
    private enum DebugType
    {
        HitTarget,
        CastedEffects,
        OwnEffects,
        EffectsFromOthers,
        EffectEvents,
        IncludeTickEvent,
        WatchList,
        IgnoreList
    }

    private enum ListMode
    {
        Add,
        Set,
        Disable,
        Remove
    }

    public DebugPrintCommand()
    {
        Aliases = new()
        {
            "debugprint"
        };
        Description = "Enables and disables debug print settings";
        Usage = "/debugprint type [setting] [settingList]";
        Parameters = new()
        {
            new Parameter<string>("type", "The debug setting type to configure. Enter 'list' to display available types."),
            new Parameter<string>("setting", "The value to set the setting to. Enter 'List' to display what the valid values are."),
            new Parameter<string>("settingList", "Used to set the watch & ignore lists. Multiple values can be entered by separating with commas without spaces.")
        };
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        Player player = trigger.Session.Player;

        string type = trigger.Get<string>("type");
        string setting = trigger.Get<string>("setting");
        string? settingList = trigger.Get<string>("settingList");

        if (string.IsNullOrEmpty(type))
        {
            trigger.Session.Send(NoticePacket.Notice("Enter a setting to set", NoticeType.Chat));

            return;
        }

        type = type.ToLower();

        if (type == "list")
        {
            trigger.Session.Send(NoticePacket.Notice($"/debugprint HitTarget amount - Displays the target ids of hit targets. 'amount' is the number of targets to print. -1 means no limit while 0 means disabling it.", NoticeType.Chat));
            trigger.Session.Send(NoticePacket.Notice($"/debugprint CastedEffects enabled - Prints out additional effects that get applied on targets in chat", NoticeType.Chat));
            trigger.Session.Send(NoticePacket.Notice($"/debugprint OwnEffects enabled - Enables & disables printing effects originating from you", NoticeType.Chat));
            trigger.Session.Send(NoticePacket.Notice($"/debugprint EffectsFromOthers enabled - Enables & disables printing effects applied to you by something else", NoticeType.Chat));
            trigger.Session.Send(NoticePacket.Notice($"/debugprint EffectEvents enabled - Enables & disables printing events that get fired on effects", NoticeType.Chat));
            trigger.Session.Send(NoticePacket.Notice($"/debugprint IncludeTickEvent enabled - Enables & disables printing the tick event with EffectEvents", NoticeType.Chat));
            trigger.Session.Send(NoticePacket.Notice($"/debugprint WatchList mode idList - Sets the watch list to filter which effects are printed. Valid modes are: add, set, disable, remove", NoticeType.Chat));
            trigger.Session.Send(NoticePacket.Notice($"/debugprint IgnoreList mode idList - Sets the ignore list to filter which effects are printed", NoticeType.Chat));

            return;
        }

        if (!Enum.TryParse(type, ignoreCase: true, out DebugType typeValue))
        {
            trigger.Session.Send(NoticePacket.Notice($"{type} is not a valid debug print setting type. Available: int HitTarget, bool CastedEffects, bool OwnEffects, bool EffectsFromOthers", NoticeType.Chat));

            return;
        }

        List<int>? listToSet = null;
        string listName = "";

        switch (typeValue)
        {
            case DebugType.HitTarget:
                if (string.IsNullOrEmpty(setting))
                {
                    player.DebugPrint.TargetsToPrint = player.DebugPrint.TargetsToPrint != 0 ? 0 : -1;
                }
                else
                {
                    player.DebugPrint.TargetsToPrint = int.Parse(setting);
                }

                trigger.Session.Send(NoticePacket.Notice($"Set HitTarget to {player.DebugPrint.TargetsToPrint}", NoticeType.Chat));

                break;
            case DebugType.CastedEffects:
                player.DebugPrint.PrintCastedEffects = setting is null ? !player.DebugPrint.PrintCastedEffects : (setting == "true" || setting == "1");

                trigger.Session.Send(NoticePacket.Notice($"Set CastedEffects to {player.DebugPrint.PrintCastedEffects}", NoticeType.Chat));

                break;
            case DebugType.OwnEffects:
                player.DebugPrint.PrintOwnEffects = setting is null ? !player.DebugPrint.PrintOwnEffects : (setting == "true" || setting == "1");

                trigger.Session.Send(NoticePacket.Notice($"Set OwnEffects to {player.DebugPrint.PrintOwnEffects}", NoticeType.Chat));

                break;
            case DebugType.EffectsFromOthers:
                player.DebugPrint.PrintEffectsFromOthers = setting is null ? !player.DebugPrint.PrintEffectsFromOthers : (setting == "true" || setting == "1");

                trigger.Session.Send(NoticePacket.Notice($"Set EffectsFromOthers to {player.DebugPrint.PrintEffectsFromOthers}", NoticeType.Chat));

                break;
            case DebugType.EffectEvents:
                player.DebugPrint.PrintEffectEvents = setting is null ? !player.DebugPrint.PrintEffectEvents : (setting == "true" || setting == "1");

                trigger.Session.Send(NoticePacket.Notice($"Set EffectEvents to {player.DebugPrint.PrintEffectEvents}", NoticeType.Chat));

                break;
            case DebugType.IncludeTickEvent:
                player.DebugPrint.IncludeEffectTickEvent = setting is null ? !player.DebugPrint.IncludeEffectTickEvent : (setting == "true" || setting == "1");

                trigger.Session.Send(NoticePacket.Notice($"Set IncludeTickEvent to {player.DebugPrint.IncludeEffectTickEvent}", NoticeType.Chat));

                break;
            case DebugType.WatchList:
                listToSet = player.DebugPrint.EffectWatchList;
                listName = "WatchList";

                break;
            case DebugType.IgnoreList:
                listToSet = player.DebugPrint.EffectIgnoreList;
                listName = "IgnoreList";

                break;
        }

        if (listToSet is null)
        {
            return;
        }

        if (string.IsNullOrEmpty(setting))
        {
            trigger.Session.Send(NoticePacket.Notice("Please enter a list modification mode. Valid modes are: add, set, disable, remove", NoticeType.Chat));

            return;
        }

        setting = setting.ToLower();

        if (!Enum.TryParse(setting, true, out ListMode mode))
        {
            trigger.Session.Send(NoticePacket.Notice($"Invalid mode '{setting}'. Please enter a mode. Valid modes are: add, set, disable, remove", NoticeType.Chat));

            return;
        }

        if (mode == ListMode.Disable)
        {
            listToSet.Clear();

            trigger.Session.Send(NoticePacket.Notice($"Cleared {listName}", NoticeType.Chat));

            return;
        }

        if (string.IsNullOrEmpty(settingList))
        {
            trigger.Session.Send(NoticePacket.Notice($"Please enter a list of ids to {setting}", NoticeType.Chat));

            return;
        }

        int[] idList = settingList.Split(',')
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(int.Parse)
            .Except(new int[] { 0 })
            .ToArray();

        if (idList.Length == 0)
        {
            trigger.Session.Send(NoticePacket.Notice("Invalid id list entered. Please enter at least one valid id.", NoticeType.Chat));

            return;
        }

        string modeName = mode switch
        {
            ListMode.Add => "Adding",
            ListMode.Set => "Setting",
            ListMode.Remove => "Removing",
            _ => "[error]"
        };

        trigger.Session.Send(NoticePacket.Notice($"{modeName} {listName} items: {string.Join(", ", idList)}", NoticeType.Chat));

        if (mode == ListMode.Set)
        {
            listToSet.Clear();
        }

        if (mode != ListMode.Remove)
        {
            listToSet.AddRange(idList);
        }

        if (mode == ListMode.Remove)
        {
            int removing = 0;

            for (int i = 0; i < listToSet.Count; ++i)
            {
                if (idList.Contains(listToSet[i]))
                {
                    ++removing;
                }
                else
                {
                    listToSet[i - removing] = listToSet[i];
                }
            }

            if (removing > 0)
            {
                listToSet.RemoveRange(listToSet.Count - removing, removing);
            }
        }

        trigger.Session.Send(NoticePacket.Notice($"{listName} items: {string.Join(", ", listToSet)}", NoticeType.Chat));
    }
}

public class PrintEffectsCommand : InGameCommand
{
    public PrintEffectsCommand()
    {
        Aliases = new()
        {
            "printeffects"
        };
        Description = "Prints the effects currently applied to you.";
        Usage = "/printeffects";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        Player player = trigger.Session.Player;

        trigger.Session.Send(NoticePacket.Notice($"Effects currently on {player.Name}:", NoticeType.Chat));

        foreach (AdditionalEffect effect in player.AdditionalEffects.Effects)
        {
            trigger.Session.Send(NoticePacket.Notice($"\tEffect: {effect.Id} Level {effect.Level} with {effect.Stacks} stacks", NoticeType.Chat));
        }
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
        ShopHelper.OpenShop(trigger.Session, 999999, 29000307);
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

        if (!Enum.TryParse(weather, ignoreCase: true, out WeatherType weatherType))
        {
            trigger.Session.SendNotice($"Available weathers: {WeatherTypes}");
            return;
        }

        trigger.Session.FieldManager.BroadcastPacket(FieldPropertyPacket.ChangeWeather(weatherType));
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
                trigger.Session.FieldManager.BroadcastPacket(FieldPropertyPacket.ChangeAmbientLight(red, green, blue));
                break;
            case 1:
                trigger.Session.FieldManager.BroadcastPacket(FieldPropertyPacket.ChangeDirectionalLight(red, green, blue));
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

public class ClearInventoryCommand : InGameCommand
{
    private readonly string InventoryTypes = "";
    public ClearInventoryCommand()
    {
        foreach (object slot in Enum.GetValues(typeof(InventoryTab)))
        {
            InventoryTypes += $"{slot}, ";
        }

        InventoryTypes = InventoryTypes.TrimEnd(',', ' ');
        Aliases = new()
        {
            "clearinv",
            "clearinventory"
        };
        Description = "Clears the chosen inventory.";
        Parameters = new()
        {
            new Parameter<string>("inventory", "Inventory name. e.g.: Gear (InventoryTab.cs)"),
        };
        Usage = "/clearinv [inventory]";
    }
    public override void Execute(GameCommandTrigger trigger)
    {
        string inventory = trigger.Get<string>("inventory");

        if (!Enum.TryParse(inventory, ignoreCase: true, out InventoryTab inventoryTab))
        {
            trigger.Session.SendNotice($"Available inventories: {InventoryTypes}");
            return;
        }

        IReadOnlyCollection<Item> itemsInTab = trigger.Session.Player.Inventory.GetItems(inventoryTab);
        foreach (Item item in itemsInTab)
        {
            trigger.Session.Player.Inventory.RemoveItem(trigger.Session, item.Uid, out _);
            DatabaseManager.Items.Delete(item.Uid);
        }

        trigger.Session.SendNotice($"Cleared {inventory}");
    }
}

public class SetUserValueCommand : InGameCommand
{
    public SetUserValueCommand()
    {
        Aliases = new()
        {
            "setuservalue"
        };
        Description = "Sets a user's values for map triggers.";
        Parameters = new()
        {
            new Parameter<byte>("key", "Key of the User Value"),
            new Parameter<byte>("value", "Value used within the key. Must be an integer.")
        };
        Usage = "/setuservalue [key] [value]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        string key = trigger.Get<string>("key");
        int value = trigger.Get<int>("value");

        PlayerTrigger newPlayerTrigger = new(key)
        {
            Value = value
        };

        PlayerTrigger? playerTrigger = trigger.Session.Player.Triggers.FirstOrDefault(x => x.Key == key);
        if (playerTrigger is null)
        {
            trigger.Session.Player.Triggers.Add(newPlayerTrigger);
            trigger.Session.SendNotice($"Added key [{key}] with value of [{value}]");
            return;
        }

        playerTrigger.Value = value;
        trigger.Session.SendNotice($"Modified key [{key}] with value of [{value}]");
    }
}
