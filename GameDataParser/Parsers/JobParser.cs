using System.Collections.Generic;
using System.Linq;
using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class JobParser : Exporter<List<JobMetadata>>
    {
        public JobParser(MetadataResources resources) : base(resources, "job") { }

        protected override List<JobMetadata> Parse()
        {
            List<JobMetadata> jobs = new List<JobMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.Equals("table/job.xml"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                XmlNodeList jobNodes = document.GetElementsByTagName("job");
                foreach (XmlNode jobNode in jobNodes)
                {
                    if (jobNode.Attributes["feature"] == null || jobNode.Attributes["feature"].Value != "JobChange_02") // only get latest job data
                    {
                        continue;
                    }

                    JobMetadata metadata = new JobMetadata();

                    metadata.JobId = short.Parse(jobNode.Attributes["code"].Value);
                    metadata.StartMapId = int.Parse(jobNode.Attributes["startField"].Value);

                    List<string> openTaxis = new List<string>();
                    if (jobNode.Attributes["tutorialClearOpenTaxis"] != null)
                    {
                        openTaxis = jobNode.Attributes["tutorialClearOpenTaxis"].Value.Split(",").ToList();
                        foreach (string openTaxi in openTaxis)
                        {
                            metadata.OpenTaxis.Add(int.Parse(openTaxi));
                        }
                    }

                    List<string> openMaps = new List<string>();
                    if (jobNode.Attributes["tutorialClearOpenMaps"] != null)
                    {
                        openMaps = jobNode.Attributes["tutorialClearOpenMaps"].Value.Split(",").ToList();
                        foreach (string openMap in openMaps)
                        {
                            metadata.OpenMaps.Add(int.Parse(openMap));
                        }
                    }

                    foreach (XmlNode childNode in jobNode)
                    {
                        if (childNode.Name.Equals("startInvenItem"))
                        {
                            foreach (XmlNode startItem in childNode)
                            {
                                int itemId = int.Parse(startItem.Attributes["itemID"].Value);
                                byte rarity = byte.Parse(startItem.Attributes["grade"].Value);
                                byte amount = byte.Parse(startItem.Attributes["count"].Value);
                                metadata.TutorialItems.Add(new TutorialItemMetadata(itemId, rarity, amount));
                            }
                        }

                        else if (childNode.Name.Equals("skills"))
                        {
                            foreach (XmlNode skillNode in childNode)
                            {
                                int skillId = int.Parse(skillNode.Attributes["main"].Value);
                                byte maxLevel = 1;
                                if (skillNode.Attributes["maxLevel"] != null)
                                {
                                    maxLevel = byte.Parse(skillNode.Attributes["maxLevel"].Value);
                                }

                                List<int> subSkillIds = new List<int>();
                                if (skillNode.Attributes["sub"] != null)
                                {
                                    List<string> stringSubSkillIds = skillNode.Attributes["sub"].Value.Split(",").ToList();
                                    foreach (string subSkillId in stringSubSkillIds)
                                    {
                                        subSkillIds.Add(int.Parse(subSkillId));
                                    }
                                }

                                short subJobCode = 0;
                                if (skillNode.Attributes["subJobCode"] != null)
                                {
                                    subJobCode = short.Parse(skillNode.Attributes["subJobCode"].Value);
                                }
                                metadata.Skills.Add(new JobSkillMetadata(skillId, subJobCode, maxLevel, subSkillIds));
                            }
                        }

                        else if (childNode.Name.Equals("learn"))
                        {
                            JobLearnedSkillsMetadata learnedSkills = new JobLearnedSkillsMetadata();
                            learnedSkills.Level = int.Parse(childNode.Attributes["level"].Value);
                            foreach (XmlNode skillNode in childNode)
                            {
                                learnedSkills.SkillIds.Add(int.Parse(skillNode.Attributes["id"].Value));
                            }
                            metadata.LearnedSkills.Add(learnedSkills);
                        }
                    }

                    jobs.Add(metadata);
                }
            }
            return jobs;
        }
    }
}
