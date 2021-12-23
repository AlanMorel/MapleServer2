using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class GuildContributionParser : Exporter<List<GuildContributionMetadata>>
{
    public GuildContributionParser(MetadataResources resources) : base(resources, "guild-contribution") { }

    protected override List<GuildContributionMetadata> Parse()
    {
        List<GuildContributionMetadata> guildContribution = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/guildcontribution"))
            {
                continue;
            }

            // Parse XML
            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList contributions = document.SelectNodes("/ms2/contribution");

            foreach (XmlNode contribution in contributions)
            {
                GuildContributionMetadata metadata = new()
                {
                    Type = contribution.Attributes["type"].Value,
                    Value = int.Parse(contribution.Attributes["value"].Value)
                };

                guildContribution.Add(metadata);
            }
        }

        return guildContribution;
    }
}
