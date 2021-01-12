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
    public static class PrestigeParser
    {
        public static PrestigeMetadata Parse(MemoryMappedFile m2dFile, IEnumerable<PackFileEntry> entries)
        {
            PrestigeMetadata prestigeMetadata = new PrestigeMetadata();

            foreach (PackFileEntry entry in entries)
            {
                // Only check for reward for now, abilities will need to be added later
                if (!entry.Name.StartsWith("table/adventurelevelreward"))
                {
                    continue;
                }

                // Parse XML
                XmlDocument document = m2dFile.GetDocument(entry.FileHeader);
                XmlNodeList rewards = document.SelectNodes("/ms2/reward");

                foreach (XmlNode reward in rewards)
                {
                    int level = int.Parse(reward.Attributes["level"].Value);
                    string type = reward.Attributes["type"].Value;
                    int id = int.Parse(reward.Attributes["id"].Value.Length == 0 ? "0" : reward.Attributes["id"].Value);
                    int value = int.Parse(reward.Attributes["value"].Value);

                    prestigeMetadata.Rewards.Add(new PrestigeReward(level, type, id, value));
                }
            }

            return prestigeMetadata;
        }

        public static void Write(PrestigeMetadata prestigeMetadata)
        {
            using (FileStream writeStream = File.Create(VariableDefines.OUTPUT + "ms2-prestige-metadata"))
            {
                Serializer.Serialize(writeStream, prestigeMetadata);
            }
            using (FileStream readStream = File.OpenRead(VariableDefines.OUTPUT + "ms2-prestige-metadata"))
            {
                // Ensure the file is read equivalent
                // Debug.Assert(skills.SequenceEqual(Serializer.Deserialize<List<SkillMetadata>>(readStream)));
            }
            Console.WriteLine("Successfully parsed prestige metadata!");
        }
    }
}
