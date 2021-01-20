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
                    foreach (XmlNode level in levels)
                    {
                        int levelValue = level.Attributes["value"].Value != null ? int.Parse(level.Attributes["value"].Value) : 0;
                        int spirit = level.SelectSingleNode("consume/stat").Attributes["sp"] != null ? int.Parse(level.SelectSingleNode("consume/stat").Attributes["sp"].Value) : 0;
                        float damageRate = level.SelectSingleNode("motion/attack/damageProperty") != null ? float.Parse(level.SelectSingleNode("motion/attack/damageProperty").Attributes["rate"].Value) : 0;
                        skillLevel.Add(new SkillLevel(levelValue, spirit, damageRate));
                        metadata.SkillLevel = skillLevel[0];

                        // Get values ​​that are different from null and other "feature"
                    }
                    skillList.Add(metadata);
                }
                // Parsing SubSkills
                else if (entry.Name.StartsWith("table/job"))
                {
                    XmlDocument document = m2dFile.GetDocument(entry.FileHeader);
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
                                    int[] sub = new int[0];
                                    SkillMetadata skill = skillList.Find(x => x.SkillId == id); // This find the skill in the SkillList
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
