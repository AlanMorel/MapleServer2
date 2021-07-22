using System.Collections.Generic;
using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class GuildPropertyParser : Exporter<List<GuildPropertyMetadata>>
    {
        public GuildPropertyParser(MetadataResources resources) : base(resources, "guild-property") { }

        protected override List<GuildPropertyMetadata> Parse()
        {
            List<GuildPropertyMetadata> guildProperties = new List<GuildPropertyMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.StartsWith("table/guildproperty"))
                {
                    continue;
                }

                // Parse XML
                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                XmlNodeList properties = document.SelectNodes("/ms2/property");

                foreach (XmlNode property in properties)
                {
                    GuildPropertyMetadata metadata = new GuildPropertyMetadata();
                    metadata.Level = int.Parse(property.Attributes["level"].Value);
                    metadata.AccumExp = int.Parse(property.Attributes["accumExp"].Value);
                    metadata.Capacity = int.Parse(property.Attributes["capacity"].Value);
                    metadata.FundMax = int.Parse(property.Attributes["fundMax"].Value);
                    metadata.DonationMax = int.Parse(property.Attributes["donationMax"].Value);
                    metadata.AttendExp = int.Parse(property.Attributes["attendGuildExp"].Value);
                    metadata.WinMiniGameExp = int.Parse(property.Attributes["winMiniGameGuildExp"].Value);
                    metadata.LoseMiniGameExp = int.Parse(property.Attributes["loseMiniGameGuildExp"].Value);
                    metadata.RaidGuildExp = int.Parse(property.Attributes["raidGuildExp"].Value);
                    metadata.AttendFunds = int.Parse(property.Attributes["attendGuildFund"].Value);
                    metadata.WinMiniGameFunds = int.Parse(property.Attributes["winMiniGameGuildFund"].Value);
                    metadata.LoseMiniGameFunds = int.Parse(property.Attributes["loseMiniGameGuildFund"].Value);
                    metadata.RaidGuildFunds = int.Parse(property.Attributes["raidGuildFund"].Value);
                    metadata.AttendUserExpFactor = int.Parse(property.Attributes["attendUserExpFactor"].Value);
                    metadata.DonateUserExpFactor = float.Parse(property.Attributes["donationUserExpFactor"].Value);
                    metadata.AttendGuildCoin = int.Parse(property.Attributes["attendGuildCoin"].Value);
                    metadata.DonateGuildCoin = int.Parse(property.Attributes["donateGuildCoin"].Value);
                    metadata.WinMiniGameGuildCoin = int.Parse(property.Attributes["winMiniGameGuildCoin"].Value);
                    metadata.LoseMiniGameGuildCoin = int.Parse(property.Attributes["loseMiniGameGuildCoin"].Value);

                    guildProperties.Add(metadata);
                }
            }
            return guildProperties;
        }
    }
}
