using System.Collections.Generic;
using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class FurnishingShopParser : Exporter<List<FurnishingShopMetadata>>
    {
        public FurnishingShopParser(MetadataResources resources) : base(resources, "furnishing-shop") { }

        protected override List<FurnishingShopMetadata> Parse()
        {
            List<FurnishingShopMetadata> furnishingShops = new List<FurnishingShopMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.StartsWith("table/na/shop_ugcall"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                XmlNodeList nodes = document.SelectNodes("/ms2/key");
                foreach (XmlNode node in nodes)
                {
                    FurnishingShopMetadata metadata = new FurnishingShopMetadata();

                    metadata.ItemId = int.Parse(node.Attributes["id"].Value);
                    byte buyable = byte.Parse(node.Attributes["ugcHousingBuy"].Value);
                    if (buyable == 1)
                    {
                        metadata.Buyable = true;
                    }
                    metadata.FurnishingTokenType = byte.Parse(node.Attributes["ugcHousingMoneyType"].Value);
                    metadata.Price = int.Parse(node.Attributes["ugcHousingDefaultPrice"].Value);

                    furnishingShops.Add(metadata);
                }
            }
            return furnishingShops;
        }
    }
}

