using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class RewardContentParser : Exporter<List<RewardContentMetadata>>
    {
        public RewardContentParser(MetadataResources resources) : base(resources, "reward-content") { }

        protected override List<RewardContentMetadata> Parse()
        {
            List<RewardContentMetadata> rewards = new List<RewardContentMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.StartsWith("table/rewardcontentitemtable"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                XmlNodeList tableNodes = document.GetElementsByTagName("table");
                foreach (XmlNode tableNode in tableNodes)
                {
                    RewardContentMetadata metadata = new RewardContentMetadata();

                    metadata.Id = int.Parse(tableNode.Attributes["itemTableID"].Value);

                    foreach (XmlNode childNode in tableNode)
                    {
                        RewardContentItemMetadata metadataItem = new RewardContentItemMetadata();

                        metadataItem.MinLevel = int.Parse(childNode.Attributes["minLevel"]?.Value ?? "0");
                        metadataItem.MaxLevel = int.Parse(childNode.Attributes["maxLevel"]?.Value ?? "0");

                        foreach (XmlNode itemNode in childNode)
                        {
                            int itemId = int.Parse(itemNode.Attributes["itemID"].Value);
                            int amount = int.Parse(itemNode.Attributes["count"].Value);
                            int rarity = int.Parse(itemNode.Attributes["grade"].Value);
                            metadataItem.Items.Add(new RewardItemData(itemId, amount, rarity));
                        }
                        metadata.RewardItems.Add(metadataItem);
                    }
                    rewards.Add(metadata);
                }
            }
            return rewards;
        }
    }
}
