﻿using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Parsers.Helpers;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class GuildHouseParser : Exporter<List<GuildHouseMetadata>>
{
    public GuildHouseParser(MetadataResources resources) : base(resources, MetadataName.GuildHouse) { }

    protected override List<GuildHouseMetadata> Parse()
    {
        List<GuildHouseMetadata> houses = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/guildhouse"))
            {
                continue;
            }

            // Parse XML
            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList? properties = document.SelectNodes("/ms2/guildHouse");

            if (properties is null)
            {
                continue;
            }

            foreach (XmlNode property in properties)
            {
                if (ParserHelper.CheckForNull(property, "fieldID", "level", "theme", "upgradeReqGuildLevel", "upgradeCost", "rethemeCost"))
                {
                    continue;
                }

                GuildHouseMetadata metadata = new()
                {
                    FieldId = int.Parse(property.Attributes!["fieldID"]!.Value),
                    Level = int.Parse(property.Attributes["level"]!.Value),
                    Theme = int.Parse(property.Attributes["theme"]!.Value),
                    RequiredLevel = int.Parse(property.Attributes["upgradeReqGuildLevel"]!.Value),
                    UpgradeCost = int.Parse(property.Attributes["upgradeCost"]!.Value),
                    RethemeCost = int.Parse(property.Attributes["rethemeCost"]!.Value)
                };

                houses.Add(metadata);
            }
        }

        return houses;
    }
}
