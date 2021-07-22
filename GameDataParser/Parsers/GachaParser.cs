using System.Collections.Generic;
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
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    if (node.Name == "individualDropBox")
                    {
                        int id = int.Parse(node.Attributes["individualDropBoxID"].Value);
                        int itemId = int.Parse(node.Attributes["item"].Value);
                        byte smartDrop = byte.Parse(node.Attributes["smartDropRate"].Value);
                        bool smartGender = false;
                        if (node.Attributes["isApplySmartGenderDrop"] != null)
                        {
                            smartGender = bool.Parse(node.Attributes["isApplySmartGenderDrop"].Value);
                        }
                        short minAmount = short.Parse(node.Attributes["minCount"].Value);
                        short maxAmount = short.Parse(node.Attributes["maxCount"].Value);
                        byte rarity = byte.Parse(node.Attributes["PackageUIShowGrade"].Value);

                        GachaContent metadata = new GachaContent()
                        {
                            ItemId = itemId,
                            SmartDrop = smartDrop,
                            SmartGender = smartGender,
                            MinAmount = minAmount,
                            MaxAmount = maxAmount,
                            Rarity = rarity,
                        };

                        if (gachaContent.ContainsKey(id))
                        {
                            gachaContent[id].Add(metadata);
                        }
                        else
                        {
                            gachaContent[id] = new List<GachaContent>() { metadata };
                        }
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
                    if (node.Name == "randomBox")
                    {
                        int gachaId = int.Parse(node.Attributes["randomBoxID"].Value);
                        byte boxGroup = byte.Parse(node.Attributes["randomBoxGroup"].Value);
                        int dropBoxId = int.Parse(node.Attributes["individualDropBoxID"].Value);
                        int shopId = string.IsNullOrEmpty(node.Attributes["shopID"].Value) ? 0 : int.Parse(node.Attributes["shopID"].Value);
                        int coinItemId = string.IsNullOrEmpty(node.Attributes["coinItemID"].Value) ? 0 : int.Parse(node.Attributes["coinItemID"].Value);
                        byte coinAmount = (byte) (string.IsNullOrEmpty(node.Attributes["coinItemAmount"].Value) ? 0 : byte.Parse(node.Attributes["coinItemAmount"].Value));

                        GachaMetadata metadata = new GachaMetadata()
                        {
                            GachaId = gachaId,
                            BoxGroup = boxGroup,
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
            }
            return gacha;
        }
    }
}

