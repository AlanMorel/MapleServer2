using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Parsers.Helpers;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class FurnishingShopParser : Exporter<List<FurnishingShopMetadata>>
{
    public FurnishingShopParser(MetadataResources resources) : base(resources, MetadataName.FurnishingShop) { }

    protected override List<FurnishingShopMetadata> Parse()
    {
        List<FurnishingShopMetadata> furnishingShops = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            // files shop_maid and shop_ugcall
            if (!entry.Name.StartsWith("table/na/shop_"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList? nodes = document.SelectNodes("/ms2/key");
            if (nodes is null)
            {
                continue;
            }

            foreach (XmlNode node in nodes)
            {
                if (ParserHelper.CheckForNull(node, "id", "ugcHousingBuy", "ugcHousingMoneyType", "ugcHousingDefaultPrice"))
                {
                    continue;
                }

                FurnishingShopMetadata metadata = new()
                {
                    ItemId = int.Parse(node.Attributes!["id"]!.Value),
                    Buyable = byte.Parse(node.Attributes["ugcHousingBuy"]!.Value) == 1,
                    FurnishingTokenType = byte.Parse(node.Attributes["ugcHousingMoneyType"]!.Value),
                    Price = int.Parse(node.Attributes["ugcHousingDefaultPrice"]!.Value)
                };

                furnishingShops.Add(metadata);
            }
        }

        return furnishingShops;
    }
}
