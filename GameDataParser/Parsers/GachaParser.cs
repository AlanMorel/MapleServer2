using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class GachaParser : Exporter<List<GachaMetadata>>
    {
        public GachaParser(MetadataResources resources) : base(resources, "gacha") { }

        protected override List<GachaMetadata> Parse()
        {
            Dictionary<int, List<GachaContent>> gachaContent = new Dictionary<int, List<GachaContent>>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.StartsWith("table/individualitemdrop_newgacha"))  // Capsules
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                XmlNodeList nodes = document.SelectNodes("/ms2/individualDropBox");

                foreach (XmlNode node in nodes)
                {
                    GachaContent metadata = new GachaContent()
                    {
                        ItemId = int.Parse(node.Attributes["item"].Value),
                        SmartDrop = byte.Parse(node.Attributes["smartDropRate"].Value),
                        SmartGender = bool.Parse(node.Attributes["isApplySmartGenderDrop"]?.Value ?? "false"),
                        MinAmount = short.Parse(node.Attributes["minCount"].Value),
                        MaxAmount = short.Parse(node.Attributes["maxCount"].Value),
                        Rarity = byte.Parse(node.Attributes["PackageUIShowGrade"].Value),
                    };

                    string individualDropBoxId = node.Attributes["individualDropBoxID"].Value;
                    if (gachaContent.ContainsKey(int.Parse(individualDropBoxId)))
                    {
                        gachaContent[int.Parse(individualDropBoxId)].Add(metadata);
                    }
                    else
                    {
                        gachaContent[int.Parse(individualDropBoxId)] = new List<GachaContent>() { metadata };
                    }
                }
            }

            List<GachaMetadata> gacha = new List<GachaMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {

                if (!entry.Name.StartsWith("table/gacha_info"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    if (node.Name != "randomBox")
                    {
                        continue;
                    }

                    int dropBoxId = int.Parse(node.Attributes["individualDropBoxID"].Value);
                    _ = int.TryParse(node.Attributes["shopID"]?.Value ?? "0", out int shopId);
                    _ = int.TryParse(node.Attributes["coinItemID"]?.Value ?? "0", out int coinItemId);
                    _ = byte.TryParse(node.Attributes["coinItemAmount"]?.Value ?? "0", out byte coinAmount);

                    GachaMetadata metadata = new GachaMetadata()
                    {
                        GachaId = int.Parse(node.Attributes["randomBoxID"].Value),
                        BoxGroup = byte.Parse(node.Attributes["randomBoxGroup"].Value),
                        DropBoxId = dropBoxId,
                        ShopId = shopId,
                        CoinId = coinItemId,
                        CoinAmount = coinAmount
                    };

                    if (gachaContent.ContainsKey(dropBoxId))
                    {
                        metadata.Contents = gachaContent[dropBoxId];
                    }

                    gacha.Add(metadata);
                }
            }
            return gacha;
        }
    }
}

