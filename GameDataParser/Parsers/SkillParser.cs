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
            List<ListSubSkill> subSkillList = new List<ListSubSkill>();
            foreach (PackFileEntry xmlFile in entries)
            {
                // Parsing Skills
                if (xmlFile.Name.StartsWith("skill"))
                {
                    string skillId = Path.GetFileNameWithoutExtension(xmlFile.Name);
                    SkillMetadata metadata = new SkillMetadata();
                    List<SkillLevel> skillLevel = new List<SkillLevel>();

                    metadata.SkillId = int.Parse(skillId);
                    Debug.Assert(metadata.SkillId > 0, $"Invalid Id {metadata.SkillId} from {skillId}");

                    XmlDocument document = m2dFile.GetDocument(xmlFile.FileHeader);

                    XmlNodeList basic = document.SelectNodes("/ms2/basic");
                    foreach (XmlNode node in basic)
                    {
                        if (node.Attributes.GetNamedItem("feature") != null)
                        {
                            // Get Rank
                            metadata.Feature = node.Attributes["feature"].Value;
                        }
                    }

                    XmlNodeList levels = document.SelectNodes("/ms2/level");
                    foreach (XmlNode node in levels)
                    {
                        if (node.Attributes.GetNamedItem("feature") == null || node.Attributes.GetNamedItem("feature").Value == metadata.Feature)
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
                else if (xmlFile.Name.StartsWith("table/job"))
                {
                    XmlDocument document = m2dFile.GetDocument(xmlFile.FileHeader);
                    XmlNodeList job = document.SelectNodes("/ms2/job");
                    foreach (XmlNode node in job)
                    {
                        if (node.Attributes.GetNamedItem("feature") != null)
                        {
                            string feature = node.Attributes["feature"].Value;
                            if (feature == "JobChange_02")
                            {
                                XmlNode skills = node.SelectSingleNode("skills");
                                for (int i = 0; i < skills.ChildNodes.Count; i++)
                                {
                                    int id = int.Parse(skills.ChildNodes[i].Attributes["main"].Value);
                                    int[] sub = new int[0];
                                    if (skills.ChildNodes[i].Attributes["sub"] != null)
                                    {
                                        if (skillList.Select(x => x.SkillId).Contains(id))
                                        {
                                            sub = Array.ConvertAll(skills.ChildNodes[i].Attributes["sub"].Value.Split(","), int.Parse);
                                            skillList.Find(x => x.SkillId == id).SubSkill = sub;
                                        }
                                    }
                                    subSkillList.Add(new ListSubSkill(id, sub));
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
                                subSkillList.Add(new ListSubSkill(id, sub));
                            }
                        }
                    }
                }
                // Add SubSkills to Skills
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
