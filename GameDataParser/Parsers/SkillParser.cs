using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
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

            foreach (PackFileEntry skill in entries)
            {
                if (!skill.Name.StartsWith("skill"))
                    continue;

                string skillId = Path.GetFileNameWithoutExtension(skill.Name);
                SkillMetadata metadata = new SkillMetadata();
                List<SkillLevel> skillLevel = new List<SkillLevel>();

                metadata.SkillId = int.Parse(skillId);
                Debug.Assert(metadata.SkillId > 0, $"Invalid Id {metadata.SkillId} from {skillId}");

                //using XmlReader reader = m2dFile.GetReader(skill.FileHeader);
                XmlDocument document = m2dFile.GetDocument(skill.FileHeader);

                XmlNodeList basic = document.SelectNodes("/ms2/basic");
                foreach (XmlNode node in basic)
                {
                    if (node.Attributes.GetNamedItem("feature") != null)
                    {
                        // Get Rank
                        metadata.Feature = node.Attributes["feature"].Value;
                    }
                    XmlNode kinds = node.SelectSingleNode("kinds");
                    metadata.Type = kinds.Attributes["type"].Value != null ? int.Parse(kinds.Attributes["type"].Value) : 0;
                    if (kinds.Attributes.GetNamedItem("subType") != null)
                    {
                        // Get Rank
                        metadata.SubType = kinds.Attributes["subType"].Value != null ? int.Parse(kinds.Attributes["subType"].Value) : 0;
                    }
                    XmlNode stateAttr = node.SelectSingleNode("stateAttr");
                    if (stateAttr.Attributes.GetNamedItem("useDefaultSkill") != null)
                    {
                        // Get Rank
                        metadata.DefaultSkill = stateAttr.Attributes["useDefaultSkill"].Value != null ? byte.Parse(stateAttr.Attributes["useDefaultSkill"].Value) : 0;
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
