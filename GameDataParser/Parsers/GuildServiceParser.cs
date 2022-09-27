using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Parsers.Helpers;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class GuildServiceParser : Exporter<List<GuildServiceMetadata>>
{
    public GuildServiceParser(MetadataResources resources) : base(resources, MetadataName.GuildService) { }

    protected override List<GuildServiceMetadata> Parse()
    {
        List<GuildServiceMetadata> services = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/guildnpc"))
            {
                continue;
            }

            // Parse XML
            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList? properties = document.SelectNodes("/ms2/npc");
            if (properties is null)
            {
                continue;
            }

            foreach (XmlNode property in properties)
            {
                string locale = property.Attributes?["locale"]?.Value ?? "";

                if (locale != "NA" && locale != "")
                {
                    continue;
                }

                if (ParserHelper.CheckForNull(property, "stringKey", "type", "level", "upgradeCost", "requireGuildLevel", "requireHouseLevel"))
                {
                    continue;
                }

                GuildServiceMetadata metadata = new()
                {
                    Id = int.Parse(property.Attributes!["stringKey"]!.Value),
                    Type = property.Attributes["type"]!.Value,
                    Level = int.Parse(property.Attributes["level"]!.Value),
                    UpgradeCost = int.Parse(property.Attributes["upgradeCost"]!.Value),
                    LevelRequirement = int.Parse(property.Attributes["requireGuildLevel"]!.Value),
                    HouseLevelRequirement = int.Parse(property.Attributes["requireHouseLevel"]!.Value)
                };

                services.Add(metadata);
            }
        }

        return services;
    }
}
