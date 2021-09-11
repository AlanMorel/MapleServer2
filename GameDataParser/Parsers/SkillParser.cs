using System.Xml;
using System.Text.RegularExpressions;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class SkillParser : Exporter<List<SkillMetadata>>
    {
        public SkillParser(MetadataResources resources) : base(resources, "skill") { }

        protected override List<SkillMetadata> Parse()
        {
            List<SkillMetadata> skillList = new List<SkillMetadata>();
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
                    byte skillType = byte.Parse(kinds.Attributes["type"].Value);
                    byte skillSubType = byte.Parse(kinds.Attributes["subType"]?.Value ?? "0");
                    byte skillElement = byte.Parse(kinds.Attributes["element"].Value);
                    byte skillSuperArmor = byte.Parse(stateAttr.Attributes["superArmor"].Value);
                    bool skillRecovery = int.Parse(kinds.Attributes["spRecoverySkill"]?.Value ?? "0") == 1;

                    List<SkillLevel> skillLevels = new List<SkillLevel>();
                    foreach (XmlNode level in levels)
                    {
                        // Getting all skills level
                        string feature = level.Attributes["feature"]?.Value ?? "";
                        int levelValue = int.Parse(level.Attributes["value"].Value ?? "0");
                        // We prevent duplicates levels from older balances.
                        if (skillLevels.Exists(level => level.Level == levelValue))
                        {
                            continue;
                        }
                        int spirit = int.Parse(level.SelectSingleNode("consume/stat").Attributes["sp"]?.Value ?? "0");
                        int stamina = int.Parse(level.SelectSingleNode("consume/stat").Attributes["ep"]?.Value ?? "0");
                        float damageRate = float.Parse(level.SelectSingleNode("motion/attack/damageProperty")?.Attributes["rate"].Value ?? "0");
                        string sequenceName = level.SelectSingleNode("motion/motionProperty")?.Attributes["sequenceName"].Value ?? "";
                        string motionEffect = level.SelectSingleNode("motion/motionProperty")?.Attributes["motionEffect"].Value ?? "";

                        SkillUpgrade skillUpgrade = new SkillUpgrade();
                        if (level.SelectSingleNode("motion/upgrade")?.Attributes != null)
                        {
                            int upgradeLevel = int.Parse(level.SelectSingleNode("motion/upgrade").Attributes["level"].Value ?? "0");
                            int[] upgradeSkills = Array.ConvertAll(level.SelectSingleNode("motion/upgrade").Attributes["skillIDs"].Value.Split(","), int.Parse);
                            short[] upgradeSkillsLevel = Array.ConvertAll(level.SelectSingleNode("motion/upgrade").Attributes["skillLevels"].Value.Split(","), short.Parse);

                            skillUpgrade = new SkillUpgrade(upgradeLevel, upgradeSkills, upgradeSkillsLevel);
                        }

                        // Getting all Attack attr in each level.
                        List<SkillAttack> skillAttacks = new List<SkillAttack>();
                        List<SkillCondition> skillConditions = new List<SkillCondition>();

                        XmlNodeList conditionSkills = level.SelectNodes("motion/attack/conditionSkill") ?? level.SelectNodes("conditionSkill");
                        foreach (XmlNode conditionSkill in conditionSkills)
                        {
                            int conditionSkillId = int.Parse(conditionSkill.Attributes["skillID"]?.Value ?? "0");
                            short conditionSkillLevel = short.Parse(conditionSkill.Attributes["level"]?.Value ?? "0");
                            bool splash = conditionSkill.Attributes["splash"]?.Value == "1";
                            byte target = byte.Parse(conditionSkill.Attributes["skillTarget"].Value ?? "0");
                            byte owner = byte.Parse(conditionSkill.Attributes["skillOwner"]?.Value ?? "0");
                            SkillCondition skillCondition = new SkillCondition(conditionSkillId, conditionSkillLevel, splash, target, owner);

                            skillConditions.Add(skillCondition);
                        }

                        XmlNodeList attackListAttr = level.SelectNodes("motion/attack");
                        foreach (XmlNode attackAttr in attackListAttr)
                        {
                            // Many skills has a condition to proc another skill.
                            // We capture that as a list, since each Attack attr has one at least.
                            byte attackPoint = byte.Parse(Regex.Match(attackAttr.Attributes["point"]?.Value, @"\d").Value);
                            short targetCount = short.Parse(attackAttr.Attributes["targetCount"].Value);
                            SkillAttack skillAttack = new SkillAttack(attackPoint, targetCount);

                            skillAttacks.Add(skillAttack);
                        }

                        SkillMotion skillMotion = new SkillMotion(sequenceName, motionEffect);
                        SkillLevel skillLevel = new SkillLevel(levelValue, spirit, stamina, damageRate, feature, skillMotion, skillAttacks, skillConditions, skillUpgrade);
                        skillLevels.Add(skillLevel);
                    }
                    skillList.Add(new SkillMetadata(skillId, skillLevels, skillState, skillAttackType, skillType, skillSubType, skillElement, skillSuperArmor, skillRecovery));
                }
                // Parsing SubSkills
                else if (entry.Name.StartsWith("table/job"))
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

                            int[] sub = Array.ConvertAll(skill.Attributes["sub"].Value.Split(","), int.Parse);
                            skillList.Find(x => x.SkillId == id).SubSkills = sub;
                            for (int n = 0; n < sub.Length; n++)
                            {
                                if (skillList.Select(x => x.SkillId).Contains(sub[n]))
                                {
                                    skillList.Find(x => x.SkillId == sub[n]).Job = jobCode;
                                }
                            }
                        }
                        XmlNodeList learnSkills = job.SelectNodes("learn/skill");
                        foreach (XmlNode learnSkill in learnSkills)
                        {
                            int id = int.Parse(learnSkill.Attributes["id"].Value);
                            skillList.Find(x => x.SkillId == id).CurrentLevel = 1;
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
                XmlNodeList levels = document.SelectNodes("/ms2/level");
                int skillId = int.Parse(Path.GetFileNameWithoutExtension(entry.Name));

                if (skillList.Select(x => x.SkillId).Contains(skillId))
                {
                    foreach (XmlNode level in levels)
                    {
                        int currentLevel = int.Parse(level.SelectSingleNode("BasicProperty").Attributes["level"]?.Value ?? "0");
                        List<SkillLevel> skillLevels = skillList.Find(x => x.SkillId == skillId).SkillLevels;

                        if (skillLevels.Select(x => x.Level).Contains(currentLevel))
                        {
                            skillLevels.Find(x => x.Level == currentLevel).SkillAdditionalData = ParseSkillData(level);
                        }
                    }
                }
                // Adding missing skills from additionaleffect.
                // Since they are many skills that are called by another skill and not from player directly.
                else
                {
                    List<SkillLevel> skillLevels = new List<SkillLevel>();
                    foreach (XmlNode level in levels)
                    {
                        int currentLevel = int.Parse(level.SelectSingleNode("BasicProperty").Attributes["level"]?.Value ?? "0");
                        skillLevels.Add(new SkillLevel(currentLevel, ParseSkillData(level)));
                    }
                    skillList.Add(new SkillMetadata(skillId, skillLevels));
                }
            }
            return skillList;
        }

        public static SkillAdditionalData ParseSkillData(XmlNode level)
        {
            int duration = int.Parse(level.SelectSingleNode("BasicProperty").Attributes["durationTick"]?.Value ?? "0");
            int buffType = int.Parse(level.SelectSingleNode("BasicProperty").Attributes["buffType"]?.Value ?? "0");
            int buffSubType = int.Parse(level.SelectSingleNode("BasicProperty").Attributes["buffSubType"]?.Value ?? "0");
            int buffCategory = int.Parse(level.SelectSingleNode("BasicProperty").Attributes["buffCategory"]?.Value ?? "0");
            int maxStack = int.Parse(level.SelectSingleNode("BasicProperty").Attributes["maxBuffCount"]?.Value ?? "0");
            byte keepCondition = byte.Parse(level.SelectSingleNode("BasicProperty").Attributes["keepCondition"]?.Value ?? "0");

            return new SkillAdditionalData(duration, buffType, buffSubType, buffCategory, maxStack, keepCondition);
        }
    }
}
