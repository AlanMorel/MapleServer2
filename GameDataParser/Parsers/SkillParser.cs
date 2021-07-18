using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class SkillParser : Exporter<List<SkillMetadata>>
    {
        public SkillParser(MetadataResources resources) : base(resources, "skill") { }

        protected override List<SkillMetadata> Parse()
        {
            List<SkillMetadata> skillList = new List<SkillMetadata>();
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {
                // Parsing Skills
                if (entry.Name.StartsWith("skill"))
                {
                    XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);
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
                        XmlNode motionProperty = level.SelectSingleNode("motion/motionProperty");

                        string feature = level.Attributes["feature"]?.Value ?? "";
                        int levelValue = int.Parse(level.Attributes["value"].Value ?? "0");
                        int spirit = int.Parse(level.SelectSingleNode("consume/stat").Attributes["sp"]?.Value ?? "0");
                        int stamina = int.Parse(level.SelectSingleNode("consume/stat").Attributes["ep"]?.Value ?? "0");
                        float damageRate = float.Parse(level.SelectSingleNode("motion/attack/damageProperty")?.Attributes["rate"].Value ?? "0");
                        string sequenceName = motionProperty?.Attributes["sequenceName"].Value ?? "";
                        string motionEffect = motionProperty?.Attributes["motionEffect"].Value ?? "";

                        // Getting all Attack attr in each level.
                        XmlNodeList attackListAttr = level.SelectNodes("motion/attack");
                        List<int> conditionSkillIds = new List<int>();

                        foreach (XmlNode attackAttr in attackListAttr)
                        {
                            // Many skills has a condition to proc another skill.
                            // We capture that as a list, since each Attack attr has one at least.
                            XmlNodeList conditionSkillList = attackAttr.SelectNodes("conditionSkill");

                            foreach (XmlNode conditionSkill in conditionSkillList)
                            {
                                conditionSkillIds.Add(int.Parse(conditionSkill.Attributes["skillID"]?.Value ?? "0"));
                            }
                        }

                        SkillMotion skillMotion = new SkillMotion(sequenceName, motionEffect);
                        SkillAttack skillAttack = new SkillAttack(conditionSkillIds);
                        skillLevels.Add(new SkillLevel(levelValue, spirit, stamina, damageRate, feature, skillMotion, skillAttack));
                    }
                    skillList.Add(new SkillMetadata(skillId, skillLevels, skillState, skillAttackType, skillType, skillSubType, skillElement, skillSuperArmor, skillRecovery));
                }
                // Parsing SubSkills
                else if (entry.Name.StartsWith("table/job"))
                {
                    XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                    XmlNodeList jobs = document.SelectNodes("/ms2/job");
                    foreach (XmlNode job in jobs)
                    {
                        if (job.Attributes["feature"] != null) // Getting attribute that just have "feature"
                        {
                            string feature = job.Attributes["feature"].Value;
                            if (feature == "JobChange_02") // Getting JobChange_02 skillList for now until better handle Awakening system.
                            {
                                XmlNode skills = job.SelectSingleNode("skills");
                                int jobCode = int.Parse(job.Attributes["code"].Value);
                                for (int i = 0; i < skills.ChildNodes.Count; i++)
                                {
                                    int id = int.Parse(skills.ChildNodes[i].Attributes["main"].Value);
                                    SkillMetadata skill = skillList.Find(x => x.SkillId == id); // This find the skill in the SkillList
                                    skill.Job = jobCode;
                                    if (skills.ChildNodes[i].Attributes["sub"] != null)
                                    {
                                        if (skillList.Select(x => x.SkillId).Contains(id))
                                        {
                                            int[] sub = Array.ConvertAll(skills.ChildNodes[i].Attributes["sub"].Value.Split(","), int.Parse); // Trim?
                                            skill.SubSkills = sub;
                                            for (int n = 0; n < sub.Length; n++)
                                            {
                                                if (skillList.Select(x => x.SkillId).Contains(sub[n]))
                                                {
                                                    skillList.Find(x => x.SkillId == sub[n]).Job = jobCode;
                                                }
                                            }
                                        }
                                    }
                                }
                                XmlNode learn = job.SelectSingleNode("learn");
                                for (int i = 0; i < learn.ChildNodes.Count; i++)
                                {
                                    int id = int.Parse(learn.ChildNodes[i].Attributes["id"].Value);
                                    skillList.Find(x => x.SkillId == id).Learned = 1;
                                }
                            }
                        }
                        else if (job.Attributes["code"].Value == "001")
                        {
                            XmlNode skills = job.SelectSingleNode("skills");

                            for (int i = 0; i < skills.ChildNodes.Count; i++)
                            {
                                int id = int.Parse(skills.ChildNodes[i].Attributes["main"].Value);
                                if (skills.ChildNodes[i].Attributes["sub"] != null)
                                {
                                    int[] sub = Array.ConvertAll(skills.ChildNodes[i].Attributes["sub"].Value.Split(","), int.Parse);
                                }
                            }
                        }
                    }
                }
            }
            // Parsing Additional Data
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {
                if (!entry.Name.StartsWith("additionaleffect"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                XmlNodeList levels = document.SelectNodes("/ms2/level");
                int skillId = int.Parse(Path.GetFileNameWithoutExtension(entry.Name));

                if (skillList.Select(x => x.SkillId).Contains(skillId))
                {
                    foreach (XmlNode level in levels)
                    {
                        SkillAdditionalData data = ParseSkillData(level);

                        int currentLevel = int.Parse(level.SelectSingleNode("BasicProperty").Attributes["level"]?.Value ?? "0");
                        if (skillList.Find(x => x.SkillId == skillId).SkillLevels.Select(x => x.Level).Contains(currentLevel))
                        {
                            skillList.Find(x => x.SkillId == skillId).SkillLevels.Find(x => x.Level == currentLevel).SkillAdditionalData = data;
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

                        SkillAdditionalData data = ParseSkillData(level);
                        SkillLevel skillLevel = new SkillLevel(currentLevel, data);
                        skillLevels.Add(skillLevel);
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

            return new SkillAdditionalData(duration, buffType, buffSubType, buffCategory, maxStack);
        }
    }
}
