using System.Collections.Generic;
using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class PremiumClubDailyBenefitParser : Exporter<List<PremiumClubDailyBenefitMetadata>>
    {
        public PremiumClubDailyBenefitParser(MetadataResources resources) : base(resources, "premium-club-daily-benefit") { }

        protected override List<PremiumClubDailyBenefitMetadata> Parse()
        {
            List<PremiumClubDailyBenefitMetadata> benefit = new List<PremiumClubDailyBenefitMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.StartsWith("table/vipbenefititemtable"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    PremiumClubDailyBenefitMetadata metadata = new PremiumClubDailyBenefitMetadata();

                    if (node.Name == "benefit")
                    {
                        metadata.BenefitId = int.Parse(node.Attributes["id"].Value);
                        metadata.ItemId = int.Parse(node.Attributes["itemID"].Value);
                        metadata.ItemAmount = short.Parse(node.Attributes["itemCount"].Value);
                        metadata.ItemRarity = byte.Parse(node.Attributes["itemRank"].Value);
                        benefit.Add(metadata);
                    }
                }
            }
            return benefit;
        }
    }
}

