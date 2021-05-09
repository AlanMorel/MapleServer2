using System.Collections.Generic;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class GuildContributionParser : Exporter<List<GuildContributionMetadata>>
    {
        public GuildContributionParser(MetadataResources resources) : base(resources, "guild-contribution") { }

        protected override List<GuildContributionMetadata> Parse()
        {
            List<GuildContributionMetadata> guildContribution = new List<GuildContributionMetadata>();
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {
                if (!entry.Name.StartsWith("table/guildcontribution"))
                {
                    continue;
                }

                // Parse XML
                XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                XmlNodeList contributions = document.SelectNodes("/ms2/contribution");

                foreach (XmlNode contribution in contributions)
                {
                    GuildContributionMetadata metadata = new GuildContributionMetadata();
                    metadata.Type = contribution.Attributes["type"].Value;
                    metadata.Value = int.Parse(contribution.Attributes["value"].Value);

                    guildContribution.Add(metadata);
                }
            }
            return guildContribution;
        }
    }
}
