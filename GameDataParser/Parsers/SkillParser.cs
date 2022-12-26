using System.Text.RegularExpressions;
using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Parsers.Helpers;
using GameDataParser.Tools;
using M2dXmlGenerator;
using Maple2.File.IO.Crypto.Common;
using Maple2.File.Parser.Tools;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class SkillParser : Exporter<List<SkillMetadata>>
{
    public SkillParser(MetadataResources resources) : base(resources, MetadataName.Skill) { }

    protected override List<SkillMetadata> Parse()
    {
        List<SkillMetadata> skillList = new();

        Filter.Load(Resources.XmlReader, "NA", "Live");

        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            // Parsing Skills
            if (entry.Name.StartsWith("skill"))
            {
                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                XmlNode? ui = document.SelectSingleNode("/ms2/basic/ui");
                XmlNode? kinds = document.SelectSingleNode("/ms2/basic/kinds");
                XmlNode? stateAttr = document.SelectSingleNode("/ms2/basic/stateAttr");
                XmlNodeList? levels = document.SelectNodes("/ms2/level");

                if (levels is null)
                {
                    continue;
                }

                int skillId = int.Parse(Path.GetFileNameWithoutExtension(entry.Name));
                string skillState = kinds?.Attributes?["state"]?.Value ?? "";
                byte skillAttackType = byte.Parse(ui?.Attributes?["attackType"]?.Value ?? "0");
                SkillType skillType = (SkillType) byte.Parse(kinds?.Attributes?["type"]?.Value ?? "0");
                SkillSubType skillSubType = (SkillSubType) byte.Parse(kinds?.Attributes?["subType"]?.Value ?? "0");
                SkillRangeType skillRangeType = (SkillRangeType) byte.Parse(kinds?.Attributes?["rangeType"]?.Value ?? "0");
                byte skillElement = byte.Parse(kinds?.Attributes?["element"]?.Value ?? "0");
                byte skillSuperArmor = byte.Parse(stateAttr?.Attributes?["superArmor"]?.Value ?? "0");
                bool skillRecovery = int.Parse(kinds?.Attributes?["spRecoverySkill"]?.Value ?? "0") == 1;
                int[] groupIds = kinds?.Attributes?["groupIDs"]?.Value.SplitAndParseToInt(',').ToArray() ?? Array.Empty<int>();

                List<SkillLevel> skillLevels = new();
                foreach (XmlNode level in levels)
                {
                    // Getting all skills level
                    string feature = level.Attributes?["feature"]?.Value ?? "";

                    if (feature != "" && !FeatureLocaleFilter.Features.ContainsKey(feature))
                    {
                        continue;
                    }

                    short levelValue = short.Parse(level.Attributes?["value"]?.Value ?? "0");
                    // We prevent duplicates levels from older balances.
                    if (skillLevels.Exists(x => x.Level == levelValue))
                    {
                        continue;
                    }

                    float cooldown = 0;

                    foreach (XmlNode? beginCondition in level.SelectNodes("beginCondition")!)
                    {
                        cooldown = float.Parse(beginCondition?.Attributes?["cooldownTime"]?.Value ?? "0");
                    }

                    List<SkillCondition> levelSkillConditions = new();

                    ParseConditionSkill(level, levelSkillConditions);


                    List<SkillMotion> skillMotions = new();
                    foreach (XmlNode motionNode in level.SelectNodes("motion")!)
                    {
                        string sequenceName = motionNode.SelectSingleNode("motionProperty")?.Attributes?["sequenceName"]?.Value ?? "";
                        string motionEffect = motionNode.SelectSingleNode("motionProperty")?.Attributes?["motionEffect"]?.Value ?? "";

                        List<SkillAttack> skillAttacks = new();
                        foreach (XmlNode attackNode in motionNode?.SelectNodes("attack")!)
                        {
                            if (attackNode is null)
                            {
                                continue;
                            }

                            // TODO: Parse other properties like: pause, arrow
                            DamageProperty damageProperty = ParseDamageProperty(attackNode);
                            RangeProperty rangeProperty = ParseRangeProperty(attackNode);
                            ArrowProperty arrowProperty = ParseArrowProperty(attackNode);

                            byte attackPoint = byte.Parse(Regex.Match(attackNode.Attributes?["point"]?.Value ?? "0", @"\d").Value);
                            short targetCount = short.Parse(attackNode.Attributes?["targetCount"]?.Value ?? "0");
                            long magicPathId = long.Parse(attackNode.Attributes?["magicPathID"]?.Value ?? "0");
                            long cubeMagicPathId = long.Parse(attackNode.Attributes?["cubeMagicPathID"]?.Value ?? "0");
                            int[] compulsionType = attackNode.Attributes?["compulsionType"]?.Value?.SplitAndParseToInt(',')?.ToArray() ?? Array.Empty<int>();
                            SkillDirection direction = (SkillDirection) int.Parse(attackNode.Attributes?["direction"]?.Value ?? "0");

                            List<SkillCondition> skillConditions = new();

                            ParseConditionSkill(attackNode, skillConditions);

                            skillAttacks.Add(new(attackPoint, targetCount, magicPathId, cubeMagicPathId, rangeProperty, arrowProperty, skillConditions, damageProperty,
                                compulsionType, direction));
                        }

                        skillMotions.Add(new(sequenceName, motionEffect, skillAttacks));
                    }

                    SkillUpgrade skillUpgrade = ParseSkillUpgrade(level);
                    (int spirit, int stamina) = ParseConsume(level);
                    RangeProperty rangePropertySkill = ParseDetectProperty(level, "detectProperty");
                    RangeProperty sensorPropertySkill = ParseDetectProperty(level, "sensorProperty");

                    skillLevels.Add(new(levelValue, spirit, stamina, feature, levelSkillConditions, skillMotions, skillUpgrade, cooldown,
                        ParseBeginCondition(level), rangePropertySkill, sensorPropertySkill));
                }

                skillList.Add(new(skillId, skillLevels, skillState, skillAttackType, skillType, skillSubType, skillElement, skillSuperArmor, skillRecovery,
                    skillRangeType, groupIds));
            }

            // Parsing SubSkills
            if (entry.Name.StartsWith("table/job"))
            {
                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);

                foreach (XmlNode job in document.SelectNodes("/ms2/job")!)
                {
                    // Grabs all the skills and them the jobCode.
                    int jobCode = int.Parse(job.Attributes!["code"]!.Value);

                    foreach (XmlNode skill in job.SelectNodes("skills/skill")!)
                    {
                        int id = int.Parse(skill.Attributes!["main"]!.Value);
                        short maxLevel = short.Parse(skill.Attributes["maxLevel"]?.Value ?? "1");
                        skillList.Find(x => x.SkillId == id)!.Job = jobCode;
                        skillList.Find(x => x.SkillId == id)!.MaxLevel = maxLevel;

                        // If it has subSkill, add as well.
                        if (skill.Attributes["sub"] == null)
                        {
                            continue;
                        }

                        int[] sub = skill.Attributes["sub"]!.Value.SplitAndParseToInt(',').ToArray();
                        skillList.Find(x => x.SkillId == id)!.SubSkills = sub;
                        foreach (int subSkillId in sub)
                        {
                            SkillMetadata? subSkill = skillList.FirstOrDefault(x => x.SkillId == subSkillId);
                            if (subSkill is null)
                            {
                                continue;
                            }

                            subSkill.Job = jobCode;
                        }
                    }
                }
            }
        }

        // Parsing Additional Data
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("additionaleffect"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            int skillId = int.Parse(Path.GetFileNameWithoutExtension(entry.Name));

            SkillMetadata? skill = skillList.FirstOrDefault(x => x.SkillId == skillId);
            if (skill is null)
            {
                continue;
            }

            foreach (XmlNode levelNode in document.SelectNodes("/ms2/level")!)
            {
                short currentLevel = short.Parse(levelNode.SelectSingleNode("BasicProperty")?.Attributes?["level"]?.Value ?? "0");
                SkillLevel? skillLevel = skill.SkillLevels.FirstOrDefault(x => x.Level == currentLevel);
                if (skillLevel is null)
                {
                    continue;
                }

                skillLevel.SkillAdditionalData = ParseSkillData(levelNode);
            }
        }

        return skillList;
    }

    public static void ParseConditionSkill(XmlNode? parentNode, List<SkillCondition> skillConditions, string nodeName = "conditionSkill")
    {
        foreach (XmlNode conditionNode in parentNode?.SelectNodes(nodeName)!)
        {
            if (conditionNode?.Attributes is null)
            {
                continue;
            }

            int[] conditionSkillId = conditionNode.Attributes["skillID"]?.Value.SplitAndParseToInt(',').ToArray() ?? Array.Empty<int>();
            short[] conditionSkillLevel = conditionNode.Attributes["level"]?.Value.SplitAndParseToShort(',').ToArray() ?? Array.Empty<short>();
            bool splash = conditionNode.Attributes["splash"]?.Value == "1";
            byte target = byte.Parse(conditionNode.Attributes["skillTarget"]?.Value ?? "0");
            byte owner = byte.Parse(conditionNode.Attributes["skillOwner"]?.Value ?? "0");
            bool immediateActive = conditionNode.Attributes["immediateActive"]?.Value == "1";
            short fireCount = short.Parse(conditionNode.Attributes["fireCount"]?.Value ?? "0");
            int interval = int.Parse(conditionNode.Attributes["interval"]?.Value ?? "0");
            uint delay = uint.Parse(conditionNode.Attributes["delay"]?.Value ?? "0");
            int removeDelay = int.Parse(conditionNode.Attributes["removeDelay"]?.Value ?? "0");
            bool useDirection = int.Parse(conditionNode.Attributes["useDirection"]?.Value ?? "0") == 1;
            bool randomCast = int.Parse(conditionNode.Attributes["randomCast"]?.Value ?? "0") == 1;
            int[] linkSkillID = conditionNode.Attributes["linkSkillID"]?.Value.SplitAndParseToInt(',').ToArray() ?? Array.Empty<int>();
            int overlapCount = int.Parse(conditionNode.Attributes["overlapCount"]?.Value ?? "0");
            bool nonTargetActive = int.Parse(conditionNode.Attributes["nonTargetActive"]?.Value ?? "0") == 1;
            bool onlySensingActive = int.Parse(conditionNode.Attributes["onlySensingActive"]?.Value ?? "0") == 1;
            bool dependOnCasterState = int.Parse(conditionNode.Attributes["dependOnCasterState"]?.Value ?? "0") == 1;
            bool activeByIntervalTick = int.Parse(conditionNode.Attributes["activeByIntervalTick"]?.Value ?? "0") == 1;
            bool dependOnDamageCount = int.Parse(conditionNode.Attributes["dependOnDamageCount"]?.Value ?? "0") == 1;

            skillConditions.Add(new(conditionSkillId, conditionSkillLevel, splash, target, owner, fireCount, interval, immediateActive)
            {
                Delay = delay,
                RemoveDelay = removeDelay,
                UseDirection = useDirection,
                RandomCast = randomCast,
                LinkSkillId = linkSkillID,
                OverlapCount = overlapCount,
                NonTargetActive = nonTargetActive,
                OnlySensingActive = onlySensingActive,
                DependOnCasterState = dependOnCasterState,
                ActiveByIntervalTick = activeByIntervalTick,
                DependOnDamageCount = dependOnDamageCount,
                BeginCondition = ParseBeginCondition(conditionNode)
            });
        }
    }

    public static SkillBeginCondition? ParseBeginCondition(XmlNode? parent)
    {
        SkillBeginCondition? beginCondition = null;

        foreach (XmlNode beginNode in parent?.SelectNodes("beginCondition")!)
        {
            // <stat> can only contain hp, sp, and ep
            if (beginNode is null)
            {
                continue;
            }

            beginCondition = new()
            {
                Owner = ParseConditionSubject(beginNode, "owner"),
                Target = ParseConditionSubject(beginNode, "target"),
                Caster = ParseConditionSubject(beginNode, "caster"),
                Probability = float.Parse(beginNode.Attributes?["probability"]?.Value ?? "0"),
                InvokeEffectFactor = float.Parse(beginNode.Attributes?["invokeEffectFactor"]?.Value ?? "0"),
                CooldownTime = float.Parse(beginNode.Attributes?["cooldownTime"]?.Value ?? "0"),
                DefaultRechargingCooldownTime = float.Parse(beginNode.Attributes?["defaultRechargingCooldownTime"]?.Value ?? "0"),
                AllowDeadState = int.Parse(beginNode.Attributes?["allowDeadState"]?.Value ?? "0") == 1,
                RequireDurationWithoutMove = float.Parse(beginNode.Attributes?["beginCondition.requireDurationWithoutMove"]?.Value ?? "0"),
                UseTargetCountFactor = int.Parse(beginNode.Attributes?["useTargetCountFactor"]?.Value ?? "0") == 1,
                //RequireSkillCodes = new(),
                //RequireMapCodes = new(),
                //RequireMapCategoryCodes = new(),
                //RequireDungeonRooms = new(),
                //Jobs = new(),
                //MapContinents = new()
            };
        }

        return beginCondition;
    }

    private static ConditionOperator ParseConditionOperator(string operatorName)
    {
        if (!Enum.TryParse(operatorName, out ConditionOperator operatorValue))
        {
            return ConditionOperator.None;
        }

        return operatorValue;
    }

    private static BeginConditionSubject? ParseConditionSubject(XmlNode? parentNode, string tagName)
    {
        foreach (XmlNode ownerNode in parentNode?.SelectNodes(tagName)!)
        {
            if (!Enum.TryParse(ownerNode.Attributes!["targetCountSign"]?.Value ?? "", out ConditionOperator targetCountSign))
            {
                targetCountSign = ConditionOperator.None;
            }

            if (!Enum.TryParse(ownerNode.Attributes?["hasBuffCountCompare"]?.Value ?? "", out ConditionOperator hasBuffCountCompare))
            {
                targetCountSign = ConditionOperator.None;
            }

            CompareStatCondition? compareLess = null;
            CompareStatCondition? compareGreater = null;

            // <compareStat> can only contain func and a stat type (hp, sp, asp, msp, ep)
            foreach (XmlNode conditionNode in ownerNode.ChildNodes)
            {
                // the nodes are always formatted as <compareStat stat="value" func="Func"> so the first attribute is the stat name
                XmlAttribute? statAttribute = conditionNode.Attributes?[0];

                if (statAttribute is null)
                {
                    continue;
                }

                StatEntry? entry = StatEntry.Entries.GetValueOrDefault(statAttribute.Name, null);

                if (entry is null)
                {
                    continue;
                }

                CompareStatCondition condition = new()
                {
                    Attribute = entry.Attribute
                };

                if (statAttribute.Name == "asp" || statAttribute.Name == "msp")
                {
                    condition.Value = long.Parse(statAttribute.Value);
                }
                else
                {
                    condition.Rate = double.Parse(statAttribute.Value);
                }

                Enum.TryParse(conditionNode.Attributes?["func"]?.Value, out condition.Func);

                if (condition.Func == ConditionOperator.LessEquals || condition.Func == ConditionOperator.Less)
                {
                    compareLess = condition;
                }
                else
                {
                    compareGreater = condition;
                }
            }

            return new()
            {
                EventSkillIDs = ownerNode.Attributes?["eventSkillID"]?.Value?.SplitAndParseToInt(',')?.ToArray() ?? Array.Empty<int>(),
                EventEffectIDs = ownerNode.Attributes?["eventEffectID"]?.Value?.SplitAndParseToInt(',')?.ToArray() ?? Array.Empty<int>(),
                RequireBuffId = int.Parse(ownerNode.Attributes?["hasBuffID"]?.Value ?? "0"),
                HasNotBuffId = int.Parse(ownerNode.Attributes?["hasNotBuffID"]?.Value ?? "0"),
                RequireBuffCount = ownerNode.Attributes?["hasBuffCount"]?.Value?.SplitAndParseToInt(',')?.ToArray() ?? Array.Empty<int>(),
                RequireBuffCountCompare = ownerNode.Attributes?["hasBuffCountCompare"]?.Value?.Split(',')?.Select(x => ParseConditionOperator(x))?.ToArray() ?? Array.Empty<ConditionOperator>(),
                RequireBuffLevel = int.Parse(ownerNode.Attributes?["hasBuffLevel"]?.Value ?? "0"),
                EventCondition = (EffectEvent) int.Parse(ownerNode.Attributes?["eventCondition"]?.Value ?? "0"),
                IgnoreOwnerEvent = int.Parse(ownerNode.Attributes?["ignoreOwnerEvent"]?.Value ?? "0"),
                TargetCheckRange = int.Parse(ownerNode.Attributes?["targetCheckRange"]?.Value ?? "0"),
                TargetCheckMinRange = int.Parse(ownerNode.Attributes?["targetCheckMinRange"]?.Value ?? "0"),
                TargetInRangeCount = int.Parse(ownerNode.Attributes?["targetInRangeCount"]?.Value ?? "0"),
                TargetFriendly = (TargetAllieganceType) int.Parse(ownerNode.Attributes?["targetFriendly"]?.Value ?? "0"),
                TargetCountSign = ParseConditionOperator(ownerNode.Attributes?["targetCountSign"]?.Value ?? ""),
                CompareStatLess = compareLess,
                CompareStatGreater = compareGreater
            };
        }

        return null;
    }

    private static DamageProperty ParseDamageProperty(XmlNode attack)
    {
        float damageRate = float.Parse(attack?.SelectSingleNode("damageProperty")?.Attributes?["rate"]?.Value ?? "0");
        long damageValue = long.Parse(attack?.SelectSingleNode("damageProperty")?.Attributes?["value"]?.Value ?? "0");
        float hitSpeedRate = float.Parse(attack?.SelectSingleNode("damageProperty")?.Attributes?["hitSpeedRate"]?.Value ?? "0");
        int count = int.Parse(attack?.SelectSingleNode("damageProperty")?.Attributes?["count"]?.Value ?? "0");

        return new(damageRate, hitSpeedRate, count, damageValue);
    }

    private static (int spirit, int stamina) ParseConsume(XmlNode level)
    {
        int spirit = int.Parse(level.SelectSingleNode("consume/stat")?.Attributes?["sp"]?.Value ?? "0");
        int stamina = int.Parse(level.SelectSingleNode("consume/stat")?.Attributes?["ep"]?.Value ?? "0");
        return (spirit, stamina);
    }

    private static SkillUpgrade ParseSkillUpgrade(XmlNode level)
    {
        XmlNode? upgradeNode = level.SelectSingleNode("upgrade");
        int upgradeLevel = int.Parse(upgradeNode?.Attributes?["level"]?.Value ?? "0");
        int[]? upgradeSkills = upgradeNode?.Attributes?["skillIDs"]?.Value.SplitAndParseToInt(',').ToArray();
        short[]? upgradeSkillsLevel = upgradeNode?.Attributes?["skillLevels"]?.Value.SplitAndParseToShort(',').ToArray();

        return new(upgradeLevel, upgradeSkills, upgradeSkillsLevel);
    }

    private static RangeProperty ParseRangeProperty(XmlNode attackNode)
    {
        XmlNode? rangeNode = attackNode.SelectSingleNode("rangeProperty");

        ParseRange(rangeNode, out string rangeType, out int distance, out CoordF rangeAdd, out CoordF rangeOffset, out bool includeCaster,
            out ApplyTarget applyTarget);

        return new(includeCaster, rangeType, distance, rangeAdd, rangeOffset, applyTarget, 0, 0, false, 0, 0, 0, false);
    }

    private static ArrowProperty ParseArrowProperty(XmlNode? attackNode)
    {
        XmlNode? arrowNode = attackNode?.SelectSingleNode("arrowProperty");

        BounceType bounceType = (BounceType) int.Parse(arrowNode?.Attributes?["bounceType"]?.Value ?? "0");
        int bounceCount = int.Parse(arrowNode?.Attributes?["bounceCount"]?.Value ?? "0");
        bool bounceOverlap = int.Parse(arrowNode?.Attributes?["bounceOverlap"]?.Value ?? "0") == 1;
        int bounceRadius = int.Parse(arrowNode?.Attributes?["bounceRadius"]?.Value ?? "0");
        bool nonTarget = int.Parse(arrowNode?.Attributes?["nonTarget"]?.Value ?? "0") == 1;

        return new(bounceType, bounceCount, bounceOverlap, bounceRadius, nonTarget);
    }

    private static RangeProperty ParseDetectProperty(XmlNode attackNode, string nodeName)
    {
        XmlNode? rangeNode = attackNode.SelectSingleNode(nodeName);

        ParseRange(rangeNode, out string rangeType, out int distance, out CoordF rangeAdd, out CoordF rangeOffset, out bool includeCaster,
            out ApplyTarget applyTarget);

        int sensorStartDelay = int.Parse(rangeNode?.Attributes?["sensorStartDelay"]?.Value ?? "0");
        int sensorSplashStartDelay = int.Parse(rangeNode?.Attributes?["sensorSplashStartDelay"]?.Value ?? "0");
        bool sensorForceInvokeByInterval = int.Parse(rangeNode?.Attributes?["sensorForceInvokeByInterval"]?.Value ?? "0") == 1;

        int targetSelectType = int.Parse(rangeNode?.Attributes?["targetSelectType"]?.Value ?? "0");
        int targetHasBuffID = int.Parse(rangeNode?.Attributes?["targetHasBuffID"]?.Value ?? "0");
        int targetHasNotBuffID = int.Parse(rangeNode?.Attributes?["targetHasNotBuffID"]?.Value ?? "0");
        bool targetHasBuffOwner = int.Parse(rangeNode?.Attributes?["targetHasBuffOwner"]?.Value ?? "0") == 1;

        return new(includeCaster, rangeType, distance, rangeAdd, rangeOffset, applyTarget, sensorStartDelay, sensorSplashStartDelay, sensorForceInvokeByInterval, targetSelectType, targetHasBuffID, targetHasNotBuffID, targetHasBuffOwner);
    }

    private static void ParseRange(XmlNode? rangeNode, out string rangeType, out int distance, out CoordF rangeAdd, out CoordF rangeOffset,
        out bool includeCaster, out ApplyTarget applyTarget)
    {
        rangeType = "";
        distance = 0;
        includeCaster = false;
        rangeAdd = default;
        rangeOffset = default;
        applyTarget = ApplyTarget.None;

        if (rangeNode is null)
        {
            return;
        }

        if (ParserHelper.CheckForNull(rangeNode, "includeCaster", "rangeType", "distance", "rangeAdd", "rangeOffset", "applyTarget"))
        {
            return;
        }

        includeCaster = rangeNode.Attributes!["includeCaster"]!.Value == "1";
        rangeType = rangeNode.Attributes["rangeType"]!.Value;
        _ = int.TryParse(rangeNode.Attributes["distance"]!.Value, out distance);
        rangeAdd = CoordF.Parse(rangeNode.Attributes["rangeAdd"]!.Value);
        rangeOffset = CoordF.Parse(rangeNode.Attributes["rangeOffset"]!.Value);
        applyTarget = (ApplyTarget) int.Parse(rangeNode.Attributes["applyTarget"]!.Value);
    }

    private static SkillAdditionalData ParseSkillData(XmlNode level)
    {
        XmlNode? basicProperty = level.SelectSingleNode("BasicProperty");

        int duration = int.Parse(basicProperty?.Attributes?["durationTick"]?.Value ?? "0");
        BuffType buffType = (BuffType) int.Parse(basicProperty?.Attributes?["buffType"]?.Value ?? "0");
        BuffSubType buffSubType = (BuffSubType) int.Parse(basicProperty?.Attributes?["buffSubType"]?.Value ?? "0");
        int buffCategory = int.Parse(basicProperty?.Attributes?["buffCategory"]?.Value ?? "0");
        int maxStack = int.Parse(basicProperty?.Attributes?["maxBuffCount"]?.Value ?? "0");
        byte keepCondition = byte.Parse(basicProperty?.Attributes?["keepCondition"]?.Value ?? "0");

        return new(duration, buffType, buffSubType, buffCategory, maxStack, keepCondition);
    }
}
