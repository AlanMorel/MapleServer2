using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class GuildPropertyParser : Exporter<List<GuildPropertyMetadata>>
{
    public GuildPropertyParser(MetadataResources resources) : base(resources, "guild-property") { }

    protected override List<GuildPropertyMetadata> Parse()
    {
        List<GuildPropertyMetadata> guildProperties = new();
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
                GuildPropertyMetadata metadata = new()
                {
                    Level = int.Parse(property.Attributes["level"].Value),
                    AccumExp = int.Parse(property.Attributes["accumExp"].Value),
                    Capacity = int.Parse(property.Attributes["capacity"].Value),
                    FundMax = int.Parse(property.Attributes["fundMax"].Value),
                    DonationMax = int.Parse(property.Attributes["donationMax"].Value),
                    AttendExp = int.Parse(property.Attributes["attendGuildExp"].Value),
                    WinMiniGameExp = int.Parse(property.Attributes["winMiniGameGuildExp"].Value),
                    LoseMiniGameExp = int.Parse(property.Attributes["loseMiniGameGuildExp"].Value),
                    RaidGuildExp = int.Parse(property.Attributes["raidGuildExp"].Value),
                    AttendFunds = int.Parse(property.Attributes["attendGuildFund"].Value),
                    WinMiniGameFunds = int.Parse(property.Attributes["winMiniGameGuildFund"].Value),
                    LoseMiniGameFunds = int.Parse(property.Attributes["loseMiniGameGuildFund"].Value),
                    RaidGuildFunds = int.Parse(property.Attributes["raidGuildFund"].Value),
                    AttendUserExpFactor = int.Parse(property.Attributes["attendUserExpFactor"].Value),
                    DonateUserExpFactor = float.Parse(property.Attributes["donationUserExpFactor"].Value),
                    AttendGuildCoin = int.Parse(property.Attributes["attendGuildCoin"].Value),
                    DonateGuildCoin = int.Parse(property.Attributes["donateGuildCoin"].Value),
                    WinMiniGameGuildCoin = int.Parse(property.Attributes["winMiniGameGuildCoin"].Value),
                    LoseMiniGameGuildCoin = int.Parse(property.Attributes["loseMiniGameGuildCoin"].Value)
                };

                guildProperties.Add(metadata);
            }
        }

        return guildProperties;
    }
}
