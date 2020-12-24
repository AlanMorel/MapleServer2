using GameDataParser.Crypto.Common;
using Maple2Storage.Types;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace GameDataParser.Parsers
{
    public static class SkillParser
    {
        private const string OUTPUT = "Resources/ms2-skill-metadata";

        public static List<SkillMetadata> Parse(MemoryMappedFile m2dFile, IEnumerable<PackFileEntry> entries)
        {
            List<SkillMetadata> skillList = new List<SkillMetadata>();

            foreach (PackFileEntry skill in entries)
            {
                if (!skill.Name.StartsWith("skill/10")) continue;

                string skillId = Path.GetFileNameWithoutExtension(skill.Name);
                SkillMetadata metadata = new SkillMetadata();
                List<SkillMotion> motions = new List<SkillMotion>();
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
                    metadata.SubType = kinds.Attributes["subType"].Value != null ? int.Parse(kinds.Attributes["subType"].Value) : 0;
                    metadata.RangeType = kinds.Attributes["rangeType"].Value != null ? int.Parse(kinds.Attributes["rangeType"].Value) : 0;
                    metadata.Element = kinds.Attributes["element"].Value != null ? int.Parse(kinds.Attributes["element"].Value) : 0;
                }
                XmlNodeList levels = document.SelectNodes("/ms2/level");
                foreach (XmlNode node in levels)
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
                            upgradeSkillLevel = Array.ConvertAll(upgrade.Attributes.GetNamedItem("skillLevels").Value.Split(","), Int32.Parse);
                        }
                    }

                    XmlNode motion = node.SelectSingleNode("motion/motionProperty");

                    string sequenceName = "";
                    string motionEffect = "";
                    string strTagEffects = "";

                    if (motion.Attributes != null)
                    {
                        if (motion.Attributes.GetNamedItem("sequenceName") != null)
                        {
                            sequenceName = motion.Attributes["sequenceName"].Value;
                        }
                        if (motion.Attributes.GetNamedItem("motionEffect") != null)
                        {
                            motionEffect = motion.Attributes["motionEffect"].Value;
                        }
                        if (motion.Attributes.GetNamedItem("strTagEffects") != null)
                        {
                            strTagEffects = motion.Attributes["strTagEffects"].Value;
                        }
                    }
                    motions.Add(new SkillMotion(sequenceName, motionEffect, strTagEffects));
                    metadata.SkillLevel.Add(new SkillLevel(level, spirit, upgradeLevel, upgradeSkillId, upgradeSkillLevel, motions));
                }

                skillList.Add(metadata);
            }
            return skillList;
        }

        public static void Write(List<SkillMetadata> skills)
        {
            using (FileStream writeStream = File.OpenWrite(OUTPUT))
            {
                Serializer.Serialize(writeStream, skills);
            }
            using (FileStream readStream = File.OpenRead(OUTPUT))
            {
                // Ensure the file is read equivalent
                // Debug.Assert(skills.SequenceEqual(Serializer.Deserialize<List<SkillMetadata>>(readStream)));
            }
            Console.WriteLine("Successfully parsed item metadata!");
        }
    }
}
