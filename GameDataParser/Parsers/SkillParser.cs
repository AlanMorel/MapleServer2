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
            foreach (PackFileEntry entry in resources.xmlFiles)
            {
                // Parsing Skills
                if (entry.Name.StartsWith("skill"))
                {
                    string skillId = Path.GetFileNameWithoutExtension(entry.Name);
                    SkillMetadata metadata = new SkillMetadata();
                    List<SkillLevel> skillLevels = new List<SkillLevel>();

                    metadata.SkillId = int.Parse(skillId);
                    XmlDocument document = resources.xmlMemFile.GetDocument(entry.FileHeader);
                    XmlNodeList levels = document.SelectNodes("/ms2/level");
                    foreach (XmlNode level in levels)
                    {
                        // Getting all skills level
                        string feature = level.Attributes["feature"] != null ? level.Attributes["feature"].Value : "";
                        int levelValue = level.Attributes["value"].Value != null ? int.Parse(level.Attributes["value"].Value) : 0;
                        int spirit = level.SelectSingleNode("consume/stat").Attributes["sp"] != null ? int.Parse(level.SelectSingleNode("consume/stat").Attributes["sp"].Value) : 0;
                        float damageRate = level.SelectSingleNode("motion/attack/damageProperty") != null ? float.Parse(level.SelectSingleNode("motion/attack/damageProperty").Attributes["rate"].Value) : 0;
                        skillLevels.Add(new SkillLevel(levelValue, spirit, damageRate, feature));
                    }
                    metadata.SkillLevels = skillLevels;
                    string state = document.SelectSingleNode("/ms2/basic/kinds").Attributes["state"]?.Value;
                    metadata.State = string.IsNullOrEmpty(state) ? "" : state;

                    skillList.Add(metadata);
                }
                // Parsing SubSkills
                else if (entry.Name.StartsWith("table/job"))
                {
                    XmlDocument document = resources.xmlMemFile.GetDocument(entry.FileHeader);
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
    }
}
