using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class PrestigeParser : Exporter<PrestigeMetadata>
    {
        public PrestigeParser(MetadataResources resources) : base(resources, "prestige") { }

        protected override PrestigeMetadata Parse()
        {
            PrestigeMetadata prestigeMetadata = new PrestigeMetadata();

            foreach (PackFileEntry entry in Resources.XmlFiles)
            {
                // Only check for reward for now, abilities will need to be added later
                if (!entry.Name.StartsWith("table/adventurelevelreward"))
                {
                    continue;
                }

                // Parse XML
                XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                XmlNodeList rewards = document.SelectNodes("/ms2/reward");

                foreach (XmlNode reward in rewards)
                {
                    int level = int.Parse(reward.Attributes["level"].Value);
                    string type = reward.Attributes["type"].Value;
                    int id = int.Parse(reward.Attributes["id"].Value.Length == 0 ? "0" : reward.Attributes["id"].Value);
                    int value = int.Parse(reward.Attributes["value"].Value);

                    prestigeMetadata.Rewards.Add(new PrestigeReward(level, type, id, value));
                }
            }

            return prestigeMetadata;
        }
    }
}
