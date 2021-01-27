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
    public static class GuildParser
    {
        public static GuildMetadata Parse(MemoryMappedFile m2dFile, IEnumerable<PackFileEntry> entries)
        {
            GuildMetadata guildMetadata = new GuildMetadata();

            foreach (PackFileEntry entry in entries)
            {
                if (!entry.Name.StartsWith("table/guildcontribution"))
                {
                    continue;
                }

                // Parse XML
                XmlDocument document = m2dFile.GetDocument(entry.FileHeader);
                XmlNodeList contributions = document.SelectNodes("/ms2/contribution");

                foreach (XmlNode contribution in contributions)
                {
                    string type = contribution.Attributes["type"].Value;
                    int value = int.Parse(contribution.Attributes["value"].Value);

                    guildMetadata.Contribution.Add(new GuildContribution(type, value));
                }
            }

            foreach (PackFileEntry entry in entries)
            {
                if (!entry.Name.StartsWith("table/guildbuff"))
                {
                    continue;
                }

                // Parse XML
                XmlDocument document = m2dFile.GetDocument(entry.FileHeader);
                XmlNodeList buffs = document.SelectNodes("/ms2/guildBuff");

                foreach (XmlNode buff in buffs)
                {
                    int id = int.Parse(buff.Attributes["id"].Value);
                    byte level = byte.Parse(buff.Attributes["level"].Value);
                    int effectId = int.Parse(buff.Attributes["additionalEffectId"].Value);
                    byte effectLevel = byte.Parse(buff.Attributes["additionalEffectLevel"].Value);
                    byte levelRequirement = byte.Parse(buff.Attributes["requireLevel"].Value);
                    int upgradeCost = int.Parse(buff.Attributes["upgradeCost"].Value);
                    int cost = int.Parse(buff.Attributes["cost"].Value);
                    short duration = short.Parse(buff.Attributes["duration"].Value);

                    guildMetadata.Buff.Add(new GuildBuff(id, level, effectId, effectLevel, levelRequirement, upgradeCost, cost, duration));
                }
            }

            return guildMetadata;
        }

        public static void Write(GuildMetadata guildMetadata)
        {
            using (FileStream writeStream = File.Create($"{Paths.OUTPUT}/ms2-guild-metadata"))
            {
                Serializer.Serialize(writeStream, guildMetadata);
            }
            using (FileStream readStream = File.OpenRead($"{Paths.OUTPUT}/ms2-guild-metadata"))
            {
                // Ensure the file is read equivalent
                // Debug.Assert(skills.SequenceEqual(Serializer.Deserialize<List<SkillMetadata>>(readStream)));
            }
            Console.WriteLine("\rSuccessfully parsed guild metadata!");
        }
    }
}
