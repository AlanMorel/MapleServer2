using System.Text.RegularExpressions;
using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Tools;
using Maple2.File.IO.Crypto.Common;
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
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            // Parsing Skills
            if (entry.Name.StartsWith("skill"))
            {
                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                XmlNode ui = document.SelectSingleNode("/ms2/basic/ui");
                XmlNode kinds = document.SelectSingleNode("/ms2/basic/kinds");
                XmlNode stateAttr = document.SelectSingleNode("/ms2/basic/stateAttr");
                XmlNodeList levels = document.SelectNodes("/ms2/level");

                int skillId = int.Parse(Path.GetFileNameWithoutExtension(entry.Name));
                string skillState = kinds.Attributes["state"]?.Value ?? "";
                byte skillAttackType = byte.Parse(ui.Attributes["attackType"]?.Value ?? "0");
                SkillType skillType = (SkillType) byte.Parse(kinds.Attributes["type"].Value);
                SkillSubType skillSubType = (SkillSubType) byte.Parse(kinds.Attributes["subType"]?.Value ?? "0");
                SkillRangeType skillRangeType = (SkillRangeType) byte.Parse(kinds.Attributes["rangeType"]?.Value ?? "0");
                byte skillElement = byte.Parse(kinds.Attributes["element"].Value);
                byte skillSuperArmor = byte.Parse(stateAttr.Attributes["superArmor"].Value);
                bool skillRecovery = int.Parse(kinds.Attributes["spRecoverySkill"]?.Value ?? "0") == 1;
                int[] groupIds = kinds.Attributes["groupIDs"]?.Value?.SplitAndParseToInt(',')?.ToArray() ?? new int[0];

                List<SkillLevel> skillLevels = new();
                foreach (XmlNode level in levels)
                {
                    // Getting all skills level
                    string feature = level.Attributes["feature"]?.Value ?? "";
                    int levelValue = int.Parse(level.Attributes["value"].Value ?? "0");
                    // We prevent duplicates levels from older balances.
                    if (skillLevels.Exists(x => x.Level == levelValue))
                    {
                        continue;
                    }

                    float cooldown = 0;

                    foreach (XmlNode beginCondition in level.SelectNodes("beginCondition"))
                    {
                        cooldown = float.Parse(beginCondition.Attributes["cooldownTime"]?.Value ?? "0");
                    }

                    List<SkillCondition> levelSkillConditions = new();

                    ParseConditionSkill(level, levelSkillConditions);

                    List<SkillMotion> skillMotions = new();
                    foreach (XmlNode motionNode in level.SelectNodes("motion"))
                    {
                        string sequenceName = motionNode.SelectSingleNode("motionProperty")?.Attributes["sequenceName"].Value ?? "";
                        string motionEffect = motionNode.SelectSingleNode("motionProperty")?.Attributes["motionEffect"].Value ?? "";

                        List<SkillAttack> skillAttacks = new();
                        foreach (XmlNode attackNode in motionNode.SelectNodes("attack"))
                        {
                            // TODO: Parse other properties like: pause, arrow
                            DamageProperty damageProperty = ParseDamageProperty(attackNode);
                            RangeProperty rangeProperty = ParseRangeProperty(attackNode);

                            byte attackPoint = byte.Parse(Regex.Match(attackNode.Attributes["point"]?.Value, @"\d").Value);
                            short targetCount = short.Parse(attackNode.Attributes["targetCount"].Value);
                            long magicPathId = long.Parse(attackNode.Attributes["magicPathID"]?.Value ?? "0");
                            long cubeMagicPathId = long.Parse(attackNode.Attributes["cubeMagicPathID"]?.Value ?? "0");
                            int[] compulsionType = attackNode.Attributes["compulsionType"]?.Value?.SplitAndParseToInt(',')?.ToArray() ?? new int[0];

                            List<SkillCondition> skillConditions = new();

                            ParseConditionSkill(attackNode, skillConditions);

                            skillAttacks.Add(new(attackPoint, targetCount, magicPathId, cubeMagicPathId, rangeProperty, skillConditions, damageProperty, compulsionType));
                        }

                        skillMotions.Add(new(sequenceName, motionEffect, skillAttacks));
                    }

                    SkillUpgrade skillUpgrade = ParseSkillUpgrade(level);
                    (int spirit, int stamina) = ParseConsume(level);

                    skillLevels.Add(new(levelValue, spirit, stamina, feature, levelSkillConditions, skillMotions, skillUpgrade, cooldown, ParseBeginCondition(level)));
                }

                skillList.Add(new(skillId, skillLevels, skillState, skillAttackType, skillType, skillSubType, skillElement, skillSuperArmor, skillRecovery, skillRangeType, groupIds));
            }

            // Parsing SubSkills
            if (entry.Name.StartsWith("table/job"))
            {
                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                XmlNodeList jobs = document.SelectNodes("/ms2/job");
                foreach (XmlNode job in jobs)
                {
                    // Grabs all the skills and them the jobCode.
                    XmlNodeList skills = job.SelectNodes("skills/skill");
                    int jobCode = int.Parse(job.Attributes["code"].Value);
                    foreach (XmlNode skill in skills)
                    {
                        int id = int.Parse(skill.Attributes["main"].Value);
                        short maxLevel = short.Parse(skill.Attributes["maxLevel"]?.Value ?? "1");
                        skillList.Find(x => x.SkillId == id).Job = jobCode;
                        skillList.Find(x => x.SkillId == id).MaxLevel = maxLevel;

                        // If it has subSkill, add as well.
                        if (skill.Attributes["sub"] == null)
                        {
                            continue;
                        }

                        int[] sub = skill.Attributes["sub"].Value.SplitAndParseToInt(',').ToArray();
                        skillList.Find(x => x.SkillId == id).SubSkills = sub;
                        foreach (int subSkillId in sub)
                        {
                            SkillMetadata subSkill = skillList.FirstOrDefault(x => x.SkillId == subSkillId);
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
            XmlNodeList levelNodes = document.SelectNodes("/ms2/level");
            int skillId = int.Parse(Path.GetFileNameWithoutExtension(entry.Name));

            SkillMetadata skill = skillList.FirstOrDefault(x => x.SkillId == skillId);
            if (skill is null)
            {
                continue;
            }

            foreach (XmlNode levelNode in levelNodes)
            {
                int currentLevel = int.Parse(levelNode.SelectSingleNode("BasicProperty").Attributes["level"]?.Value ?? "0");
                SkillLevel skillLevel = skill.SkillLevels.FirstOrDefault(x => x.Level == currentLevel);
                if (skillLevel is null)
                {
                    continue;
                }

                skillLevel.SkillAdditionalData = ParseSkillData(levelNode);
            }
        }

        return skillList;
    }

    private static void ParseConditionSkill(XmlNode parentNode, List<SkillCondition> skillConditions)
    {
        foreach (XmlNode conditionNode in parentNode.SelectNodes("conditionSkill"))
        {
            int[] conditionSkillId = conditionNode.Attributes["skillID"]?.Value?.SplitAndParseToInt(',')?.ToArray() ?? new int[0];
            short[] conditionSkillLevel = conditionNode.Attributes["level"]?.Value?.SplitAndParseToShort(',')?.ToArray() ?? new short[0];
            bool splash = conditionNode.Attributes["splash"]?.Value == "1";
            byte target = byte.Parse(conditionNode.Attributes["skillTarget"].Value ?? "0");
            byte owner = byte.Parse(conditionNode.Attributes["skillOwner"]?.Value ?? "0");
            bool immediateActive = conditionNode.Attributes["immediateActive"]?.Value == "1";
            short fireCount = short.Parse(conditionNode.Attributes["fireCount"].Value ?? "0");
            int interval = int.Parse(conditionNode.Attributes["interval"].Value ?? "0");
            uint delay = uint.Parse(conditionNode.Attributes["delay"].Value ?? "0");
            int removeDelay = int.Parse(conditionNode.Attributes["removeDelay"]?.Value ?? "0");
            bool useDirection = int.Parse(conditionNode.Attributes["useDirection"]?.Value ?? "0") == 1;

            skillConditions.Add(new(conditionSkillId, conditionSkillLevel, splash, target, owner, fireCount, interval, immediateActive)
            {
                Delay = delay,
                RemoveDelay = removeDelay,
                UseDirection = useDirection,
                BeginCondition = ParseBeginCondition(conditionNode)
            });
            ;
        }
    }

    private static SkillBeginCondition ParseBeginCondition(XmlNode parent)
    {
        SkillBeginCondition beginCondition = null;

        foreach (XmlNode beginNode in parent.SelectNodes("beginCondition"))
        {
            beginCondition = new()
            {
                Owner = ParseConditionSubject(beginNode, "owner"),
                Target = ParseConditionSubject(beginNode, "target"),
                Caster = ParseConditionSubject(beginNode, "caster"),
                Probability = float.Parse(beginNode.Attributes["probability"]?.Value ?? "0")
            };
        }

        return beginCondition;
    }

    private static BeginConditionSubject ParseConditionSubject(XmlNode parentNode, string tagName)
    {
        foreach (XmlNode ownerNode in parentNode.SelectNodes(tagName))
        {
            return new()
            {
                EventSkillIDs = ownerNode.Attributes["eventSkillID"]?.Value?.SplitAndParseToInt(',')?.ToArray() ?? new int[0],
                EventEffectIDs = ownerNode.Attributes["eventEffectID"]?.Value?.SplitAndParseToInt(',')?.ToArray() ?? new int[0],
                HasBuffId = int.Parse(ownerNode.Attributes["hasBuffID"].Value ?? "0"),
                HasNotBuffId = int.Parse(ownerNode.Attributes["hasNotBuffID"]?.Value ?? "0"),
                HasBuffCount = int.Parse(ownerNode.Attributes["hasBuffCount"].Value ?? "0"),
            };
        }

        return null;
    }

    private static DamageProperty ParseDamageProperty(XmlNode attack)
    {
        float damageRate = float.Parse(attack.SelectSingleNode("damageProperty")?.Attributes?["rate"]?.Value ?? "0");
        float hitSpeedRate = float.Parse(attack.SelectSingleNode("damageProperty")?.Attributes?["hitSpeedRate"]?.Value ?? "0");

        return new(damageRate, hitSpeedRate);
    }

    private static (int spirit, int stamina) ParseConsume(XmlNode level)
    {
        int spirit = int.Parse(level.SelectSingleNode("consume/stat")?.Attributes?["sp"]?.Value ?? "0");
        int stamina = int.Parse(level.SelectSingleNode("consume/stat")?.Attributes?["ep"]?.Value ?? "0");
        return (spirit, stamina);
    }

    private static SkillUpgrade ParseSkillUpgrade(XmlNode level)
    {
        XmlNode upgradeNode = level.SelectSingleNode("upgrade");
        int upgradeLevel = int.Parse(upgradeNode?.Attributes?["level"]?.Value ?? "0");
        int[] upgradeSkills = upgradeNode?.Attributes?["skillIDs"]?.Value.SplitAndParseToInt(',').ToArray();
        short[] upgradeSkillsLevel = upgradeNode?.Attributes?["skillLevels"]?.Value.SplitAndParseToShort(',').ToArray();

        return new(upgradeLevel, upgradeSkills, upgradeSkillsLevel);
    }

    private static RangeProperty ParseRangeProperty(XmlNode attackNode)
    {
        XmlNode rangeNode = attackNode.SelectSingleNode("rangeProperty");

        bool includeCaster = rangeNode.Attributes["includeCaster"]?.Value == "1";
        string rangeType = rangeNode.Attributes["rangeType"].Value;
        _ = int.TryParse(rangeNode.Attributes["distance"].Value, out int distance);
        CoordF rangeAdd = CoordF.Parse(rangeNode.Attributes["rangeAdd"].Value);
        CoordF rangeOffset = CoordF.Parse(rangeNode.Attributes["rangeOffset"].Value);

        return new(includeCaster, rangeType, distance, rangeAdd, rangeOffset);
    }

    private static SkillAdditionalData ParseSkillData(XmlNode level)
    {
        XmlNode basicProperty = level.SelectSingleNode("BasicProperty");

        int duration = int.Parse(basicProperty.Attributes["durationTick"]?.Value ?? "0");
        BuffType buffType = (BuffType) int.Parse(basicProperty.Attributes["buffType"]?.Value ?? "0");
        BuffSubType buffSubType = (BuffSubType) int.Parse(basicProperty.Attributes["buffSubType"]?.Value ?? "0");
        int buffCategory = int.Parse(basicProperty.Attributes["buffCategory"]?.Value ?? "0");
        int maxStack = int.Parse(basicProperty.Attributes["maxBuffCount"]?.Value ?? "0");
        byte keepCondition = byte.Parse(basicProperty.Attributes["keepCondition"]?.Value ?? "0");

        return new(duration, buffType, buffSubType, buffCategory, maxStack, keepCondition);
    }
}
