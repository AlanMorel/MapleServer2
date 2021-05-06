using System.Collections.Generic;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class GuildServiceParser : Exporter<List<GuildServiceMetadata>>
    {
        public GuildServiceParser(MetadataResources resources) : base(resources, "guild-service") { }

        protected override List<GuildServiceMetadata> Parse()
        {
            List<GuildServiceMetadata> services = new List<GuildServiceMetadata>();
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {
                if (!entry.Name.StartsWith("table/guildnpc"))
                {
                    continue;
                }

                // Parse XML
                XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                XmlNodeList properties = document.SelectNodes("/ms2/npc");

                foreach (XmlNode property in properties)
                {
                    string locale = string.IsNullOrEmpty(property.Attributes["locale"]?.Value) ? "" : property.Attributes["locale"].Value;

                    if (locale != "NA" && locale != "")
                    {
                        continue;
                    }

                    GuildServiceMetadata metadata = new GuildServiceMetadata();
                    metadata.Id = int.Parse(property.Attributes["stringKey"].Value);
                    metadata.Type = property.Attributes["type"].Value;
                    metadata.Level = int.Parse(property.Attributes["level"].Value);
                    metadata.UpgradeCost = int.Parse(property.Attributes["upgradeCost"].Value);
                    metadata.LevelRequirement = int.Parse(property.Attributes["requireGuildLevel"].Value);
                    metadata.HouseLevelRequirement = int.Parse(property.Attributes["requireHouseLevel"].Value);

                    services.Add(metadata);
                }
            }
            return services;
        }
    }
}
