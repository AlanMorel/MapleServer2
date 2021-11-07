using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class PremiumClubDailyBenefitParser : Exporter<List<PremiumClubDailyBenefitMetadata>>
{
    public PremiumClubDailyBenefitParser(MetadataResources resources) : base(resources, "premium-club-daily-benefit") { }

    protected override List<PremiumClubDailyBenefitMetadata> Parse()
    {
        List<PremiumClubDailyBenefitMetadata> metadataList = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/vipbenefititemtable"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList nodes = document.SelectNodes("/ms2/benefit");

            foreach (XmlNode node in nodes)
            {
                PremiumClubDailyBenefitMetadata metadata = new();

                metadata.BenefitId = int.Parse(node.Attributes["id"].Value);
                metadata.ItemId = int.Parse(node.Attributes["itemID"].Value);
                metadata.ItemAmount = short.Parse(node.Attributes["itemCount"].Value);
                metadata.ItemRarity = byte.Parse(node.Attributes["itemRank"].Value);

                metadataList.Add(metadata);
            }
        }
        return metadataList;
    }
}
