﻿using System.Xml;
using GameDataParser.Files;
using GameDataParser.Tools;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class JobParser : Exporter<List<JobMetadata>>
{
    public JobParser(MetadataResources resources) : base(resources, "job") { }

    protected override List<JobMetadata> Parse()
    {
        List<JobMetadata> jobs = new();
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

                JobMetadata metadata = new()
                {
                    JobId = short.Parse(jobNode.Attributes["code"].Value),
                    StartMapId = int.Parse(jobNode.Attributes["startField"].Value),
                    OpenTaxis = jobNode.Attributes["tutorialClearOpenTaxis"]?.Value.SplitAndParseToInt(',').ToList(),
                    OpenMaps = jobNode.Attributes["tutorialClearOpenMaps"]?.Value.SplitAndParseToInt(',').ToList()
                };

                foreach (XmlNode childNode in jobNode)
                {
                    if (childNode.Name.Equals("startInvenItem"))
                    {
                        Dictionary<int, TutorialItemMetadata> tutorialItemsDictionary = new();
                        foreach (XmlNode startItem in childNode)
                        {
                            TutorialItemMetadata tutorialItem = new()
                            {
                                ItemId = int.Parse(startItem.Attributes["itemID"].Value),
                                Rarity = byte.Parse(startItem.Attributes["grade"].Value),
                                Amount = byte.Parse(startItem.Attributes["count"].Value)
                            };

                            if (tutorialItemsDictionary.ContainsKey(tutorialItem.ItemId))
                            {
                                tutorialItemsDictionary[tutorialItem.ItemId].Amount += tutorialItem.Amount;
                                continue;
                            }

                            tutorialItemsDictionary[tutorialItem.ItemId] = tutorialItem;
                        }

                        metadata.TutorialItems.AddRange(tutorialItemsDictionary.Values);
                    }
                    else if (childNode.Name.Equals("skills"))
                    {
                        foreach (XmlNode skillNode in childNode)
                        {
                            int skillId = int.Parse(skillNode.Attributes["main"].Value);
                            byte maxLevel = byte.Parse(skillNode.Attributes["maxLevel"]?.Value ?? "1");
                            short subJobCode = short.Parse(skillNode.Attributes["subJobCode"]?.Value ?? "0");
                            byte quickSlotPriority = byte.Parse(skillNode.Attributes["quickSlotPriority"]?.Value ?? "99");

                            List<int> subSkillIds = skillNode.Attributes["sub"]?.Value.SplitAndParseToInt(',').ToList();

                            metadata.Skills.Add(new(skillId, subJobCode, maxLevel, subSkillIds, quickSlotPriority));
                        }
                    }
                    else if (childNode.Name.Equals("learn"))
                    {
                        JobLearnedSkillsMetadata learnedSkills = new()
                        {
                            Level = int.Parse(childNode.Attributes["level"].Value)
                        };
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
