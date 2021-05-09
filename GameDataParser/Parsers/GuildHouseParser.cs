﻿using System.Collections.Generic;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class GuildHouseParser : Exporter<List<GuildHouseMetadata>>
    {
        public GuildHouseParser(MetadataResources resources) : base(resources, "guild-house") { }

        protected override List<GuildHouseMetadata> Parse()
        {
            List<GuildHouseMetadata> houses = new List<GuildHouseMetadata>();
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {
                if (!entry.Name.StartsWith("table/guildhouse"))
                {
                    continue;
                }

                // Parse XML
                XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                XmlNodeList properties = document.SelectNodes("/ms2/guildHouse");

                foreach (XmlNode property in properties)
                {
                    GuildHouseMetadata metadata = new GuildHouseMetadata();
                    metadata.FieldId = int.Parse(property.Attributes["fieldID"].Value);
                    metadata.Level = int.Parse(property.Attributes["level"].Value);
                    metadata.Theme = int.Parse(property.Attributes["theme"].Value);
                    metadata.RequiredLevel = int.Parse(property.Attributes["upgradeReqGuildLevel"].Value);
                    metadata.UpgradeCost = int.Parse(property.Attributes["upgradeCost"].Value);
                    metadata.RethemeCost = int.Parse(property.Attributes["rethemeCost"].Value);

                    houses.Add(metadata);
                }
            }
            return houses;
        }
    }
}
