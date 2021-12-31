using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class SurvivalSilverPassRewardParser : Exporter<List<SurvivalSilverPassRewardMetadata>>
{
    public SurvivalSilverPassRewardParser(MetadataResources resources) : base(resources, "survival-silver-pass-reward") { }

    protected override List<SurvivalSilverPassRewardMetadata> Parse()
    {
        List<SurvivalSilverPassRewardMetadata> rewards = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.Equals("table/na/survivalpassreward.xml"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {
                if (node.Name != "survivalPassReward")
                {
                    continue;
                }

                if (int.Parse(node.Attributes["id"].Value) > 1)
                {
                    continue;
                }

                SurvivalSilverPassRewardMetadata metadata = new()
                {
                    Level = int.Parse(node.Attributes["level"].Value),
                    Type1 = node.Attributes["type1"].Value,
                    Id1 = node.Attributes["id1"].Value,
                    Value1 = node.Attributes["value1"].Value,
                    Count1 = node.Attributes["count1"].Value
                };

                rewards.Add(metadata);
            }
        }

        return rewards;
    }
}
