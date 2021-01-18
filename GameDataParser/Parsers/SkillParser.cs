using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace GameDataParser.Parsers
{
    public static class SkillParser
    {
        public static List<SkillMetadata> Parse(MemoryMappedFile m2dFile, IEnumerable<PackFileEntry> entries)
        {
            List<SkillMetadata> skillList = new List<SkillMetadata>();
            foreach (PackFileEntry entry in entries)
            {
                // Parsing Skills
                if (entry.Name.StartsWith("skill"))
                {
                    string skillId = Path.GetFileNameWithoutExtension(entry.Name);
                    SkillMetadata metadata = new SkillMetadata();
                    List<SkillLevel> skillLevel = new List<SkillLevel>();

                    metadata.SkillId = int.Parse(skillId);
                    XmlDocument document = m2dFile.GetDocument(entry.FileHeader);
                    XmlNodeList levels = document.SelectNodes("/ms2/level");
                    foreach (XmlNode node in levels)
                    {
                        if (node.Attributes.GetNamedItem("feature") == null || node.Attributes.GetNamedItem("feature").Value == "JobChange_01")
                        {
                            int level = node.Attributes["value"].Value != null ? int.Parse(node.Attributes["value"].Value) : 1;
                            XmlNode consume = node.SelectSingleNode("consume/stat");

                            int spirit = 0;
                            int upgradeLevel = 0;
                            int[] upgradeSkillId = new int[0];
                            int[] upgradeSkillLevel = new int[0];

                            if (consume.Attributes.GetNamedItem("sp") != null)
                            {
                                spirit = int.Parse(consume.Attributes["sp"].Value);
                            }

                            XmlNode upgrade = node.SelectSingleNode("upgrade");
                            if (upgrade.Attributes != null)
                            {
                                if (upgrade.Attributes.GetNamedItem("level") != null)
                                {
                                    upgradeLevel = int.Parse(upgrade.Attributes["level"].Value);
                                }
                                if (upgrade.Attributes.GetNamedItem("skillIDs") != null)
                                {
                                    upgradeSkillId = Array.ConvertAll(upgrade.Attributes.GetNamedItem("skillIDs").Value.Split(","), int.Parse);
                                }
                                if (upgrade.Attributes.GetNamedItem("skillLevels") != null)
                                {
                                    upgradeSkillLevel = Array.ConvertAll(upgrade.Attributes.GetNamedItem("skillLevels").Value.Split(","), int.Parse);
                                }
                            }
                            skillLevel.Add(new SkillLevel(level, spirit, upgradeLevel, upgradeSkillId, upgradeSkillLevel));
                            metadata.SkillLevel = skillLevel[0];
                        }
                    }
                    skillList.Add(metadata);
                }
                // Parsing SubSkills
                else if (entry.Name.StartsWith("table/job"))
                {
                    XmlDocument document = m2dFile.GetDocument(entry.FileHeader);
                    XmlNodeList job = document.SelectNodes("/ms2/job");
                    foreach (XmlNode node in job)
                    {
                        if (node.Attributes.GetNamedItem("feature") != null)
                        {
                            string feature = node.Attributes["feature"].Value;
                            if (feature == "JobChange_02")
                            {
                                int jobCode = int.Parse(node.Attributes.GetNamedItem("code").Value);
                                XmlNode skills = node.SelectSingleNode("skills");
                                for (int i = 0; i < skills.ChildNodes.Count; i++)
                                {
                                    int id = int.Parse(skills.ChildNodes[i].Attributes["main"].Value);
                                    int[] sub = new int[0];
                                    SkillMetadata skill = skillList.Find(x => x.SkillId == id);
                                    skill.Job = jobCode;
                                    if (skills.ChildNodes[i].Attributes["sub"] != null)
                                    {
                                        if (skillList.Select(x => x.SkillId).Contains(id))
                                        {
                                            sub = Array.ConvertAll(skills.ChildNodes[i].Attributes["sub"].Value.Split(","), int.Parse);
                                            skill.SubSkill = sub;
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
                            }
                        }
                        else if (node.Attributes.GetNamedItem("code").Value == "001")
                        {
                            XmlNode skills = node.SelectSingleNode("skills");

                            for (int i = 0; i < skills.ChildNodes.Count; i++)
                            {
                                int id = int.Parse(skills.ChildNodes[i].Attributes["main"].Value);
                                int[] sub = new int[0];
                                if (skills.ChildNodes[i].Attributes["sub"] != null)
                                {
                                    sub = Array.ConvertAll(skills.ChildNodes[i].Attributes["sub"].Value.Split(","), int.Parse);
                                }
                            }
                        }
                    }
                }
            }
            return skillList;
        }

        public static void Write(List<SkillMetadata> skills)
        {
            using (FileStream writeStream = File.Create(VariableDefines.OUTPUT + "ms2-skill-metadata"))
            {
                Serializer.Serialize(writeStream, skills);
            }
            using (FileStream readStream = File.OpenRead(VariableDefines.OUTPUT + "ms2-skill-metadata"))
            {
                // Ensure the file is read equivalent
                // Debug.Assert(skills.SequenceEqual(Serializer.Deserialize<List<SkillMetadata>>(readStream)));
            }
            Console.WriteLine("\rSuccessfully parsed skill metadata!");
        }
    }
}
