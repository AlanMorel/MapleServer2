using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class PremiumClubPackageParser : Exporter<List<PremiumClubPackageMetadata>>
{
    public PremiumClubPackageParser(MetadataResources resources) : base(resources, "premium-club-package") { }

    protected override List<PremiumClubPackageMetadata> Parse()
    {
        List<PremiumClubPackageMetadata> package = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/vipgoodstable"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList nodes = document.SelectNodes("/ms2/goods");

            foreach (XmlNode node in nodes)
            {
                PremiumClubPackageMetadata metadata = new()
                {
                    Id = int.Parse(node.Attributes["id"].Value),
                    VipPeriod = int.Parse(node.Attributes["vipPeriod"].Value),
                    Price = int.Parse(node.Attributes["price"].Value),
                    BuyLimit = byte.Parse(node.Attributes["buyLimit"].Value)
                };

                BonusItem bonusItem = new();

                string[] itemId = node.Attributes["bonusItemID"].Value.Split(",");
                string[] itemRarity = node.Attributes["bonusItemRank"].Value.Split(",");
                string[] itemAmount = node.Attributes["bonusItemCount"].Value.Split(",");

                for (int i = 0; i < itemId.Length; i++)
                {
                    if (itemId[i] == "") // filter out blanks
                    {
                        continue;
                    }

                    bonusItem.Id = int.Parse(itemId[i]);
                    bonusItem.Rarity = byte.Parse(itemRarity[i]);
                    bonusItem.Amount = short.Parse(itemAmount[i]);

                    metadata.BonusItem.Add(bonusItem);
                }

                package.Add(metadata);
            }
        }

        return package;
    }
}
