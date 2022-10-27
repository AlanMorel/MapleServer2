using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class PrestigeRewardParser : Exporter<List<PrestigeRewardMetadata>>
{
    public PrestigeRewardParser(MetadataResources resources) : base(resources, MetadataName.PrestigeReward) { }

    protected override List<PrestigeRewardMetadata> Parse()
    {
        List<PrestigeRewardMetadata> metadatas = new();

        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            // Only check for reward for now, abilities will need to be added later
            if (!entry.Name.StartsWith("table/adventurelevelreward"))
            {
                continue;
            }

            // Parse XML
            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList rewards = document.SelectNodes("/ms2/reward");

            foreach (XmlNode reward in rewards)
            {
                int level = int.Parse(reward.Attributes["level"].Value);
                string type = reward.Attributes["type"].Value;
                int id = int.Parse(reward.Attributes["id"].Value == "" ? "0" : reward.Attributes["id"].Value);
                int rarity = int.Parse(reward.Attributes["rank"]?.Value == "" ? "0" : reward.Attributes["rank"].Value);
                int amount = int.Parse(reward.Attributes["value"].Value);

                PrestigeRewardMetadata metadata = new(level, type, id, rarity, amount);
                metadatas.Add(metadata);
            }
        }

        return metadatas;
    }
}
